using Moq;
using Proge.WinFormsApp;
using Proge.WinFormsApp.Api;
using Xunit;

namespace Proge.WinFormsApp.UnitTests
{
    public class BudgetPresenterTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<IBudgetView> _mockView;
        private readonly BudgetPresenter _presenter;

        public BudgetPresenterTests()
        {
            _mockApiClient = new Mock<IApiClient>();
            _mockView = new Mock<IBudgetView>();

            _mockApiClient
                .Setup(x => x.List())
                .ReturnsAsync(new Result<List<Budget>> { Value = new List<Budget>() });

            _presenter = new BudgetPresenter(_mockView.Object, _mockApiClient.Object);
        }

        [Fact]
        public async Task Load_ShouldSetBudgetsOnView()
        {
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Client = "Klient A", Date = DateTime.Today, ServiceCost = 100, TotalCost = 200 },
                new Budget { Id = 2, Client = "Klient B", Date = DateTime.Today, ServiceCost = 200, TotalCost = 400 }
            };
            _mockApiClient.Setup(x => x.List()).ReturnsAsync(new Result<List<Budget>> { Value = budgets });

            await _presenter.Load();

            _mockView.VerifySet(x => x.Budgets = budgets, Times.Once);
        }

        [Fact]
        public async Task Load_WhenApiFails_ShouldNotSetBudgets()
        {
            _mockApiClient.Setup(x => x.List()).ReturnsAsync(new Result<List<Budget>> { Error = "API viga" });

            await _presenter.Load();

            _mockView.VerifySet(x => x.Budgets = It.IsAny<IList<Budget>>(), Times.Never);
        }

        [Fact]
        public void UpdateView_WhenBudgetIsNull_ShouldClearFields()
        {
            _presenter.UpdateView(null);

            _mockView.VerifySet(x => x.Id = 0, Times.Once);
            _mockView.VerifySet(x => x.Client = string.Empty, Times.Once);
            _mockView.VerifySet(x => x.ServiceCost = 0, Times.Once);
            _mockView.VerifySet(x => x.TotalCost = 0, Times.Once);
        }

        [Fact]
        public void UpdateView_WhenBudgetIsNotNull_ShouldSetFields()
        {
            var budget = new Budget { Id = 1, Client = "Test", Date = DateTime.Today, ServiceCost = 100, TotalCost = 200 };

            _presenter.UpdateView(budget);

            _mockView.VerifySet(x => x.Id = 1, Times.Once);
            _mockView.VerifySet(x => x.Client = "Test", Times.Once);
            _mockView.VerifySet(x => x.ServiceCost = 100, Times.Once);
            _mockView.VerifySet(x => x.TotalCost = 200, Times.Once);
        }

        [Fact]
        public async Task Save_ShouldCallApiSave()
        {
            _mockView.Setup(x => x.Id).Returns(0);
            _mockView.Setup(x => x.Client).Returns("Uus klient");
            _mockView.Setup(x => x.Date).Returns(DateTime.Today);
            _mockView.Setup(x => x.ServiceCost).Returns(100);
            _mockView.Setup(x => x.TotalCost).Returns(200);
            _mockView.Setup(x => x.SelectedItem).Returns((Budget)null);

            await _presenter.Save();

            _mockApiClient.Verify(x => x.Save(It.IsAny<Budget>()), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenSelectedItemIsNull_ShouldNotCallApiDelete()
        {
            _mockView.Setup(x => x.SelectedItem).Returns((Budget)null);

            await _presenter.Delete();

            _mockApiClient.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenSelectedItemExists_ShouldCallApiDelete()
        {
            var budget = new Budget { Id = 5, Client = "Kustuta" };
            _mockView.Setup(x => x.SelectedItem).Returns(budget);

            await _presenter.Delete();

            _mockApiClient.Verify(x => x.Delete(5), Times.Once);
        }
    }
}