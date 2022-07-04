using Application.Order.Create;
using Common.Domain.ActionContext;
using Common.Domain.User.ValueObjects;
using Domain.Common;
using Domain.DiscountVoucher;
using Domain.DiscountVoucher.ValueObjects;
using Domain.Order;
using Domain.Product;
using Domain.Product.ValueObjects;
using Domain.ShoppingCart;
using Domain.ShoppingCartSnapshot;
using Moq;
using Xunit;

namespace Tests.Application.Order
{
    public class CreateOrderCommandHandlerUnitTests
    {
        private CreateOrderCommandHandler _sut;

        private Mock<IShoppingCartSnapshotRepository> _shoppingCartSnapshotRepositoryMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IDiscountVoucherRepository> _discountVoucherRepositoryMock;
        private Mock<IShoppingCartRepository> _shoppingCartRepositoryMock;
        private Mock<IActionContextProvider> _actionContextProviderMock;
        private Mock<IProductRepository> _productRepositoryMock;

        private readonly CreateOrderCommand _defaultCommand = new(Guid.Parse("5f9e33f4-6dbe-4556-84b6-75f2556ac260"), PaymentMethod.BankTransfer, "Bob");
        private readonly UserId _userId = new(Guid.Parse("aaa3f72a-0cce-4562-b0eb-4b5b1dbd511f"));
        private readonly Money _discountValue = new(20);

        public CreateOrderCommandHandlerUnitTests()
        {
            _shoppingCartSnapshotRepositoryMock = new Mock<IShoppingCartSnapshotRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _discountVoucherRepositoryMock = new Mock<IDiscountVoucherRepository>();
            _shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
            _actionContextProviderMock = new Mock<IActionContextProvider>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _sut = new(
                _shoppingCartSnapshotRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _discountVoucherRepositoryMock.Object,
                _shoppingCartRepositoryMock.Object,
                _actionContextProviderMock.Object,
                _productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCartIsValidValidDiscountApplied_SavesOrder()
        {
            await SetHappyPathMocks();
            await _sut.Handle(_defaultCommand, CancellationToken.None);

            _shoppingCartRepositoryMock.Verify(m => m.GetByUserIdAsync(It.Is<UserId>(u => u == _userId)), Times.Once);
            _shoppingCartSnapshotRepositoryMock.Verify(m => m.InsertAsync(It.IsAny<ShoppingCartSnapshot>()), Times.Once);
            _orderRepositoryMock.Verify(m => m.InsertAsync(It.Is<Domain.Order.Order>(o =>
                o.DiscountValue == _discountValue // remaining variebles can be shared similarly 
                && o.ItemLinesTotalValue == new Money(300) // to not refere value like here...
                                                           // and so on...
                )));
        }

        [Fact]
        public void Handle_WhenCreateOrderCommandIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.Handle(null, CancellationToken.None));
        }

        [Fact]
        public void Handle_WhenCartIsEmpty_ThrowsApplicationException()
        {
            var actionContext = new ActionContext(_userId);
            _actionContextProviderMock.SetupGet(x => x.ActionContext).Returns(actionContext);
            _shoppingCartRepositoryMock.Setup(m => m.GetByUserIdAsync(It.IsAny<UserId>())).ReturnsAsync(new ShoppingCart(_userId));

            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.Handle(_defaultCommand, CancellationToken.None));

            _shoppingCartRepositoryMock.Verify(v => v.GetByUserIdAsync(It.Is<UserId>(u => u == _userId)), Times.Once);
        }


        private async Task SetHappyPathMocks()
        {
            var actionContext = new ActionContext(_userId);
            _actionContextProviderMock.SetupGet(x => x.ActionContext).Returns(actionContext);

            ShoppingCartSnapshot shoppingCartSnapshot = new();
            ShoppingCart shoppingCrat = new(_userId);
            Product productA = new Product(new ProductId(), new Money(100));
            Product productB = new Product(new ProductId(), new Money(200));
            shoppingCrat.AddProduct(productA.Id);
            shoppingCrat.AddProduct(productB.Id);
            DiscountVoucherId discountVoucherId = new();
            var voucherCode = new Code("Ex-ple");

            DiscountVoucher discountVoucher = new(new ExpirationDate(DateTime.UtcNow.AddYears(1).Date), voucherCode, _discountValue);
            shoppingCrat.UseDiscountCode(discountVoucherId);


            _productRepositoryMock
                .Setup(m => m.GetAllPrices(It.IsAny<ICollection<ProductId>>()))
                .ReturnsAsync(new Dictionary<ProductId, Money>
                {
                    { productA.Id, productA.Price },
                    { productB.Id, productB.Price }
                });

            await shoppingCartSnapshot.PopulateCartSnapshot(shoppingCrat, _productRepositoryMock.Object);
            _discountVoucherRepositoryMock.Setup(m => m.ExistsAsync(It.Is<DiscountVoucherId>(dvi => dvi == discountVoucherId))).ReturnsAsync(true);
            _discountVoucherRepositoryMock.Setup(m => m.GetAsync(It.Is<DiscountVoucherId>(dvi => dvi == discountVoucherId))).ReturnsAsync(discountVoucher);
            _shoppingCartRepositoryMock.Setup(m => m.GetByUserIdAsync(It.Is<UserId>(uid => uid == _userId))).ReturnsAsync(shoppingCrat);
        }
    }
}
