using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MetalsWebApplication
{
    /// <summary>
    /// shoppingcarCount 的摘要说明
    /// </summary>
    public class shoppingcarCount : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = (string)context.Session["username"];
            int count=0;
            try
            {
                int userid = (int)SqlHelper.ExecuteScalar("select userid from userlogin where username=@username",
                                   new SqlParameter("@username", username));
                count = (int)SqlHelper.ExecuteScalar("select count(*) from shoppingcar where userid=@userid and quantity>0 and state='购物车'",
                                   new SqlParameter("@userid", userid));
            }
            catch (Exception e) { 
                context.Response.Write("error");
                return;
            }
            context.Response.Write(count);
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