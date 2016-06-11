using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace assignment4.Models
{
    public class ProductMembership
    {
        public int ID { get; set; }
       
        //public String UserName { get; set; }
        public int ProductId { get; set; }

        
        public String Name { get; set; }

        public String UserId { get; set; }

        //public String LeaderId { get; set; }
        //  public virtual /*Editable*/Product product{ get; set; }
        /// <summary>
        /// /////////
        /// </summary>
        // public virtual EditableProduct EditableProduct { get; set; }
        //public virtual Student Student { get; set; }
    }
}