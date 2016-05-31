using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace assignment4.Helpers
{
    public static class MemberLevelList
    {
        //Based on http://stackoverflow.com/questions/5052752/adding-your-own-htmlhelper-in-asp-net-mvc-3
        //Simplified version
        public static MvcHtmlString MemberLevelDropdownList(this HtmlHelper html)
        {
            List<SelectListItem> memberlevellist = new List<SelectListItem>
                    {
                        new SelectListItem() { Value= "Leader",Text= "Leader"},
                         new SelectListItem() {Value="Member",Text="Seeker"},
                    };

            return html.DropDownList("Register as", memberlevellist);
        }

        public static MvcHtmlString MemberLevelDropdownList<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            Dictionary<string, string> memberlevellist = new Dictionary<string, string>()
                          {

                             {"Leader", " Leader"},
                             {"Member", " Seeker"},

                          };

            return html.DropDownListFor(expression, new SelectList(memberlevellist, "key", "value"), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
    }
}