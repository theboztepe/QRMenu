using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    //[TransactionScopeAspect] method üstüne yazılacak
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using TransactionScope transactionScope = new();
            try
            {
                invocation.Proceed();
                transactionScope.Complete();
            }
            catch
            {
                transactionScope.Dispose();
                throw;
            }
        }
    }
}