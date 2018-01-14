using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication1
{
    /// <summary>
    /// main 的摘要说明
    /// </summary>
    public class main : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (SessionHelper2.Get("username") == null && SessionHelper2.Get("password") == null)
            {
                context.Response.Redirect("Login.ashx");
            }
            else{
            string html = CommonHelper.RenderHtml("main.html"," ");
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