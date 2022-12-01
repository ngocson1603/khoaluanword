using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Khoaluan.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime When { get; set; }

        public int UserID { get; set; }
        public virtual Users Sender { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
