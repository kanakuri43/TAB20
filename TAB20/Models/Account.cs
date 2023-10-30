using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAB20.Models
{
    [Table("accounts")]
    public class Account
    {
        public int Id {get; set; }
        [Column("account_name")]
        public string AccountName { get; set; }
        [Column("account_category_id")]
        public int AccountCategoryId { get; set; }
        [Column("is_carry_forward")]
        public int IsCarryForward { get; set; }
    }
}
