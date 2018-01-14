using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MetalsWebApplication
{
    /// <summary>
    /// shoppingcarAdd 的摘要说明
    /// </summary>
    public class shoppingcarAdd : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int proid = int.Parse(context.Request["proid"]);
            string username = (string)context.Session["username"];         
            int userid = (int)SqlHelper.ExecuteScalar("select userid from userlogin where username=@username",
                               new SqlParameter("@username", username));
            int isshopping = (int)SqlHelper.ExecuteScalar("select count(*) from shoppingcar where userid=@userid and proid=@proid",
                               new SqlParameter("@userid", userid), new SqlParameter("@proid", proid));
            DateTime currentTime = DateTime.Now;
            string shoppingstate = "购物车";
            if (isshopping > 0)
            {
                SqlHelper.ExecuteNonQuery("update shoppingcar set  quantity= quantity+1,time=@currentTime  where userid=@userid and proid=@proid",
                               new SqlParameter("@userid", userid), new SqlParameter("@proid", proid), new SqlParameter("@currentTime", currentTime));
                context.Response.Write("exist");
            }
            else
            {
                SqlHelper.ExecuteNonQuery("insert  into  shoppingcar(userid, proid, quantity, state, time)  values(@userid, @proid, 1, @state, @currentTime)",
                               new SqlParameter("@userid", userid), new SqlParameter("@proid", proid),
                               new SqlParameter("@state", shoppingstate), new SqlParameter("@currentTime", currentTime));
                context.Response.Write("add");
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