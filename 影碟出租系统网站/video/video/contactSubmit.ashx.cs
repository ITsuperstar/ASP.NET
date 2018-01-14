using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace video
{
    /// <summary>
    /// contactSubmit 的摘要说明
    /// </summary>
    public class contactSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string name = context.Request["name"];
            string email = context.Request["email"];
            string telphone = context.Request["telphone"];
            string message = context.Request["message"];
            try
            {
                SqlHelper.ExecuteNonQuery("insert into contact(name,email,telphone,message) values(@name,@email,@telphone,@message)",
                       new SqlParameter("@name", name), new SqlParameter("@email", email),
                       new SqlParameter("@telphone", telphone), new SqlParameter("@message", message));

            }
            catch (Exception e) { context.Response.Write("访问数据库错误！"); }
            context.Response.Redirect("/contact.ashx");     
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