using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MetalsWebApplication
{
    /// <summary>
    /// productdetail 的摘要说明
    /// </summary>
    public class productdetail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            int proid = int.Parse(context.Request["proid"].ToString());
            int PageNumber = 1;
            DataTable menufirst = new DataTable();
            DataTable proclass = new DataTable();
            DataTable pros = new DataTable();
            try
            {
                menufirst = SqlHelper.ExecuteDataTable("select * from menufirst ");
                proclass = SqlHelper.ExecuteDataTable("select * from proclassinfo ");
                pros = SqlHelper.ExecuteDataTable("select * from proinfo where proid=@proid",new SqlParameter("@proid",proid));
            }
            catch (Exception e) { context.Response.Write("访问数据库出错！"); }

            int proclassid = (int)SqlHelper.ExecuteScalar("select proclassid from proinfo where proid=@proid", new SqlParameter("@proid", proid));
            DataTable similarPros= SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by proclickrate desc) as num from  proinfo where proclassid=@proclassid and proid!=@proid) s where s.num>@Start and s.num<@End",
                                  new SqlParameter("@proid", proid),
                                  new SqlParameter("@proclassid", proclassid),
                                  new SqlParameter("@Start", (PageNumber - 1) * 3),
                                  new SqlParameter("@End", PageNumber * 3 + 1));

            DataTable linkclass = SqlHelper.ExecuteDataTable("select * from linkclass ");
            int linkclassCount = (int)SqlHelper.ExecuteScalar("select count(*) from linkclass");
            object[] linkclassData = new object[linkclassCount];
            int linknumber = 1;
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

            var Data = new { menufirsts = menufirst.Rows, proclasses = proclass.Rows, pros = pros.Rows, proclassid=proclassid, similarPros=similarPros.Rows,  linkclassData = linkclassData};
            string html = CommonHelper.RenderHtml("productdetail.html", Data);
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