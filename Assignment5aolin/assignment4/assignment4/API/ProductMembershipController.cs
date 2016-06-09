using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using assignment4.Models;

using System.Security.Claims;

using System.Web;

namespace assignment4.API
{
    public class ProductMembershipController : ApiController
    {
        //public ProductMembership Get(int id)
        // {
        //     using (Assignment4Context context = new Assignment4Context())
        //     {
        //         Product product = context.Products.Find(id);
        //         return new ProductMembership { UserName = this.User.Identity.Name, ProductId = product.Id };
        //     }
        // }

        public List<Models.ProductMembershipDTO> Get()
        {
            using (Assignment4Context context = new Assignment4Context())
            {
                return context.ProductMembership.Select(s => new ProductMembershipDTO {    ProductId=s.ProductId, UserId = s.UserId}).ToList();
            }
        }
        public ProductMembershipDTO Get(int id)
        {
            using (Assignment4Context context = new Assignment4Context())
            {
                ProductMembership pmember = context.ProductMembership.Find(id);
                return new ProductMembershipDTO { ProductId = pmember.ProductId, UserId = pmember.UserId };
            }

        }
        public HttpResponseMessage Delete(int id)
        {

            using (Assignment4Context context = new Assignment4Context())
            {
                var couldDelete = User.IsInRole("Admin") || User.IsInRole("Leader");
                if (couldDelete)
                {
                    var pmember = context.ProductMembership.Find(id);
                    context.ProductMembership.Remove(pmember);
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Okay");
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Product not added/updated." });
                }
            }
        }


        //    public HttpResponseMessage Post([FromBody]ProductMembership value)
        //    {
        //        try
        //        {
        //            var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;

        //            using (Assignment4Context context = new Assignment4Context())
        //            {
        //                //var CurrentProductID = context.Products.Find(value.ProductId);
        //                if (/*value.ID == 0 &&*/ User.IsInRole("Seeker"))
        //                {
        //                    ProductMembership newMembership = context.ProductMembership.Create();

        //                    newMembership.ID = value.ID;
        //                    newMembership.ProductId = value.ProductId;
        //                    //newMembership.UserName = User.Identity.Name;
        //                    context.ProductMembership.Add(newMembership);
        //                    context.SaveChanges();
        //                    //HttpContext.Current.Cache.Remove("ProductList");
        //                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Added to team." });
        //                }



        //            }
        //            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Product not added/updated." });

        //        }
        //        catch (Exception e)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Error Occurred. Scary Details:" + e.Message });
        //        }


        //}

    }
}

