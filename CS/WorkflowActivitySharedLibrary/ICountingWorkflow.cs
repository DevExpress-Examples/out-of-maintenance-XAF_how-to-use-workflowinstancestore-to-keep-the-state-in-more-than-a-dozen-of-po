//Refer to the http://community.devexpress.com/blogs/eaf/archive/2011/08/24/xaf-workflow-persistence-storage.aspx blog post for more details.
using System;
using System.Linq;
using System.ServiceModel;
using System.Collections.Generic;

namespace WorkflowActivitySharedLibrary {
    [ServiceContract]
    public interface ICountingWorkflow {
        [OperationContract(IsOneWay = true)]
        void start();
    }
}
