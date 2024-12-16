using Sentry;

namespace BudgetPlanner.Shared.Services.Base
{
    public class InstrumentedService
    {
        public ITransactionTracer GetSentryTransaction(string name, string description, bool setScope = true)
        {
            var transaction = SentrySdk.StartTransaction(name, description);

            if (setScope)
            {
                SentrySdk.ConfigureScope(scope => scope.Transaction = transaction);
            }
            return transaction;
        }

        public ISpan GetTransactionChild(ITransactionTracer transaction, string name, string description)
        {
            return transaction.StartChild(name, description);
        }


        public void FinishTransactionChildTrace(params ISpan[] spans)
        {
            foreach (var span in spans) {
                FinishTransactionChild(span);
            }
        }

        public void FinishTransactionChildTrace(ISpan span)
        {
            FinishTransactionChild(span);
        }


        public void FinishTransaction(ITransactionTracer transaction)
        { 
            transaction.Finish();
        }

        private void FinishTransactionChild(ISpan span) {
        
        }

    }
}
