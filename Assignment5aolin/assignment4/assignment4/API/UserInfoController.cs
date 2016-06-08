using assignment4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace assignment4.API
{
    public class UserInfoController : ApiController
    {
        public UserRightsDTO Get()
        {
            var rights = new UserRightsDTO();
            //Check rights
            
            using (Assignment4Context context = new Assignment4Context())
            {
                if (this.User.IsInRole("Seeker"))
                {
                    rights.IsSeekerRight = true;
                    
    }
                else
                {
                    rights.IsSeekerRight = false;
                }
            }
                return rights;
        }
    }
}
