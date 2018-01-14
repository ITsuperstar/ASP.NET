using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MetalsWebApplication
{
    /// <summary>
    /// productclass 的摘要说明
    /// </summary>
    public class productclass : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html"; 
            int proclassid = int.Parse(context.Request["proclassid"].ToString());
            int PageNumber = 1;
            int pagec = int.Parse(ConfigurationManager.AppSettings["pagecount"].ToString());
            if (context.Request["PageNumber"] != null)
            {
                Regex r = new Regex(@"^\d*$");
                if (r.IsMatch(context.Request["PageNumber"]))
                    PageNumber = int.Parse(context.Request["PageNumber"].ToString());
            }

            string proclassname = null;
            DataTable menufirst = new DataTable();
            DataTable prorecom = new DataTable();
            DataTable proclass = new DataTable();
            DataTable pros= new DataTable();
            try
            {
                proclassname = (string)SqlHelper.ExecuteScalar("select proclassname from proclassinfo where proclassid=@proclassid", 
                                         new SqlParameter("@proclassid", proclassid));
                menufirst = SqlHelper.ExecuteDataTable("select * from menufirst ");
                prorecom = SqlHelper.ExecuteDataTable("select * from prorecommend,proinfo where proinfo.proid=prorecommend.proid");
                proclass = SqlHelper.ExecuteDataTable("select * from proclassinfo ");
                pros = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by proclickrate desc) as num from  proinfo where proclassid=@proclassid) s where s.num>@Start and s.num<@End",
                             new SqlParameter("@proclassid", proclassid) ,
                             new SqlParameter("@Start", (PageNumber - 1) * pagec),
                              new SqlParameter("@End", PageNumber * pagec + 1));
            }
            catch (Exception e) { context.Response.Write("访问数据库出错！"); }

            int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from proinfo where proclassid=@proclassid", new SqlParameter("@proclassid", proclassid));
            int pageCount = (int)Math.Ceiling(totalCount / (double)pagec);

            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "/productclass.ashx?proclassid=" + proclassid.ToString() + "&PageNumber=" + (i + 1), Title = i + 1 };
            }


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

            var Data = new {proclassname=proclassname, menufirsts = menufirst.Rows, prorecoms = prorecom.Rows, proclasses = proclass.Rows, pros = pros.Rows
                                      ,proclassid=proclassid,PageData = pageData,TotalCount = totalCount,PageCount = pageCount,PageNumber = PageNumber, linkclassData = linkclassData };
            string html = CommonHelper.RenderHtml("productclass.html", Data);
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