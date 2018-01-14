using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// TeacherEdit 的摘要说明
    /// </summary>
    public class TeacherEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string tid = context.Request["EditID"];
            if (!string.IsNullOrEmpty(tid))
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from teacher where tid=@tid", new SqlParameter("@tid", tid));
                var Data = new { Teacher = dt.Rows[0] };
                string html = CommonHelper.RenderHtml("TeacherEdit.html", Data);
                context.Response.Write(html);
            }
            else if (!string.IsNullOrEmpty(context.Request["UpdateID"]))
            {
                string upid = context.Request["UpdateID"];
                string tname = context.Request["tname"];
                string telphone = context.Request["telphone"];
                string email = context.Request["email"];
                string birthday = context.Request["birthday"];
                try
                {
                    SqlHelper.ExecuteNonQuery("update  teacher set tname=@tname,telphone=@telphone,email=@email,birthday=@birthday where tid=@tid",
                                         new SqlParameter("@tname", tname), new SqlParameter("@telphone", telphone), new SqlParameter("@email", email)
                                        , new SqlParameter("@birthday", birthday), new SqlParameter("@tid", upid));
                }
                catch (Exception e)
                {
                    context.Response.Write("访问数据库错误！");
                }
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