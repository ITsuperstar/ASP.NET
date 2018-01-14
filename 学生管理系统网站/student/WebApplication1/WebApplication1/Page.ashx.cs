using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// page 的摘要说明
    /// </summary>
    public class page : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string act=context.Request["action"];
            if (act == "student") 
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
                    dt = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by sid asc) as num from student) s,class where s.cid=class.cid  and s.num>@Start and s.num<@End",
                    new SqlParameter("@Start", (PageNumber - 1) * 3),
                    new SqlParameter("@End", PageNumber * 3 + 1));
                }
                catch (Exception e)
                {
                    context.Response.Write("访问数据库出错！");
                }
                string json = DataTableToJson.DataTableToJsonWithJavaScriptSerializer(dt);
                context.Response.Write(json);
            }
            else if (act == "class")
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
                    dt = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by cid asc) as num from class) c,teacher where c.tid=teacher.tid  and c.num>@Start and c.num<@End",
                    new SqlParameter("@Start", (PageNumber - 1) * 3),
                    new SqlParameter("@End", PageNumber * 3 + 1));
                }
                catch (Exception e)
                {
                    context.Response.Write("访问数据库出错！");
                }
                string json = DataTableToJson.DataTableToJsonWithJavaScriptSerializer(dt);
                context.Response.Write(json);
            }
            else if (act == "teacher")
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
                    dt = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by tid asc) as num from  teacher) t where t.num>@Start and t.num<@End",
                    new SqlParameter("@Start", (PageNumber - 1) * 3),
                    new SqlParameter("@End", PageNumber * 3 + 1));
                }
                catch (Exception e)
                {
                    context.Response.Write("访问数据库出错！");
                }
                string json = DataTableToJson.DataTableToJsonWithJavaScriptSerializer(dt);
                context.Response.Write(json);
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