using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using TAB20.Models;
using TAB20.Views;

namespace TAB20.ViewModels
{
    public class FinancialStatementsViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private int[] _years = new int[7];
        private int _selectedYear;
        private ObservableCollection<BalanceSheet> _balanceSheets;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set { SetProperty(ref _selectedYear, value); }
        }
        public int[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }
        public ObservableCollection<BalanceSheet> BalanceSheets
        {
            get { return _balanceSheets; }
            set { SetProperty(ref _balanceSheets, value); }
        }

        public FinancialStatementsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            BackCommand = new DelegateCommand(BackCommandExecute);
            YearSelectionChanged = new DelegateCommand<object[]>(YearSelectionChangedExecute);

            for (int i = 0; i < 7; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i;
            }

        }

        public DelegateCommand BackCommand { get; }
        public DelegateCommand<object[]> YearSelectionChanged { get; }

        private void BackCommandExecute()
        {
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(JournalEditor), p);

        }

        private void YearSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0];
                var year = selectedItem;
                ShowBalanceSheetTable((int)year);
            }
            catch
            {

            }
        }

        private void ShowBalanceSheetTable(int year)
        {
            using (var context = new AppDbContext())
            {
                var argYear = new SqlParameter("@Year", year);

                var bs = context.Database.SqlQueryRaw<BalanceSheet>("EXEC usp_balance_sheet 2022");


                foreach (var b in bs)
                {
                    Console.WriteLine($"Name: {b.AccountId}");
                }
            }

        }


    }
}
