using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.MQ.Environment.Abstracts
{
    public abstract class EnvVars
    {
        protected static string BuildEnvName(string prefix, string name)
            => $"{prefix}_{name}".ToUpper();
    }
}
