using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace WebApplication1
{
    /// <summary>
    /// TeacherMain 的摘要说明
    /// </summary>
    public class TeacherMain : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string tname = context.Request["Search"];
            //Session验证用户是否存在
            if (SessionHelper2.Get("username") == null && SessionHelper2.Get("password") == null)
            {
                context.Response.Redirect("Login.ashx");
            }
            else
            {
                if (string.IsNullOrEmpty(tname))
                {
                    ShowList(context);
                }
                else
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        dt = SqlHelper.ExecuteDataTable("select * from teacher where tname like @tname", new SqlParameter("@tname", "%" + tname + "%"));
                    }
                    catch (Exception e) { }
                    var Data = new { Persons = dt.Rows };
                    string html = CommonHelper.RenderHtml("TeacherMain.html", Data);
                    context.Response.Write(html);
                }
            }
        }
        private void ShowList(HttpContext context)
        {
            DataTable dt = new DataTable();
            int PageNumber = 1;
            if (context.Request["PageNumber"] != null)
            {
                Regex r = new Regex(@"^\d*$");
                if (r.IsMatch(context.Request["PageNumber"]))
                    PageNumber = int.Parse(context.Request["PageNumber"].ToString());
            }
            try
            {
                dt = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by tid asc) as num from teacher) t  where  t.num>@Start and  t.num<@End",
                new SqlParameter("@Start", (PageNumber - 1) * 3),
                new SqlParameter("@End", PageNumber * 3 + 1));
            }
            catch (Exception e)
            {
                context.Response.Write("访问数据库出错！");
            }
            int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from teacher");
            int pageCount = (int)Math.Ceiling(totalCount / 3.0);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new {ID=i+1, Title = i + 1 };
            }
            var Data = new { Persons = dt.Rows, PageData = pageData, currentPage=1 };
            string html = CommonHelper.RenderHtml("TeacherMain.html", Data);
            context.Response.Write(html);
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