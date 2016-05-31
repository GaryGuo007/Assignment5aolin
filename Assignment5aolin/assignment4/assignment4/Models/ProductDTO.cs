using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace assignment4.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String ApplicationUserId { get; set; }
        public String Payable { get; set; }
        public DateTime AddedDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}