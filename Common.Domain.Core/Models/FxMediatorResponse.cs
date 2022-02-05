using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Models
{

    public abstract class FxMediatorResponse : FxResponse { }

    public abstract class FxQueryResponse : FxMediatorResponse { }
    public abstract class FxCommandResponse : FxMediatorResponse { }
}
