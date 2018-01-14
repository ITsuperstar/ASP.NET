using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication1
{
    /// <summary>
    /// QuitSession 的摘要说明
    /// </summary>
    public class QuitSession : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            SessionHelper2.Del("username");
            SessionHelper2.Del("password");
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