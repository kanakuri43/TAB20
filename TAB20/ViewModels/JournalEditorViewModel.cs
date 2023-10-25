using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TAB20.ViewModels
{
    public class JournalEditorViewModel : BindableBase
    {
        private int _id ;
        private DateTime _journalDate;
        private string _description;
        private int _debitAccountId;
        private string _debitAccountName;
        private int _creditAccountId;
        private string _creditAccountName;
        private int _price;
        private int[] _rates = new int[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
        private int _rate;
        private int _balanceAccountCode;

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
        public string DebitAccountName
        {
            get { return _debitAccountName; }
            set { SetProperty(ref _debitAccountName, value); }
        }
        public int CreditAccountId
        {
            get { return _creditAccountId; }
            set { SetProperty(ref _creditAccountId, value); }
        }
        public string CreditAccountName
        {
            get { return _creditAccountName; }
            set { SetProperty(ref _creditAccountName, value); }
        }
        public int Price
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

        public JournalEditorViewModel()
        {
            InitializeScreen();
        }
        
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
    }
}
