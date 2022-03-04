using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Interfaces
{
    public interface IEnvironment
    {
        string? Get(string key);
        void Set(string key, string value);
    }
}
