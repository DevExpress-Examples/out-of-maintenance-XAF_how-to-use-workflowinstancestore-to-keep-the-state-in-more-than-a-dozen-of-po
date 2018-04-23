'Refer to the http://community.devexpress.com/blogs/eaf/archive/2011/08/24/xaf-workflow-persistence-storage.aspx blog post for more details.

Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.ServiceModel
Imports System.Collections.Generic

Namespace WorkflowActivitySharedLibrary
	<ServiceContract> _
	Public Interface ICountingWorkflow
		<OperationContract(IsOneWay := True)> _
		Sub start()
	End Interface
End Namespace
