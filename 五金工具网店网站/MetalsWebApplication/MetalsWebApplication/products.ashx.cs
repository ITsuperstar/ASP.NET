using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MetalsWebApplication
{
    /// <summary>
    /// products 的摘要说明
    /// </summary>
    public class products : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            int PageNumber = 1;
            DataTable menufirst = new DataTable();
            DataTable prorecom = new DataTable();
            DataTable proclass = new DataTable();
            int proclassCount = (int)SqlHelper.ExecuteScalar("select count(*) from proclassinfo");
            object[] proclassData = new object[proclassCount];
            try
            {
                menufirst = SqlHelper.ExecuteDataTable("select * from menufirst ");
                prorecom = SqlHelper.ExecuteDataTable("select * from prorecommend,proinfo where proinfo.proid=prorecommend.proid");
                proclass = SqlHelper.ExecuteDataTable("select * from proclassinfo ");

                //foreach(var procalssInfo in proclass.Rows){}
                for (int i = 0; i < proclassCount; i++)
                {
                    int proclassid=(int)proclass.Rows[i].ItemArray[0];
                    string proclassname = (string)proclass.Rows[i].ItemArray[1];
                    DataTable pros = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by proclickrate desc) as num from  proinfo where proclassid=@proclassid) s where s.num>@Start and s.num<@End",
                                            new SqlParameter("@proclassid", proclassid),
                                            new SqlParameter("@Start", (PageNumber - 1) * 3),
                                            new SqlParameter("@End", PageNumber * 3 + 1));
                    proclassData[i] = new { proclassid = proclassid, proclassname = proclassname, pros = pros.Rows };
                }

            }
            catch (Exception e) { context.Response.Write("访问数据库出错！"); }

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

            var Data = new { menufirsts = menufirst.Rows, prorecoms = prorecom.Rows, proclasses = proclass.Rows, proclassdata = proclassData, linkclassData = linkclassData };
            string html = CommonHelper.RenderHtml("products.html", Data);
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