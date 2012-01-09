using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Infrastructure.PaymentSystem;
using FluentAssertions;
using Xunit;

namespace AI_.Studmix.IntegrationTests
{
    public class PaymentSystemTestFixture
    {
        public QiwiInvoiceRepository CreateSut()
        {
            return new QiwiInvoiceRepository(new PaymentSystmeProviderConfiguration());
        }
    }


    //public class PaymentSystemTests : IUseFixture<PaymentSystemTestFixture>
    //{
    //    protected PaymentSystemTestFixture Fixture { get; set; }

    //    #region IUseFixture<PaymentSystemTestFixture> Members

    //    public void SetFixture(PaymentSystemTestFixture data)
    //    {
    //        Fixture = data;
    //    }

    //    #endregion

    //    [Fact]
    //    public void CreateBill_ServiceRespondWithSuccessCode()
    //    {
    //        // Arrange
    //        var bill = new Invoice("9872854409", 100, "___Test operation___");
    //        var paymentSystem = Fixture.CreateSut();

    //        // Act
    //        var result = paymentSystem.CreateBill(bill);

    //        // Assert
    //        result.Should().Be(0);
    //    }

    //    [Fact]
    //    public void GetBillStatus_ServiceRespondWithActualStatusCode()
    //    {
    //        // Arrange
    //        var bill = new Invoice("9872854409", 100, "___Test operation___");
    //        var paymentSystem = Fixture.CreateSut();
    //        paymentSystem.CreateBill(bill);

    //        // Act
    //        var result = paymentSystem.GetBillStatus(bill.TransactionID);

    //        // Assert
    //        result.Should().Be(50);
    //    }
    //}
}