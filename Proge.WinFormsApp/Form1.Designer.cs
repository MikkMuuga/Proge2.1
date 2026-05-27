namespace Proge.WinFormsApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            BudgetsGrid = new DataGridView();
            IdLabel = new Label();
            IdField = new TextBox();
            ClientLabel = new Label();
            ClientField = new TextBox();
            DateLabel = new Label();
            DateField = new DateTimePicker();
            ServiceCostLabel = new Label();
            ServiceCostField = new TextBox();
            TotalCostLabel = new Label();
            TotalCostField = new TextBox();
            AddButton = new Button();
            SaveButton = new Button();
            DeleteButton = new Button();
            ((System.ComponentModel.ISupportInitialize)BudgetsGrid).BeginInit();
            SuspendLayout();
            // 
            // BudgetsGrid
            // 
            BudgetsGrid.AllowUserToAddRows = false;
            BudgetsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            BudgetsGrid.Location = new Point(17, 20);
            BudgetsGrid.Margin = new Padding(4, 5, 4, 5);
            BudgetsGrid.MultiSelect = false;
            BudgetsGrid.Name = "BudgetsGrid";
            BudgetsGrid.RowHeadersWidth = 62;
            BudgetsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BudgetsGrid.Size = new Size(571, 710);
            BudgetsGrid.TabIndex = 0;
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(614, 42);
            IdLabel.Margin = new Padding(4, 0, 4, 0);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(34, 25);
            IdLabel.TabIndex = 12;
            IdLabel.Text = "ID:";
            // 
            // IdField
            // 
            IdField.Location = new Point(743, 37);
            IdField.Margin = new Padding(4, 5, 4, 5);
            IdField.Name = "IdField";
            IdField.ReadOnly = true;
            IdField.Size = new Size(355, 31);
            IdField.TabIndex = 11;
            // 
            // ClientLabel
            // 
            ClientLabel.AutoSize = true;
            ClientLabel.Location = new Point(614, 100);
            ClientLabel.Margin = new Padding(4, 0, 4, 0);
            ClientLabel.Name = "ClientLabel";
            ClientLabel.Size = new Size(60, 25);
            ClientLabel.TabIndex = 10;
            ClientLabel.Text = "Client:";
            // 
            // ClientField
            // 
            ClientField.Location = new Point(743, 95);
            ClientField.Margin = new Padding(4, 5, 4, 5);
            ClientField.Name = "ClientField";
            ClientField.Size = new Size(355, 31);
            ClientField.TabIndex = 9;
            // 
            // DateLabel
            // 
            DateLabel.AutoSize = true;
            DateLabel.Location = new Point(614, 158);
            DateLabel.Margin = new Padding(4, 0, 4, 0);
            DateLabel.Name = "DateLabel";
            DateLabel.Size = new Size(53, 25);
            DateLabel.TabIndex = 8;
            DateLabel.Text = "Date:";
            // 
            // DateField
            // 
            DateField.Location = new Point(743, 153);
            DateField.Margin = new Padding(4, 5, 4, 5);
            DateField.Name = "DateField";
            DateField.Size = new Size(355, 31);
            DateField.TabIndex = 7;
            // 
            // ServiceCostLabel
            // 
            ServiceCostLabel.AutoSize = true;
            ServiceCostLabel.Location = new Point(614, 217);
            ServiceCostLabel.Margin = new Padding(4, 0, 4, 0);
            ServiceCostLabel.Name = "ServiceCostLabel";
            ServiceCostLabel.Size = new Size(112, 25);
            ServiceCostLabel.TabIndex = 6;
            ServiceCostLabel.Text = "Service Cost:";
            // 
            // ServiceCostField
            // 
            ServiceCostField.Location = new Point(743, 212);
            ServiceCostField.Margin = new Padding(4, 5, 4, 5);
            ServiceCostField.Name = "ServiceCostField";
            ServiceCostField.Size = new Size(355, 31);
            ServiceCostField.TabIndex = 5;
            // 
            // TotalCostLabel
            // 
            TotalCostLabel.AutoSize = true;
            TotalCostLabel.Location = new Point(614, 275);
            TotalCostLabel.Margin = new Padding(4, 0, 4, 0);
            TotalCostLabel.Name = "TotalCostLabel";
            TotalCostLabel.Size = new Size(94, 25);
            TotalCostLabel.TabIndex = 4;
            TotalCostLabel.Text = "Total Cost:";
            // 
            // TotalCostField
            // 
            TotalCostField.Location = new Point(743, 270);
            TotalCostField.Margin = new Padding(4, 5, 4, 5);
            TotalCostField.Name = "TotalCostField";
            TotalCostField.Size = new Size(355, 31);
            TotalCostField.TabIndex = 3;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(614, 350);
            AddButton.Margin = new Padding(4, 5, 4, 5);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(107, 38);
            AddButton.TabIndex = 2;
            AddButton.Text = "Add new";
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(743, 350);
            SaveButton.Margin = new Padding(4, 5, 4, 5);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(107, 38);
            SaveButton.TabIndex = 1;
            SaveButton.Text = "Save";
            // 
            // DeleteButton
            // 
            DeleteButton.Location = new Point(871, 350);
            DeleteButton.Margin = new Padding(4, 5, 4, 5);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(107, 38);
            DeleteButton.TabIndex = 0;
            DeleteButton.Text = "Delete";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1300, 767);
            Controls.Add(DeleteButton);
            Controls.Add(SaveButton);
            Controls.Add(AddButton);
            Controls.Add(TotalCostField);
            Controls.Add(TotalCostLabel);
            Controls.Add(ServiceCostField);
            Controls.Add(ServiceCostLabel);
            Controls.Add(DateField);
            Controls.Add(DateLabel);
            Controls.Add(ClientField);
            Controls.Add(ClientLabel);
            Controls.Add(IdField);
            Controls.Add(IdLabel);
            Controls.Add(BudgetsGrid);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Budgets";
            ((System.ComponentModel.ISupportInitialize)BudgetsGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView BudgetsGrid;
        private Label IdLabel;
        private TextBox IdField;
        private Label ClientLabel;
        private TextBox ClientField;
        private Label DateLabel;
        private DateTimePicker DateField;
        private Label ServiceCostLabel;
        private TextBox ServiceCostField;
        private Label TotalCostLabel;
        private TextBox TotalCostField;
        private Button AddButton;
        private Button SaveButton;
        private Button DeleteButton;
    }
}