using System;
using System.Transactions;
using Xbehave;

namespace WebApiMovieRest.IntegrationTests.Helpers
{
    public static class DisposableExtensions
    {
        public static T AutoRollback<T>(this T obj) where T : IDisposable
        {
            return (T) new TransactionScopeWrapper(obj)
                                .Using()
                                .WrappedDisposable;
        }

        internal class TransactionScopeWrapper : IDisposable
        {
            private readonly IDisposable _wrappedDisposable;
            private readonly TransactionScope _transactionScope;

            public TransactionScopeWrapper(IDisposable wrappedDisposable)
            {
                _transactionScope = new TransactionScope();
                _wrappedDisposable = wrappedDisposable;
            }

            public void Dispose()
            {
                _wrappedDisposable.Dispose();
                _transactionScope.Dispose();
            }

            public IDisposable WrappedDisposable
            {
                get { return _wrappedDisposable; }
            }
        }
    }
}