'Refer to the http://community.devexpress.com/blogs/eaf/archive/2011/08/24/xaf-workflow-persistence-storage.aspx blog post for more details.
Imports System
Imports DevExpress.Xpo
Imports System.ServiceModel
Imports DevExpress.Workflow.Xpo
Imports WorkflowActivitySharedLibrary
Imports System.ServiceModel.Activities

Namespace DevExpressWorkflowInstanceStoreInConfig

    Friend Class Program
        Private Const DevExpressConnectionString As String = "Integrated Security=SSPI;Pooling=false;Data Source=.\SQLEXPRESS;Initial Catalog=Q477662;Asynchronous Processing=True"
        Private Const hostBaseAddress As String = "http://localhost:8081/CountingService"

        Shared Sub Main(ByVal args() As String)
            Using session As New Session()
                session.ConnectionString = DevExpressConnectionString
                session.UpdateSchema(GetType(XpoWorkflowInstance), GetType(XpoInstanceKey))
                session.CreateObjectTypeRecords(GetType(XpoWorkflowInstance), GetType(XpoInstanceKey))
            End Using

            ' Create service host.
            Dim host As New WorkflowServiceHost(New CountingWorkflow(), New Uri(hostBaseAddress))

            ' Add service endpoint.
            host.AddServiceEndpoint("ICountingWorkflow", New BasicHttpBinding(), "")

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

