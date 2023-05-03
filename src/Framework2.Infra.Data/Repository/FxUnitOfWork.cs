using Framework2.Infra.Data.Context;

namespace Framework2.Infra.Data.Repository
{
    public abstract class FxUnitOfWork<TContext> : IDisposable
        where TContext : FxDbContext
    {
        protected readonly TContext _context;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly object _scope = new();
        private bool _disposed = false;

        public FxUnitOfWork(TContext context)
            => _context = context;

        public virtual async Task<TResult> Work<TResult>(Func<Task<TResult>> work)
        {
            try
            {
                await _semaphore.WaitAsync();
                return await work();
            }
            finally
            {
                await this.Save();
                _semaphore.Release();
            }
        }

        public virtual async Task Save()
            => await _context.SaveChangesAsync();

        public virtual void Dispose()
        {
            lock (_scope)
            {
                if (!_disposed)
                    _context.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
