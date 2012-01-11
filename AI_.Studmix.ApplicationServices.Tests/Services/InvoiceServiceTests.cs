using AI_.Studmix.ApplicationServices.Services.InvoiceService;
using AI_.Studmix.ApplicationServices.Services.InvoiceService.Requests;
using AI_.Studmix.ApplicationServices.Tests.Mocks;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;
using AI_.Studmix.Domain.Tests;
using Moq;
using Xunit;

namespace AI_.Studmix.ApplicationServices.Tests.Services
{
    public class InvoiceServiceTestFixture : TestFixtureBase
    {
        public Mock<IFinanceService> FinanceService = new Mock<IFinanceService>();
        public UnitOfWorkMock UnitOfWork = new UnitOfWorkMock();
        public InvoiceService CreateSut()
        {
            return new InvoiceService(UnitOfWork,FinanceService.Object);
        }
    }


    public class InvoiceServiceTests : InvoiceServiceTestFixture
    {
        [Fact]
        public void ReplenishBalance_Simple_InvoiceSended()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var request = new ReplenishBalanceRequest
                          {
                              Amount = 100,
                              UserName = user.UserName
                          };

            var service = CreateSut();
            // Act
            service.ReplenishBalance(request);

            // Assert
            FinanceService.Verify(s=>s.SendInvoiceToUser(user,100,It.IsAny<string>()));
        }
    }
}