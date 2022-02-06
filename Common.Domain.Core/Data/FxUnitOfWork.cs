using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Data
{
    public abstract class FxUnitOfWork<TContext> : IDisposable
        where TContext : FxDbContext
    {
        private readonly TContext _context;
        private object _scope = new object();
        private bool _disposed = false;

        public FxUnitOfWork(TContext context)
            => _context = context;

        public virtual void Save()
            => _context.SaveChanges();

        public virtual void Dispose()
        {
            lock(_scope)
            {
                if (!_disposed)
                    _context.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
