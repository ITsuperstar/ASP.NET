using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MetalsWebApplication
{
    /// <summary>
    /// Logout 的摘要说明
    /// </summary>
    public class Logout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            SessionHelper.Del("username");
            context.Response.Redirect("Login.ashx");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}