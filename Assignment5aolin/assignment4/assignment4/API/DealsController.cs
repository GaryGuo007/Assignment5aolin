using assignment4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web;


namespace assignment4.API
{
    //[Authorize]
    public class DealsController : ApiController
    {
        // GET api/<controller>
        public List<EditableProduct> Get()
        {
            if (HttpContext.Current.Cache["ProductList"] != null)
                return (List<EditableProduct>)HttpContext.Current.Cache["ProductList"];

            using (Assignment4Context context = new Assignment4Context())
            {
                var isAdmin = this.User.IsInRole("Admin");
                 var isLeader = this.User.IsInRole("Leader");
                var isSeeker = this.User.IsInRole("Seeker");
                var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
                List<EditableProduct> products = context.Products.Select(t => new EditableProduct {IsJoinable=isSeeker, IsEditable = isAdmin, Id = t.Id, AddedDate = t.AddedDate, ApplicationUserId = t.ApplicationUserId, Payable = t.Payable, Description = t.Description, Name = t.Name }).ToList();
                HttpContext.Current.Cache["ProductList"] = products;
               if (!isAdmin)
                {
                    foreach (EditableProduct product in products)
                    {
                        product.IsJoinable = isSeeker;
                        if (product.ApplicationUserId == userId)
                            product.IsEditable = true;
                    }
                }
                return products;
            }
        }

        // GET api/<controller>/5
        public EditableProduct Get(int id)
        {
            var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
            if (HttpContext.Current.Cache["Product" + id] != null)
                return (EditableProduct)HttpContext.Current.Cache["Product" + id];
            using (Assignment4Context context = new Assignment4Context())
            {
                Product product = context.Products.Find(id);
                var eProduct = new EditableProduct { IsJoinable=false,IsEditable = false, Version = product.Timestamp, Id = product.Id, AddedDate = product.AddedDate, ApplicationUserId = product.ApplicationUserId, Payable = product.Payable, Description = product.Description, Name = product.Name };
                HttpContext.Current.Cache["Product" + id] = eProduct;
                if (User.IsInRole("Admin") || (product.ApplicationUserId == userId))
                {
                    eProduct.IsEditable = true;
                    eProduct.IsJoinable = true;
                }
                if (User.IsInRole("Seeker") || (product.ApplicationUserId == userId))
                {
                    eProduct.IsEditable = false;
                    eProduct.IsJoinable = true;

                }
                return eProduct;
            }

        }

        public HttpResponseMessage Post([FromBody]EditableProduct value)
        {
            try
            {

                var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
                using (Assignment4Context context = new Assignment4Context())
                {
                    if (value.Id == 0)
                    {
                        Product newProduct = context.Products.Create();
                        newProduct.Name = value.Name;
                        newProduct.AddedDate = DateTime.Now;
                        newProduct.ApplicationUserId = userId;
                        newProduct.Description = value.Description;
                        newProduct.Id = value.Id;
                        newProduct.Payable = value.Payable;
                        context.Products.Add(newProduct);
                        context.SaveChanges();
                        HttpContext.Current.Cache.Remove("ProductList");
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Product Added." });
                    }
                    else
                    {
                        var product = context.Products.Find(value.Id);
                        if (User.IsInRole("Admin") || (product.ApplicationUserId == userId))
                        {
                            product.Name = value.Name;
                            product.AddedDate = DateTime.Now;
                            product.ApplicationUserId = userId;
                            product.Description = value.Description;
                            product.Id = value.Id;
                            product.Payable = value.Payable;
                            context.SaveChanges();
                            HttpContext.Current.Cache.Remove("ProductList");
                            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Product Updated." });

                        }

                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Product not added/updated." });

            }
            catch (Exception e)
            {
              return Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Error Occurred. Scary Details:" + e.Message });
           }
           
        }

        public HttpResponseMessage Delete(int id)
        {
            using (Assignment4Context context = new Assignment4Context())
            {
                var product = context.Products.Find(id);
                context.Products.Remove(product);
                context.SaveChanges();
                HttpContext.Current.Cache.Remove("ProductList");
                HttpContext.Current.Cache.Remove("Product" + id);
                return Request.CreateResponse(HttpStatusCode.OK, "Okay");
            }
        }
    }
}
