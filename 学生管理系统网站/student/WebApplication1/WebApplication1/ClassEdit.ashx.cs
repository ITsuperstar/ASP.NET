using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// ClassEdit 的摘要说明
    /// </summary>
    public class ClassEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string cid = context.Request["EditID"];
            if (!string.IsNullOrEmpty(cid))
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select class.*,tname   from class,teacher where cid=@cid and class.tid=teacher.tid ", new SqlParameter("@cid", cid));
                var Data = new { Class1 = dt.Rows[0] };
                string html = CommonHelper.RenderHtml("ClassEdit.html", Data);
                context.Response.Write(html);
            }
            else if (!string.IsNullOrEmpty(context.Request["UpdateID"]))
            {
                string upid = context.Request["UpdateID"];
                string classname = context.Request["classname"];
                string classnumber = context.Request["classnumber"];
                int tid =int.Parse(context.Request["tname"]);
                try
                {
                    SqlHelper.ExecuteNonQuery("update  class set classname=@classname,classnumber=@classnumber,tid=@tid where cid=@cid"
                                            , new SqlParameter("@classname", classname), new SqlParameter("@classnumber", classnumber)
                                            , new SqlParameter("@tid", tid), new SqlParameter("@cid", upid));
                }
                catch (Exception e)
                {
                    context.Response.Write("访问数据库错误！");
                }
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