using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MetalsWebApplication
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            bool isLogin = !string.IsNullOrEmpty((string)context.Session["username"]);
            if (isLogin)
            { context.Response.Redirect("/index.ashx"); }
            else
            {
                string html = CommonHelper.RenderHtml("Login.html", "");
                context.Response.Write(html);
            }
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