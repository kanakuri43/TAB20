using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAB20.Models
{
    [Table("account_journals")]
    public class AccountJournal
    {
        public int Id { get; set; }

        [Column("journal_date")]
        public DateTime JournalDate { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("debit_account_id")]
        public int DebitAccountId { get; set; }

        [Column("debit_account_name")]
        public string DebitAccountName { get; set; }

        [Column("credit_account_id")]
        public int CreditAccountId { get; set; }

        [Column("credit_account_name")]
        public string CreditAccountName { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
    }
}
