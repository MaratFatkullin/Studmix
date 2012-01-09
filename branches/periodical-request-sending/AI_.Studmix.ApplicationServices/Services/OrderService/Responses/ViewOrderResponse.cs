namespace AI_.Studmix.ApplicationServices.Services.OrderService.Responses
{
    public class ViewOrderResponse
    {
        public decimal OrderPrice { get; set; }

        public decimal UserBalance { get; set; }

        public bool IsUserCanBuyPackage { get; set; }
    }
}