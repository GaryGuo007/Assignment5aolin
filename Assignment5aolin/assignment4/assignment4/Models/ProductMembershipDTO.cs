using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace assignment4.Models
{
    public class ProductMembershipDTO
    {

        public int ID { get; set; }

        
        public int ProductId { get; set; }

        public string UserId { get; set; }

        //public String LeaderId { get; set; }
    }
}