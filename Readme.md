<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128594958/12.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4547)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* **[Program.cs](./CS/DevExpressWorkflowInstanceStoreInCode/Program.cs) (VB: [Program.vb](./VB/DevExpressWorkflowInstanceStoreInCode/Program.vb))**
* [App.config](./CS/DevExpressWorkflowInstanceStoreInConfig/App.config)
<!-- default file list end -->
# How to use WorkflowInstanceStore to keep the state in more than a dozen of popular RDBMS


<p>This example is based on the <a href="http://msdn.microsoft.com/library/ee816889.aspx"><u>How to: Configure Persistence with WorkflowServiceHost</u></a> and <a href="http://msdn.microsoft.com/en-US/library/ee395773.aspx"><u>How to: Enable SQL Persistence for Workflows and Workflow Services</u></a> help articles from MSDN.</p><p>The only difference is that in code you can use the DevExpress.Workflow.Store.<strong>WorkflowInstanceStoreBehavior</strong> class (it is a part of the DevExpress.Workflow.Activities library) instead of the standard <i>SqlWorkflowInstanceStoreBehavior</i> one.</p><p>If you want to enable persistence for self-hosted workflows that use WorkflowApplication programmatically by using <a href="http://msdn.microsoft.com/en-US/library/ee395773.aspx"><u>the SqlWorkflowInstanceStore class</u></a>, you can use the built-in DevExpress.Workflow.Store.<strong>WorkflowInstanceStore</strong> one (it is also a part of the DevExpress.Workflow.Activities library):</p>

```cs
...
using DevExpress.Workflow.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Workflow.Store;
...
WorkflowInstanceStore dxWorkflowInstanceStore = new WorkflowInstanceStore(
    typeof(XpoWorkflowInstance), typeof(XpoInstanceKey),
    new XPObjectSpaceProvider(yourDatabaseConnectionString, null)
);
```

<p> </p><p>If you configure it via the configuration file, use the <strong>DevExpressWorkflowInstanceStore</strong> element instead of the standard <i>sqlWorkflowInstanceStore</i> one.</p><p><br />
Refer to the <a href="http://community.devexpress.com/blogs/eaf/archive/2011/08/24/xaf-workflow-persistence-storage.aspx"><u>XAF Workflow persistence storage</u></a> blog post for more information.</p><p><strong>See also:</strong><strong><br />
</strong><a href="https://www.devexpress.com/Support/Center/p/K18445">How to create a correct connection string for XPO providers?</a><br />
<a href="http://documentation.devexpress.com/#Xaf/CustomDocument3343"><u>eXpressApp Framework > Concepts > Extra Modules > Workflow Module</u></a></p>

<br/>


