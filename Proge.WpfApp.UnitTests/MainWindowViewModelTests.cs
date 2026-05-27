using Moq;
using Proge.PublicAPI;
using Proge.WpfApp;

namespace Proge.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _mockApiClient = new Mock<IApiClient>();

            _mockApiClient
                .Setup(x => x.List())
                .ReturnsAsync(new List<Budget>());

            _viewModel = new MainWindowViewModel(_mockApiClient.Object);
        }

        [Fact]
        public async Task Load_ShouldFillList()
        {
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Client = "Klient A", Date = DateTime.Today, ServiceCost = 100, TotalCost = 200 },
                new Budget { Id = 2, Client = "Klient B", Date = DateTime.Today, ServiceCost = 200, TotalCost = 400 }
            };
            _mockApiClient.Setup(x => x.List()).ReturnsAsync(budgets);

            await _viewModel.Load();

            Assert.Equal(2, _viewModel.Lists.Count);
        }

        [Fact]
        public void NewCommand_ShouldSetEmptySelectedItem()
        {
            _viewModel.NewCommand.Execute(null);

            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
        }

        [Fact]
        public async Task SaveCommand_ShouldCallApiSave()
        {
            _mockApiClient.Setup(x => x.Save(It.IsAny<Budget>())).Returns(Task.CompletedTask);
            _mockApiClient.Setup(x => x.List()).ReturnsAsync(new List<Budget>());

            _viewModel.SelectedItem = new Budget { Id = 0, Client = "Test", Date = DateTime.Today };

            _viewModel.SaveCommand.Execute(null);
            await Task.Delay(100);

            _mockApiClient.Verify(x => x.Save(It.IsAny<Budget>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCommand_ShouldCallApiDelete()
        {
            var budget = new Budget { Id = 5, Client = "Kustuta mind", Date = DateTime.Today };
            _mockApiClient.Setup(x => x.Delete(5)).Returns(Task.CompletedTask);
            _mockApiClient.Setup(x => x.List()).ReturnsAsync(new List<Budget>());

            _viewModel.SelectedItem = budget;
            _viewModel.Lists.Add(budget);

            _viewModel.DeleteCommand.Execute(null);
            await Task.Delay(100);

            _mockApiClient.Verify(x => x.Delete(5), Times.Once);
        }

        [Fact]
        public void SaveCommand_WhenSelectedItemIsNull_ShouldNotExecute()
        {
            _viewModel.SelectedItem = null;

            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void DeleteCommand_WhenSelectedItemIsNull_ShouldNotExecute()
        {
            _viewModel.SelectedItem = null;

            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }
    }
}