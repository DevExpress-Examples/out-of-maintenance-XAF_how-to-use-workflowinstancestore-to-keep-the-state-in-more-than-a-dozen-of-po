//Refer to the http://community.devexpress.com/blogs/eaf/archive/2011/08/24/xaf-workflow-persistence-storage.aspx blog post for more details.
using System;
using DevExpress.Xpo;
using System.ServiceModel;
using DevExpress.Workflow.Xpo;
using WorkflowActivitySharedLibrary;
using System.ServiceModel.Activities;

namespace DevExpressWorkflowInstanceStoreInConfig {

    class Program {
        const string DevExpressConnectionString = @"Integrated Security=SSPI;Pooling=false;Data Source=.\SQLEXPRESS;Initial Catalog=Q477662;Asynchronous Processing=True";
        const string hostBaseAddress = "http://localhost:8081/CountingService";

        static void Main(string[] args) {
            using (Session session = new Session()) {
                session.ConnectionString = DevExpressConnectionString;
                session.UpdateSchema(typeof(XpoWorkflowInstance), typeof(XpoInstanceKey));
                session.CreateObjectTypeRecords(typeof(XpoWorkflowInstance), typeof(XpoInstanceKey));
            }

            // Create service host.
            WorkflowServiceHost host = new WorkflowServiceHost(new CountingWorkflow(), new Uri(hostBaseAddress));

            // Add service endpoint.
            host.AddServiceEndpoint("ICountingWorkflow", new BasicHttpBinding(), "");

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

