using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace video
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
            context.Response.Redirect("/index.ashx");   //退出后，重定向  首页
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