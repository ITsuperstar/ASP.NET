using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace video
{
    /// <summary>
    /// isLogin 的摘要说明
    /// </summary>
    public class isLogin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = (string)context.Session["username"];
            bool isLogin = !string.IsNullOrEmpty(username);
            if (isLogin)
                context.Response.Write(username);
            else
                context.Response.Write("no");
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