using Microsoft.Data.SqlClient;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using TAB20.Models;

namespace TAB20.ViewModels
{
    public class JournalEditorViewModel : BindableBase
    {
        private int _id ;
        private DateTime _journalDate;
        private string _description;
        private int _debitAccountId;
        //private string _debitAccountName;
        private int _creditAccountId;
        //private string _creditAccountName;
        private decimal _price;
        private int[] _rates = new int[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
        private int _rate;
        private int _balanceAccountCode;
        private ObservableCollection<Account> _accounts;
        private int[] _years = new int[10];
        private int _selectedYear;
        private ObservableCollection<AccountJournal> _accountJournals;

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
        //public string DebitAccountName
        //{
        //    get { return _debitAccountName; }
        //    set { SetProperty(ref _debitAccountName, value); }
        //}
        public int CreditAccountId
        {
            get { return _creditAccountId; }
            set { SetProperty(ref _creditAccountId, value); }
        }
        //public string CreditAccountName
        //{
        //    get { return _creditAccountName; }
        //    set { SetProperty(ref _creditAccountName, value); }
        //}
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

        public JournalEditorViewModel()
        {
            InitializeScreen();

            RegisterCommand = new DelegateCommand(RegisterCommandExecute);
            JournalSearchCommand = new DelegateCommand<TextBox>(JournalSearchCommandExecute);
            YearSelectionChanged = new DelegateCommand<object[]>(YearSelectionChangedExecute);

            using (var context = new AppDbContext())
            {
                Accounts = new ObservableCollection<Account>(context.Accounts.ToList());
            }
            for (int i = 0; i < 10; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i;
            }
        }

        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand<TextBox> JournalSearchCommand { get; }
        public DelegateCommand<object[]> YearSelectionChanged { get; }

        private void InitializeScreen()
        {
            Id = 0;
            JournalDate = DateTime.Today;
            Description = "";
            DebitAccountId = 0;
            CreditAccountId = 0;
            Price = 0;
            Rate = 0;

        }

        private void JournalSearchCommandExecute(TextBox slipNoTextBox)
        {
            if (slipNoTextBox.Text == "")
            {

            }
            else
            {
                ReadJournal(int.Parse(slipNoTextBox.Text));
            }
        }

        private void RegisterCommandExecute()
        {
            using (var context = new AppDbContext())
            {
                string debitAccountName = "";
                string creditAccountName = "";
                var dn = context.Accounts.Find(this.DebitAccountId);
                if (dn != null)
                {
                    debitAccountName = dn.AccountName;
                }
                var cn = context.Accounts.Find(this.CreditAccountId);
                if (cn != null)
                {
                    creditAccountName = cn.AccountName;
                }

                if (this.Id == 0)
                {
                    var accountJournal = new AccountJournal
                    {
                        JournalDate = this.JournalDate,
                        Description = this.Description,
                        DebitAccountId = this.DebitAccountId,
                        DebitAccountName = debitAccountName,
                        CreditAccountId = this.CreditAccountId,
                        CreditAccountName = creditAccountName,
                        Price = this.Price
                    };

                    context.AccountJournals.Add(accountJournal);
                    context.SaveChanges();
                }
                else
                {
                    var accountJournal = context.AccountJournals.FirstOrDefault(p => p.Id == this.Id);
                    if (accountJournal != null)
                    {

                        accountJournal.JournalDate = this.JournalDate;
                        accountJournal.Description = this.Description;
                        accountJournal.DebitAccountId = this.DebitAccountId;
                        accountJournal.DebitAccountName = debitAccountName;
                        accountJournal.CreditAccountId = this.CreditAccountId;
                        accountJournal.CreditAccountName = creditAccountName;
                        accountJournal.Price = this.Price;

                        context.SaveChanges();
                    }
                }
            }

        }

        private void ReadJournal(int AccountJournalId)
        {

            using (var context = new AppDbContext())
            {
                var j = context.AccountJournals.Find(AccountJournalId); 
                if (j != null)
                {
                    this.Id = j.Id;
                    this.JournalDate = j.JournalDate;
                    this.Description = j.Description;
                    this.DebitAccountId = j.DebitAccountId;
                    //this.DebitAccountName = j.DebitAccountName;
                    this.CreditAccountId = j.CreditAccountId;
                    //this.CreditAccountName = j.CreditAccountName;
                    this.Price = j.Price;

                }
            }
        }

        private void YearSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0] ;
                var id = selectedItem;
                ShowAccountJournalsTable((int)id);
            }
            catch
            {

            }
        }
        private void ShowAccountJournalsTable(int id)
        {
            using (var context = new AppDbContext())
            {
                this.AccountJournals = new ObservableCollection<AccountJournal>(context.AccountJournals
                                                                                       .Where(p => p.Id == 123)
                                                                                       .ToList());
            }

        }
    }
}
