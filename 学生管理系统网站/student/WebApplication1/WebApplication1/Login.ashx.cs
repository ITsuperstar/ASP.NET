using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication1
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //context.Response.Write("Hello World");
            bool isLogin = !string.IsNullOrEmpty((string)context.Session["username"]) && !string.IsNullOrEmpty((string)context.Session["password"]);
            if (isLogin) 
            { context.Response.Redirect("main.ashx"); }
            else
            {
                string html = CommonHelper.RenderHtml("Login.html","");
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