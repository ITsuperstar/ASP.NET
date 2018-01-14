using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// Query 的摘要说明
    /// </summary>
    public class Query : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string act=context.Request["action"];
            if (act == "studentToClassname")
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select cid,classname  from class");
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
                string json = DataTableToJson.DataTableToJsonWithJavaScriptSerializer(dt);
                context.Response.Write(json);
            }
            else if (act == "classToTname")
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select tid,tname  from teacher");
                }
                catch (Exception e) { context.Response.Write("访问数据库错误！"); }
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