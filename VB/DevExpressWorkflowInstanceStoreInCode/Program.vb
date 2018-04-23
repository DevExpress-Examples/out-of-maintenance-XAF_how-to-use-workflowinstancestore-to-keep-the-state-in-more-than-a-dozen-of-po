'Refer to the http://community.devexpress.com/blogs/eaf/archive/2011/08/24/xaf-workflow-persistence-storage.aspx blog post for more details.
Imports System
Imports DevExpress.Xpo
Imports System.Activities
Imports System.ServiceModel
Imports DevExpress.Workflow.Xpo
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Workflow.Store
Imports WorkflowActivitySharedLibrary
Imports System.ServiceModel.Activities
Imports System.Workflow.ComponentModel
Imports System.ServiceModel.Description
Imports System.Activities.DurableInstancing
Imports System.ServiceModel.Activities.Description

Namespace DevExpressWorkflowInstanceStoreInCode

    Friend Class Program
        Private Const DevExpressConnectionString As String = "Integrated Security=SSPI;Pooling=false;Data Source=.\SQLEXPRESS;Initial Catalog=Q477662;Asynchronous Processing=True"
        'const string connectionString = DevExpressConnectionString;
        Private Const hostBaseAddress As String = "http://localhost:8080/CountingService"


        Shared Sub Main(ByVal args() As String)
            ' Create service host.
            Dim host As New WorkflowServiceHost(New CountingWorkflow(), New Uri(hostBaseAddress))

            ' Add service endpoint.
            host.AddServiceEndpoint("ICountingWorkflow", New BasicHttpBinding(), "")


            ' Define SqlWorkflowInstanceStoreBehavior:
            '   Set interval to renew instance lock to 5 seconds.
            '   Set interval to check for runnable instances to 2 seconds.
            '   Instance Store does not keep instances after it is completed.
            '   Select exponential back-off algorithm when retrying to load a locked instance.
            '   Instance state information is compressed using the GZip compressing algorithm. 

            'We will not use this default instance store behavior in this particular demo.
'            
'            SqlWorkflowInstanceStoreBehavior instanceStoreBehavior = new SqlWorkflowInstanceStoreBehavior(connectionString);
'            instanceStoreBehavior.HostLockRenewalPeriod = new TimeSpan(0, 0, 5);
'            instanceStoreBehavior.RunnableInstancesDetectionPeriod = new TimeSpan(0, 0, 2);
'            instanceStoreBehavior.InstanceCompletionAction = InstanceCompletionAction.DeleteAll;
'            instanceStoreBehavior.InstanceLockedExceptionAction = InstanceLockedExceptionAction.AggressiveRetry;
'            instanceStoreBehavior.InstanceEncodingOption = InstanceEncodingOption.GZip;
'            host.Description.Behaviors.Add(instanceStoreBehavior);
'            

'DevExpress Solution Begins
            'We create the database schema as well as the required records in the service XPObjectType table (http://documentation.devexpress.com/#XPO/CustomDocument2632).
            Using session = New Session()
                session.ConnectionString = DevExpressConnectionString
                session.UpdateSchema(GetType(XpoWorkflowInstance), GetType(XpoInstanceKey))
                session.CreateObjectTypeRecords(GetType(XpoWorkflowInstance))
            End Using
            'Create and configure the DevExpress instance store behavior.
            Dim dxInstanceStoreBehavior = New WorkflowInstanceStoreBehavior(GetType(XpoWorkflowInstance), GetType(XpoInstanceKey), New XPObjectSpaceProvider(DevExpressConnectionString, Nothing))
            dxInstanceStoreBehavior.WorkflowInstanceStore.RunnableInstancesDetectionPeriod = New TimeSpan(0, 0, 2)
            dxInstanceStoreBehavior.WorkflowInstanceStore.InstanceCompletionAction = InstanceCompletionAction.DeleteAll

            'Take special note that WorkflowInstanceStore is created internally as follows:
            'WorkflowInstanceStore dxWorkflowInstanceStore = new WorkflowInstanceStore(
            '    typeof(XpoWorkflowInstance), typeof(XpoInstanceKey),
            '    new XPObjectSpaceProvider(DevExpressConnectionString, null)
            ');

            'Add the DevExpress instance store behavior to the host.
            host.Description.Behaviors.Add(dxInstanceStoreBehavior)
'DevExpress Solution Ends

            ' Open service host.
            host.Open()

            ' Create a client that sends a message to create an instance of the workflow.
            Dim client As ICountingWorkflow = ChannelFactory(Of ICountingWorkflow).CreateChannel(New BasicHttpBinding(), New EndpointAddress(hostBaseAddress))
            client.start()

            Console.WriteLine("(Press [Enter] at any time to terminate host)")
            Console.ReadLine()
            host.Close()
        End Sub
    End Class
End Namespace

