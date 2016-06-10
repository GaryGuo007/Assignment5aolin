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

            List<EditableProduct> products = null;
            //var userId= User.Identity.Name;
            if (HttpContext.Current.Cache["ProductList"] != null)
                products = (List<EditableProduct>)HttpContext.Current.Cache["ProductList"];

            using (Assignment4Context context = new Assignment4Context())
            {
                var isAdmin = this.User.IsInRole("Admin");
                var isLeader = this.User.IsInRole("Leader");
                var isSeeker = this.User.IsInRole("Seeker");

                var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
                if (products == null)
                {
                    products = context.Products.Select(t => new EditableProduct { IsJoinable = isSeeker, IsEditable = isAdmin, Id = t.Id, AddedDate = t.AddedDate, ApplicationUserId = t.ApplicationUserId, Payable = t.Payable, Description = t.Description, Name = t.Name }).ToList();
                    HttpContext.Current.Cache["ProductList"] = products;
                }

                foreach (EditableProduct product in products)
                {
                    product.IsJoinable = isSeeker;
                    ///  if (product.ApplicationUserId == userId)
                    //  {
                    //    product.IsEditable = true;
                    if (User.IsInRole("Admin") || (product.ApplicationUserId == userId))
                    {
                        product.IsEditable = true;
                        product.IsJoinable = false;
                    }
                    if (User.IsInRole("Seeker") || (product.ApplicationUserId == userId))
                    {
                        product.IsEditable = false;
                        product.IsJoinable = true;
                    }
                    if (User.IsInRole("Leader") && (product.ApplicationUserId == userId))
                    {
                        product.IsEditable = true;
                        product.IsJoinable = false;
                    }
                }
                // }

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
                var eProduct = new EditableProduct {  IsJoinable = false, IsEditable = false, Version = product.Timestamp, Id = product.Id, AddedDate = product.AddedDate, ApplicationUserId = product.ApplicationUserId, Payable = product.Payable, Description = product.Description, Name = product.Name };
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
                if (User.IsInRole("Leader") || (product.ApplicationUserId == userId))
                {
                    eProduct.IsEditable = true;
                    eProduct.IsJoinable = false;
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
                    if (value.Id == 0 && !User.IsInRole("Seeker"))
                    {
                        Product newProduct = context.Products.Create();
                        newProduct.Name = value.Name;
                        newProduct.AddedDate = DateTime.Now;
                        newProduct.ApplicationUserId = userId;
                        newProduct.Description = value.Description;
                        newProduct.Id = value.Id;
                        newProduct.Payable = value.Payable;
                        //newProduct.JoinedMemberList = value.JoinedMemberList;
                        context.Products.Add(newProduct);
                        context.SaveChanges();
                        HttpContext.Current.Cache.Remove("ProductList");
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Product Added." });
                    }
                    else if (User.IsInRole("Seeker"))
                    {
                        var productMemberShip = context.ProductMembership.Create();
                        var product = context.Products.Find(value.Id);
                        //product.JoinedMemberList = product.JoinedMemberList.Union
                        //    (User.Identity.Name.ToList());
                        //product.JoinedMemberList += userId;
                        productMemberShip.ProductId = value.Id;
                        //productMemberShip.Name = value.Name;  // project title
                        productMemberShip.UserId = User.Identity.Name; // user email
                        context.ProductMembership.Add(productMemberShip);
                        context.SaveChanges();
                        HttpContext.Current.Cache.Remove("ProductList");

                        //return Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Seeker Can't add project" });
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "You have joined this project." });

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
                           // product.JoinedMemberList = value.JoinedMemberList;
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

        //public HttpResponseMessage Post([FromBody]ProductMembership value)
        //{
        //    try
        //    {
        //        var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
        //        using (Assignment4Context context = new Assignment4Context())
        //        {
        //            if (User.IsInRole("Seeker"))
        //            {

        //                var productMemberShip = context.ProductMembership.Create();
        //                productMemberShip.ProductId = value.ProductId;
        //                productMemberShip.UserId = userId;
        //                context.ProductMembership.Add(productMemberShip);
        //                context.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Joined." });
        //            }
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Seeker not joined" });

        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Error Occurred. Scary Details:" + e.Message });
        //    }

        //}

        public HttpResponseMessage Delete(int id)
        {
            using (Assignment4Context context = new Assignment4Context())
            {
                //  if (!User.IsInRole("Seeker"))
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
}
