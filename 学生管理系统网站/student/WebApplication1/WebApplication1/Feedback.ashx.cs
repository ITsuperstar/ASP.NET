using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// Feedback 的摘要说明
    /// </summary>
    public class Feedback : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string contact=context.Request["contact"];
            string feedback = context.Request["Feedback"];
            if (string.IsNullOrEmpty(contact) && string.IsNullOrEmpty(feedback))
            {
                var data = new{contact="",message="",text=""};
                string html = CommonHelper.RenderHtml("Feedback.html",data);
                context.Response.Write(html);
            }
            else if (string.IsNullOrEmpty(contact))
            {
                var data = new { contact="",message = "联系方式是必填项",text=feedback };
                string html = CommonHelper.RenderHtml("Feedback.html", data);
                context.Response.Write(html);
            }
            else if (string.IsNullOrEmpty(feedback))
            {
                var data = new {contact=contact, message = "请您填写需要反馈内容", text ="" };
                string html = CommonHelper.RenderHtml("Feedback.html", data);
                context.Response.Write(html);
            }
            else
            {
                try
                {
                    SqlHelper.ExecuteNonQuery("insert into feedback(contact,content) values(@contact,@content)",
                      new SqlParameter("@contact", contact), new SqlParameter("@content",feedback));
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
                //context.Response.Write("<script>alert('ok!')</script>");
                context.Response.Redirect("main.ashx");
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