using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace video
{
    /// <summary>
    /// reviewSubmit 的摘要说明
    /// </summary>
    public class reviewSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string videoid = context.Request["videoid"];
            string name = context.Request["name"];
            string email = context.Request["email"];
            string message = context.Request["message"];
            DateTime reviewtime = DateTime.Now;
            try
            {
                SqlHelper.ExecuteNonQuery("insert into review(videoid,name,email,message,reviewtime) values(@videoid,@name,@email,@message,@reviewtime)",
                       new SqlParameter("@videoid", videoid), new SqlParameter("@name", name), 
                       new SqlParameter("@email", email),new SqlParameter("@message", message),
                       new SqlParameter("@reviewtime", reviewtime));
                      
            }
            catch (Exception e) { context.Response.Write("访问数据库错误！"); }
            context.Response.Redirect("/detail.ashx?videoid=" + videoid);     
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