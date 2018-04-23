//Refer to the http://community.devexpress.com/blogs/eaf/archive/2011/08/24/xaf-workflow-persistence-storage.aspx blog post for more details.
using System;
using DevExpress.Xpo;
using System.Activities;
using System.ServiceModel;
using DevExpress.Workflow.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Workflow.Store;
using WorkflowActivitySharedLibrary;
using System.ServiceModel.Activities;
using System.Workflow.ComponentModel;
using System.ServiceModel.Description;
using System.Activities.DurableInstancing;
using System.ServiceModel.Activities.Description;

namespace DevExpressWorkflowInstanceStoreInCode {

    class Program {
        const string DevExpressConnectionString = @"Integrated Security=SSPI;Pooling=false;Data Source=.\SQLEXPRESS;Initial Catalog=Q477662;Asynchronous Processing=True";
        //const string connectionString = DevExpressConnectionString;
        const string hostBaseAddress = "http://localhost:8080/CountingService";


        static void Main(string[] args) {
            // Create service host.
            WorkflowServiceHost host = new WorkflowServiceHost(new CountingWorkflow(), new Uri(hostBaseAddress));

            // Add service endpoint.
            host.AddServiceEndpoint("ICountingWorkflow", new BasicHttpBinding(), "");

           
            // Define SqlWorkflowInstanceStoreBehavior:
            //   Set interval to renew instance lock to 5 seconds.
            //   Set interval to check for runnable instances to 2 seconds.
            //   Instance Store does not keep instances after it is completed.
            //   Select exponential back-off algorithm when retrying to load a locked instance.
            //   Instance state information is compressed using the GZip compressing algorithm. 

            //We will not use this default instance store behavior in this particular demo.
            /*
            SqlWorkflowInstanceStoreBehavior instanceStoreBehavior = new SqlWorkflowInstanceStoreBehavior(connectionString);
            instanceStoreBehavior.HostLockRenewalPeriod = new TimeSpan(0, 0, 5);
            instanceStoreBehavior.RunnableInstancesDetectionPeriod = new TimeSpan(0, 0, 2);
            instanceStoreBehavior.InstanceCompletionAction = InstanceCompletionAction.DeleteAll;
            instanceStoreBehavior.InstanceLockedExceptionAction = InstanceLockedExceptionAction.AggressiveRetry;
            instanceStoreBehavior.InstanceEncodingOption = InstanceEncodingOption.GZip;
            host.Description.Behaviors.Add(instanceStoreBehavior);
            */
           
//DevExpress Solution Begins
            //We create the database schema as well as the required records in the service XPObjectType table (http://documentation.devexpress.com/#XPO/CustomDocument2632).
            using (var session = new Session()) {
                session.ConnectionString = DevExpressConnectionString;
                session.UpdateSchema(typeof(XpoWorkflowInstance), typeof(XpoInstanceKey));
                session.CreateObjectTypeRecords(typeof(XpoWorkflowInstance));
            }
            //Create and configure the DevExpress instance store behavior.
            var dxInstanceStoreBehavior = new WorkflowInstanceStoreBehavior(
                typeof(XpoWorkflowInstance), typeof(XpoInstanceKey), DevExpressConnectionString);
            dxInstanceStoreBehavior.WorkflowInstanceStore.RunnableInstancesDetectionPeriod = new TimeSpan(0, 0, 2);
            dxInstanceStoreBehavior.WorkflowInstanceStore.InstanceCompletionAction = InstanceCompletionAction.DeleteAll;

            //Take special note that WorkflowInstanceStore is created internally as follows:
            //WorkflowInstanceStore dxWorkflowInstanceStore = new WorkflowInstanceStore(
            //    typeof(XpoWorkflowInstance), typeof(XpoInstanceKey),
            //    new XPObjectSpaceProvider(DevExpressConnectionString, null)
            //);
            
            //Add the DevExpress instance store behavior to the host.
            host.Description.Behaviors.Add(dxInstanceStoreBehavior);
//DevExpress Solution Ends

            // Open service host.
            host.Open();

            // Create a client that sends a message to create an instance of the workflow.
            ICountingWorkflow client = ChannelFactory<ICountingWorkflow>.CreateChannel(new BasicHttpBinding(), new EndpointAddress(hostBaseAddress));
            client.start();

            Console.WriteLine("(Press [Enter] at any time to terminate host)");
            Console.ReadLine();
            host.Close();
        }
    }
}

