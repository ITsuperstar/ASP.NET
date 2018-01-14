using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace video
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
            { context.Response.Redirect("/user.ashx"); }
            else
            {
                string html = CommonHelper.RenderHtml("login.html", "");
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