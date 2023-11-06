using Microsoft.Data.SqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using TAB20.Models;
using TAB20.Views;

namespace TAB20.ViewModels
{
    public class JournalEditorViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private int _id ;
        private DateTime _journalDate;
        private string _description;
        private int _debitAccountId;
        private int _creditAccountId;
        private decimal _price;
        private int[] _rates = new int[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
        private int _rate;
        private int _balanceAccountCode;
        private ObservableCollection<Account> _accounts;
        private int[] _years = new int[7];
        private int _selectedYear;
        private ObservableCollection<AccountJournal> _accountJournals;
        private int _selectedId;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        public DateTime JournalDate
        {
            get { return _journalDate; }
            set { SetProperty(ref _journalDate, value); }
        }
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        public int DebitAccountId
        {
            get { return _debitAccountId; }
            set { SetProperty(ref _debitAccountId, value); }
        }
        public int CreditAccountId
        {
            get { return _creditAccountId; }
            set { SetProperty(ref _creditAccountId, value); }
        }
        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
        public int[] Rates
        {
            get { return _rates; }
            set { SetProperty(ref _rates, value); }
        }
        public int Rate
        {
            get { return _rate; }
            set { SetProperty(ref _rate, value); }
        }
        public int BalanceAccountCode
        {
            get { return _balanceAccountCode; }
            set { SetProperty(ref _balanceAccountCode, value); }
        }
        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }
        public int[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }
        public int SelectedYear
        {
            get { return _selectedYear; }
            set { SetProperty(ref _selectedYear, value); }
        }
        public ObservableCollection<AccountJournal> AccountJournals
        {
            get { return _accountJournals; }
            set { SetProperty(ref _accountJournals, value); }
        }
        public int SelectedId
        {
            get { return _selectedId; }
            set { SetProperty(ref _selectedId, value); }
        }

        public JournalEditorViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            InitializeScreen();

            RegisterCommand = new DelegateCommand(RegisterCommandExecute);
            DeleteCommand = new DelegateCommand(DeleteCommandExecute);
            JournalSearchCommand = new DelegateCommand<TextBox>(JournalSearchCommandExecute);
            YearSelectionChanged = new DelegateCommand<object[]>(YearSelectionChangedExecute);
            AccountJournalsTableDoubleClick = new DelegateCommand(AccountJournalsTableDoubleClickExecute);
            FinancialStatementsCommand = new DelegateCommand(FinancialStatementsCommandExecute);

            using (var context = new AppDbContext())
            {
                Accounts = new ObservableCollection<Account>(context.Accounts.ToList());
            }
            for (int i = 0; i < 7; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i;
            }

            this.SelectedYear = DateTime.Now.Year;
            ShowAccountJournalsTable(this.SelectedYear);
        }

        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand<TextBox> JournalSearchCommand { get; }
        public DelegateCommand<object[]> YearSelectionChanged { get; }
        public DelegateCommand AccountJournalsTableDoubleClick { get; }
        public DelegateCommand FinancialStatementsCommand { get; }

        private void InitializeScreen()
        {
            this.Id = 0;
            this.JournalDate = DateTime.Today;
            this.Description = "";
            this.DebitAccountId = 0;
            this.CreditAccountId = 0;
            this.Price = 0;
            this.Rate = 0;

        }

        private void JournalSearchCommandExecute(TextBox IdTextBox)
        {
            if (IdTextBox.Text == "")
            {

            }
            else
            {
                ReadJournal(int.Parse(IdTextBox.Text));
            }
        }

        private void AccountJournalsTableDoubleClickExecute()
        {
            ReadJournal(this.SelectedId);

        }

        private void FinancialStatementsCommandExecute()
        {
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(FinancialStatements), p);

        }

        private void DeleteCommandExecute()
        {
            using (var context = new AppDbContext())
            {
                var accountJournal = context.AccountJournals.FirstOrDefault(p => p.Id == this.Id);
                if (accountJournal != null)
                {
                    context.AccountJournals.Remove(accountJournal);
                    context.SaveChanges();
                }
            }
            InitializeScreen();
            ShowAccountJournalsTable(this.SelectedYear);

        }

        private void RegisterCommandExecute()
        {
            if(this.DebitAccountId == 0 || this.CreditAccountId == 0 || this.Price == 0)
            {
                return;
            }

            using (var context = new AppDbContext())
            {
                var debitAccount = context.Accounts.Find(this.DebitAccountId);
                var creditAccount = context.Accounts.Find(this.CreditAccountId);

                if (this.Id == 0)
                {
                    // Create
                    var accountJournal = new AccountJournal
                    {
                        JournalDate = this.JournalDate,
                        Description = this.Description,
                        DebitAccountId = this.DebitAccountId,
                        DebitAccountName = debitAccount.AccountName,
                        CreditAccountId = this.CreditAccountId,
                        CreditAccountName = creditAccount.AccountName,
                        Price = this.Price
                    };
                    context.AccountJournals.Add(accountJournal);
                    context.SaveChanges();

                    // 案分するとき
                    if (Rate != 100)
                    {
                        var divideBalanceAccount = context.Accounts.Find(this.BalanceAccountCode);

                        var accountJournalDivide = new AccountJournal
                        {
                            JournalDate = this.JournalDate,
                            Description = this.Description + "（案分）",
                            DebitAccountId = this.DebitAccountId,
                            DebitAccountName = divideBalanceAccount.AccountName,
                            CreditAccountId = this.CreditAccountId,
                            CreditAccountName = debitAccount.AccountName,
                            Price = this.Price - (this.Price * (decimal)(Rate / 100.0))
                        };
                        context.AccountJournals.Add(accountJournalDivide);
                        context.SaveChanges();

                    }

                }
                else
                {
                    // Update
                    var accountJournal = context.AccountJournals.FirstOrDefault(p => p.Id == this.Id);
                    if (accountJournal != null)
                    {

                        accountJournal.JournalDate = this.JournalDate;
                        accountJournal.Description = this.Description;
                        accountJournal.DebitAccountId = this.DebitAccountId;
                        accountJournal.DebitAccountName = debitAccount.AccountName;
                        accountJournal.CreditAccountId = this.CreditAccountId;
                        accountJournal.CreditAccountName = creditAccount.AccountName;
                        accountJournal.Price = this.Price;

                        context.SaveChanges();
                    }
                }
            }
            InitializeScreen();
            ShowAccountJournalsTable(this.SelectedYear);
        }

        private void ReadJournal(int AccountJournalId)
        {

            using var context = new AppDbContext();
            var j = context.AccountJournals.Find(AccountJournalId);
            if (j != null)
            {
                this.Id = j.Id;
                this.JournalDate = j.JournalDate;
                this.Description = j.Description;
                this.DebitAccountId = j.DebitAccountId;
                this.CreditAccountId = j.CreditAccountId;
                this.Price = j.Price;

            }
        }

        private void YearSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0] ;
                var year = selectedItem;
                ShowAccountJournalsTable((int)year);
            }
            catch
            {

            }
        }
        private void ShowAccountJournalsTable(int year)
        {
            using var context = new AppDbContext();
            this.AccountJournals = new ObservableCollection<AccountJournal>(context.AccountJournals
                                                                                   .Where(j => j.JournalDate.Year == year)
                                                                                   .ToList());

        }
    }
}
