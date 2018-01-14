using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MetalsWebApplication
{
    /// <summary>
    /// contact 的摘要说明
    /// </summary>
    public class contact : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            DataTable menufirst = new DataTable();
            DataTable proclass = new DataTable();
            try
            {
                menufirst = SqlHelper.ExecuteDataTable("select * from menufirst ");
                proclass = SqlHelper.ExecuteDataTable("select * from proclassinfo ");
            }
            catch (Exception e) { context.Response.Write("访问数据库出错！"); }

            int linknumber = 1;
            DataTable linkclass = SqlHelper.ExecuteDataTable("select * from linkclass ");
            int linkclassCount = (int)SqlHelper.ExecuteScalar("select count(*) from linkclass");
            object[] linkclassData = new object[linkclassCount];
            for (int i = 0; i < linkclassCount; i++)
            {
                int linkclassid = (int)linkclass.Rows[i].ItemArray[0];
                string linkclassname = (string)linkclass.Rows[i].ItemArray[1];
                DataTable links = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by linkclickrate desc) as num from  link where linkcid=@linkcid) s where s.num>@Start and s.num<@End",
                                        new SqlParameter("@linkcid", linkclassid),
                                        new SqlParameter("@Start", (linknumber - 1) * 4),
                                        new SqlParameter("@End", linknumber * 4 + 1));
                linkclassData[i] = new { linkclassname = linkclassname, links = links.Rows };
            }

            var Data = new { menufirsts = menufirst.Rows, proclasses = proclass.Rows, linkclassData = linkclassData };
            string html = CommonHelper.RenderHtml("contact.html", Data);
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