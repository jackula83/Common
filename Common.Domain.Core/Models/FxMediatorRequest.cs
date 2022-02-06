using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Models
{
    public abstract class FxMediatorRequest : FxRequest
    {
        public abstract void From<TControllerRequest>(TControllerRequest request)
            where TControllerRequest : FxControllerRequest;
    }

    public abstract class FxQueryRequest : FxMediatorRequest { }
    public abstract class FxCommandRequest : FxMediatorRequest { }
}
