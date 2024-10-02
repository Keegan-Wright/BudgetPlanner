namespace BudgetPlanner.Models.Response
{
    public class UpcomingPaymentsResponse
    {
        public string PaymentName { get; set; }
         
        public decimal Amount { get; set; }
         
        public DateTime PaymentDate { get; set; }

        public string PaymentType { get; set; }

    }
}
