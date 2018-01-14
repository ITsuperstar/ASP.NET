using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// StudentAdd 的摘要说明
    /// </summary>
    public class StudentAdd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string name = context.Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select cid,classname from class");
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
                var data = new {  classnames =dt.Rows };
                string html = CommonHelper.RenderHtml("StudentAdd.html", data);   //注意html文件后缀
                context.Response.Write(html);
            }
            else
            {
                string sex = context.Request["sex"];
                string birthday = context.Request["birthday"];
                string height = context.Request["height"];
                int cid = int.Parse(context.Request["classname"]);
                string specialties = context.Request["specialties"];
                try
                {
                    SqlHelper.ExecuteNonQuery("insert into student(name,sex,birthday, height, cid, specialties) values(@name,@sex,@birthday,@height,@cid,@specialties)",
                      new SqlParameter("@name", name), new SqlParameter("@sex", sex), new SqlParameter("@birthday", birthday)
                      , new SqlParameter("@height", height), new SqlParameter("@cid", cid), new SqlParameter("@specialties", specialties));
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
                //context.Response.Write("<script>alert('ok!')</script>");
                context.Response.Redirect("StudentMain.ashx");
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