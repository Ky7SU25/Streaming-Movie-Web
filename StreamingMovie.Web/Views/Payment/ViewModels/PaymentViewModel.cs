namespace StreamingMovie.Web.Views.Payment.ViewModels
{
    public class PaymentViewModel
    {
        public int UserID { get; set; }
        public string UserFullName { get; set; }
        
        public DateTime PaymentDate { get; set; }
        public DateTime? ExpDate { get; set; }
    }
}
