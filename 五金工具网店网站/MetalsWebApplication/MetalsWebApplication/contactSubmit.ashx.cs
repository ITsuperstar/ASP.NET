using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MetalsWebApplication
{
    /// <summary>
    /// contactSubmit 的摘要说明
    /// </summary>
    public class contactSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string name=context.Request["author"];
            string email=context.Request["email"];
            string subject=context.Request["subject"];
            string text = context.Request["text"];
            try
            {
                SqlHelper.ExecuteNonQuery("insert into contact(name,email,subject,message) values(@name,@email,@subject,@message)",
                       new SqlParameter("@name", name),new SqlParameter("@email", email),
                       new SqlParameter("@subject", subject),new SqlParameter("@message", text));
 
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