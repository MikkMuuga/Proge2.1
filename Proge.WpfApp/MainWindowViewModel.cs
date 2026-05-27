using Proge.PublicAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Proge.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        public ObservableCollection<Budget> Lists { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Predicate<Budget> ConfirmDelete { get; set; }

        private readonly IApiClient _apiClient;

        public MainWindowViewModel() : this(new ApiClient()) { }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            Lists = new ObservableCollection<Budget>();

            NewCommand = new RelayCommand<Budget>(
                list =>
                {
                    SelectedItem = new Budget { Date = DateTime.Today };
                }
            );

            SaveCommand = new RelayCommand<Budget>(
                async list =>
                {
                    await _apiClient.Save(SelectedItem);
                    await Load();
                },
                list => SelectedItem != null
            );

            DeleteCommand = new RelayCommand<Budget>(
                async list =>
                {
                    if (ConfirmDelete != null)
                    {
                        var result = ConfirmDelete(SelectedItem);
                        if (!result) return;
                    }
                    await _apiClient.Delete(SelectedItem.Id);
                    Lists.Remove(SelectedItem);
                    SelectedItem = null;
                },
                list => SelectedItem != null
            );
        }

        public async Task Load()
        {
            Lists.Clear();
            var lists = await _apiClient.List();
            foreach (var item in lists)
            {
                Lists.Add(item);
            }
        }

        private Budget _selectedItem;
        public Budget SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }
    }
}