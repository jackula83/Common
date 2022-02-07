namespace Common.Domain.Core.Data
{
    public abstract class FxUnitOfWork<TContext> : IDisposable
        where TContext : FxDbContext
    {
        protected readonly TContext _context;
        private object _scope = new object();
        private bool _disposed = false;

        public FxUnitOfWork(TContext context)
            => _context = context;

        public virtual async Task Save()
            => await _context.SaveChangesAsync();

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
