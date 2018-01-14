using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// ClassAdd 的摘要说明
    /// </summary>
    public class ClassAdd : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string classname = context.Request["classname"];
            if (string.IsNullOrEmpty(classname))
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select tid,tname from teacher");
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
                var data = new { classname = "", classnumber = " ", tnames = dt.Rows};
                string html = CommonHelper.RenderHtml("ClassAdd.html", data);   //注意html文件后缀
                context.Response.Write(html);
            }
            else
            {
                string classnumber = context.Request["classnumber"];
                int tid =int.Parse(context.Request["tname"]);
                try
                {
                    SqlHelper.ExecuteNonQuery("insert into class(classname,classnumber,tid) values(@classname,@classnumber,@tid)",
                      new SqlParameter("@classname", classname), new SqlParameter("@classnumber", classnumber), new SqlParameter("@tid", tid));
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
                //context.Response.Write("<script>alert('ok!')</script>");
                context.Response.Redirect("ClassMain.ashx");
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