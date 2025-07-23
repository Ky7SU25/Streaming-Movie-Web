using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMovie.Domain.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentCreateDate { get; set; } = DateTime.Now;
        public DateTime PaymentExpDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        [Column("Status")]
        public string Status { get; set; }
        public virtual User User { get; set; }

    }
}
