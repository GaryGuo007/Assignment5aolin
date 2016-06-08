using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace assignment4.Models
{
    public class EditableProduct
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }
        public String ApplicationUserId { get; set; }
        public String Payable { get; set; }
        public DateTime AddedDate { get; set; }
        public Boolean IsEditable { get; set; }
        public Boolean IsJoinable { get; set; }

        public String[] JoinedMemberList { get; set; }


        public byte[] Version { get; set; }

        /// <summary>
        /// ///////////+++
        /// </summary>
        public EditableProduct()
        {
            this.ProductMembership = new HashSet<ProductMembership>();
        }
        public virtual ICollection<ProductMembership> ProductMembership { get; set; }



    }
}