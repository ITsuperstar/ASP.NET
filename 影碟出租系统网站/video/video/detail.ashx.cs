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
    /// detail 的摘要说明
    /// </summary>
    public class detail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string videoid=context.Request["videoid"];
            int pagec = int.Parse(ConfigurationManager.AppSettings["pagecount"].ToString());  //从webconfig中获取每页展示的个数
            int PageNumber = 1;
            if (context.Request["PageNumber"] != null)
            {
                Regex r = new Regex(@"^\d*$");
                if (r.IsMatch(context.Request["PageNumber"]))
                    PageNumber = int.Parse(context.Request["PageNumber"].ToString());
            }
            DataTable videoinfo = new DataTable();
            DataTable recommend = new DataTable();
            DataTable review = new DataTable();
            try
            {
                videoinfo = SqlHelper.ExecuteDataTable("select * from videoinfo where videoid=@videoid ",
                                        new SqlParameter("@videoid", videoid));
                recommend = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id asc) as num from  recommend) s,videoinfo where videoinfo.videoid=s.videoid and s.num>@Start and s.num<@End",
                                        new SqlParameter("@Start", (PageNumber - 1) *4 ),
                                        new SqlParameter("@End", PageNumber *4 + 1));
                review = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id desc) as num from  review where videoid=@videoid) s where s.num>@Start and s.num<@End",
                                        new SqlParameter("@videoid", videoid),
                                        new SqlParameter("@Start", (PageNumber - 1) * 5),
                                        new SqlParameter("@End", PageNumber * 5 + 1));           
            }
            catch (Exception e) { context.Response.Write("访问数据库出错！"); }

            //传递DataTable类型的参数时，正确的传递：videoinfo = videoinfo.Rows ！！！   （错误的传递：videoinfo = videoinfo）
            var Data = new { videoinfo = videoinfo.Rows, recommend = recommend.Rows, review = review.Rows }; 
            string html = CommonHelper.RenderHtml("detail.html", Data);
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