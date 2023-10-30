﻿using Microsoft.Data.SqlClient;
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
        private string _debitAccountName;
        private int _creditAccountId;
        private string _creditAccountName;
        private decimal _price;
        private int[] _rates = new int[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
        private int _rate;
        private int _balanceAccountCode;
        private ObservableCollection<Account> _accounts;

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

        public JournalEditorViewModel()
        {
            InitializeScreen();

            RegisterCommand = new DelegateCommand(RegisterCommandExecute);
            JournalSearchCommand = new DelegateCommand<TextBox>(JournalSearchCommandExecute);

            using (var context = new AppDbContext())
            {
                Accounts = new ObservableCollection<Account>(context.Accounts.ToList());

            }
        }

        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand<TextBox> JournalSearchCommand { get; }

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
                if(this.Id == 0)
                {
                    var accountJournal = new AccountJournal
                    {
                        JournalDate = this.JournalDate,
                        Description = this.Description,
                        DebitAccountId = this.DebitAccountId,
                        DebitAccountName = this.DebitAccountName,
                        CreditAccountId = this.CreditAccountId,
                        CreditAccountName = this.CreditAccountName,
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
                        accountJournal.DebitAccountName = this.DebitAccountName;
                        accountJournal.CreditAccountId = this.CreditAccountId;
                        accountJournal.CreditAccountName = this.CreditAccountName;
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
                    this.DebitAccountName = j.DebitAccountName;
                    this.CreditAccountId = j.CreditAccountId;
                    this.CreditAccountName = j.CreditAccountName;
                    this.Price = j.Price;

                }
            }
        }
    }
}
