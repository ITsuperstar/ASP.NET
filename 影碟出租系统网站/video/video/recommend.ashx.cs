using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace video 
{
    /// <summary>
    /// recommend 的摘要说明
    /// </summary>
    public class recommend : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            int PageNumber = 1;
            int pagec = int.Parse(ConfigurationManager.AppSettings["pagecount"].ToString());  //从webconfig中获取每页展示的个数
            DataTable videoinfo = new DataTable();
            if (context.Request["PageNumber"] != null)
            {
                Regex r = new Regex(@"^\d*$");
                if (r.IsMatch(context.Request["PageNumber"]))
                    PageNumber = int.Parse(context.Request["PageNumber"].ToString());
            }

            try
            {
                videoinfo = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id asc) as num from  recommend) s,videoinfo where videoinfo.videoid=s.videoid and s.num>@Start and s.num<@End",
                                        new SqlParameter("@Start", (PageNumber - 1) * pagec),
                                        new SqlParameter("@End", PageNumber * pagec + 1));
            }
            catch (Exception e) { context.Response.Write("访问数据库出错！"); }

            int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from recommend");
            int pageCount = (int)Math.Ceiling(totalCount / (double)pagec);

            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "/recommend.ashx?PageNumber=" + (i + 1), Title = i + 1 };
            }
            //后端传递前一页和后一页的链接
            string prehref = "";
            string reahref = "";
            if (PageNumber > 1)
                prehref = "/recommend.ashx?PageNumber=" + (PageNumber - 1);
            if (PageNumber < pageCount)
                reahref = "/recommend.ashx?PageNumber=" + (PageNumber + 1);

            var Data = new
            {
                videoinfo = videoinfo.Rows,
                PageData = pageData,
                TotalCount = totalCount,
                PageCount = pageCount,
                PageNumber = PageNumber,
                prehref = prehref,
                reahref = reahref
            };
            string html = CommonHelper.RenderHtml("recommend.html", Data);
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