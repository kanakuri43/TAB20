using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAB20.Models
{
    public class BalanceSheet
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal DebitPrice { get; set; }
        public decimal CreditPrice { get; set; }
    }
}
