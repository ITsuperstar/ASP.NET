using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// TeacherAdd 的摘要说明
    /// </summary>
    public class TeacherAdd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string tname = context.Request["tname"];
            string telphone = context.Request["telphone"];
            string email = context.Request["email"];
            string birthday = context.Request["birthday"];
            if (string.IsNullOrEmpty(tname))
            {
                var data = new { tname = "", telphone = " ", email = " ", birthday = " "};
                string html = CommonHelper.RenderHtml("TeacherAdd.html", data);   //注意html文件后缀
                context.Response.Write(html);
            }
            else
            {
                try
                {
                    SqlHelper.ExecuteNonQuery("insert into teacher(tname,telphone,email, birthday) values(@tname,@telphone,@email,@birthday)",
                      new SqlParameter("@tname", tname), new SqlParameter("@telphone", telphone), new SqlParameter("@email", email)
                      , new SqlParameter("@birthday", birthday));
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
                //context.Response.Write("<script>alert('ok!')</script>");
                context.Response.Redirect("TeacherMain.ashx");
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