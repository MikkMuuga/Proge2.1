using Proge.PublicAPI;

namespace Proge.WinFormsApp
{
    public partial class Form1 : Form, IBudgetView
    {
        public IList<Budget> Budgets
        {
            get => (IList<Budget>)BudgetsGrid.DataSource;
            set => BudgetsGrid.DataSource = value;
        }

        public Budget SelectedItem { get; set; }
        public BudgetPresenter Presenter { get; set; }

        public string Client
        {
            get => ClientField.Text;
            set => ClientField.Text = value;
        }

        public DateTime Date
        {
            get => DateField.Value;
            set => DateField.Value = value;
        }

        public decimal ServiceCost
        {
            get => decimal.TryParse(ServiceCostField.Text, out var v) ? v : 0;
            set => ServiceCostField.Text = value.ToString();
        }

        public decimal TotalCost
        {
            get => decimal.TryParse(TotalCostField.Text, out var v) ? v : 0;
            set => TotalCostField.Text = value.ToString();
        }

        public int Id
        {
            get => int.TryParse(IdField.Text, out var v) ? v : 0;
            set => IdField.Text = value.ToString();
        }

        public Form1()
        {
            InitializeComponent();
            var presenter = new BudgetPresenter(this, new ApiClient());
            presenter.OnError = error =>
            {
                MessageBox.Show(error, "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            BudgetsGrid.AutoGenerateColumns = true;
            BudgetsGrid.SelectionChanged += BudgetsGrid_SelectionChanged;

            AddButton.Click += AddButton_Click;
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;

            Load += Form1_Load;
        }

        private void BudgetsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (BudgetsGrid.SelectedRows.Count == 0)
                SelectedItem = null;
            else
                SelectedItem = (Budget)BudgetsGrid.SelectedRows[0].DataBoundItem;

            Presenter.UpdateView(SelectedItem);
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            Presenter.UpdateView(null);
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            await Presenter.Save();
        }

        private async void DeleteButton_Click(object? sender, EventArgs e)
        {
            await Presenter.Delete();
        }

        private async void Form1_Load(object? sender, EventArgs e)
        {
            await Presenter.Load();
        }
    }
}