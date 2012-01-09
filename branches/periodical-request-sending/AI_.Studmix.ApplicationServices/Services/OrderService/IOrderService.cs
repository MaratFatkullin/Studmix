using AI_.Studmix.ApplicationServices.Services.OrderService.Requests;
using AI_.Studmix.ApplicationServices.Services.OrderService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.OrderService
{
    public interface IOrderService
    {
        ViewOrderResponse ViewOrder(ViewOrderRequest request);
        MakeOrderResponse MakeOrder(MakeOrderRequest request);
    }
}