using System;
using System.Linq;
using AI_.Studmix.ApplicationServices.Services.OrderService.Requests;
using AI_.Studmix.ApplicationServices.Services.OrderService.Responses;
using AI_.Studmix.ApplicationServices.Specifications;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.ApplicationServices.Services.OrderService
{
    public class OrderService : IOrderService
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        protected IFinanceService FinanceService { get; set; }

        public OrderService(IUnitOfWork unitOfWork, IFinanceService financeService)
        {
            UnitOfWork = unitOfWork;
            FinanceService = financeService;
        }

        public ViewOrderResponse ViewOrder(ViewOrderRequest request)
        {
            var response = new ViewOrderResponse();

            var package = UnitOfWork.GetRepository<ContentPackage>().GetByID(request.PackageID);
            var user = UnitOfWork.GetRepository<User>().Get(new GetUserByUserName(request.UserName)).Single();
            new Order(user, package);

            response.IsUserCanBuyPackage = FinanceService.UserCanBuyPackage(user, package);
            response.UserBalance = user.Balance;
            response.OrderPrice = package.Price;
            return response;
        }
    }
}