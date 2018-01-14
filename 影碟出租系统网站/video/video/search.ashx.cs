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
    /// search 的摘要说明
    /// </summary>
    public class search : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string SearchContent = context.Request["searchcontent"];
            int SearchType=1;
            if (context.Request["searchtype"] != null)
            {
                SearchType = int.Parse(context.Request["searchtype"].ToString());
            }
            DataTable searchinfo = new DataTable();
            int PageNumber = 1;
            int pagec = int.Parse(ConfigurationManager.AppSettings["pagecount"].ToString());  //从webconfig中获取每页展示的个数
            if (context.Request["PageNumber"] != null)
            {
                Regex r = new Regex(@"^\d*$");
                if (r.IsMatch(context.Request["PageNumber"]))
                    PageNumber = int.Parse(context.Request["PageNumber"].ToString());
            }
            //首次访问，展示所有影碟
            if (string.IsNullOrEmpty(SearchContent))
            {
                try
                {
                    searchinfo = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id asc) as num from  videoinfo) s where  s.num>@Start and s.num<@End",
                                            new SqlParameter("@Start", (PageNumber - 1) * pagec),
                                            new SqlParameter("@End", PageNumber * pagec + 1));
                }
                catch (Exception e) { context.Response.Write("访问数据库出错！"); }

                int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from videoinfo");
                int pageCount = (int)Math.Ceiling(totalCount / (double)pagec);

                object[] pageData = new object[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    pageData[i] = new { Href = "/search.ashx?PageNumber=" + (i + 1), Title = i + 1 };
                }
                //后端传递前一页和后一页的链接
                string prehref = "";
                string reahref = "";
                if (PageNumber > 1)
                    prehref = "/search.ashx?PageNumber=" + (PageNumber - 1);
                if (PageNumber < pageCount)
                    reahref = "/search.ashx?PageNumber=" + (PageNumber + 1);

                var Data = new
                {
                    searchinfo = searchinfo.Rows,
                    PageData = pageData,
                    TotalCount = totalCount,
                    PageCount = pageCount,
                    PageNumber = PageNumber,
                    prehref = prehref,
                    reahref = reahref
                };
                string html = CommonHelper.RenderHtml("search.html", Data);
                context.Response.Write(html);
            }

            //按照搜索内容推荐
            else
            {
                if (SearchType == 1)
                {
                    searchinfo = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id asc) as num from  videoinfo where videoname  like @videoname) s where s.num>@Start and s.num<@End",
                                            new SqlParameter("@videoname", "%" + SearchContent + "%"), 
                                            new SqlParameter("@Start", (PageNumber - 1) * pagec),
                                            new SqlParameter("@End", PageNumber * pagec + 1));

                    int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from videoinfo where videoname like @videoname", new SqlParameter("@videoname", "%" + SearchContent + "%"));
                    int pageCount = (int)Math.Ceiling(totalCount / (double)pagec);

                    object[] pageData = new object[pageCount];
                    for (int i = 0; i < pageCount; i++)
                    {
                        pageData[i] = new { Href = "/search.ashx?PageNumber=" + (i + 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType, Title = i + 1 };
                    }
                    //后端传递前一页和后一页的链接
                    string prehref = "";
                    string reahref = "";
                    if (PageNumber > 1)
                        prehref = "/search.ashx?PageNumber=" + (PageNumber - 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType;
                    if (PageNumber < pageCount)
                        reahref = "/search.ashx?PageNumber=" + (PageNumber + 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType;

                    var Data = new
                    {
                        searchinfo = searchinfo.Rows,
                        PageData = pageData,
                        TotalCount = totalCount,
                        PageCount = pageCount,
                        PageNumber = PageNumber,
                        prehref = prehref,
                        reahref = reahref
                    };
                    string html = CommonHelper.RenderHtml("search.html", Data);
                    context.Response.Write(html);
                }

                else if (SearchType == 2)
                {
                    searchinfo = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id asc) as num from  videoinfo where videoid= @videoid) s where s.num>@Start and s.num<@End",
                                            new SqlParameter("@videoid", SearchContent),
                                            new SqlParameter("@Start", (PageNumber - 1) * pagec),
                                            new SqlParameter("@End", PageNumber * pagec + 1));

                    int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from videoinfo where videoid= @videoid", new SqlParameter("@videoid", SearchContent));
                    int pageCount = (int)Math.Ceiling(totalCount / (double)pagec);

                    object[] pageData = new object[pageCount];
                    for (int i = 0; i < pageCount; i++)
                    {
                        pageData[i] = new { Href = "/search.ashx?PageNumber=" + (i + 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType, Title = i + 1 };
                    }
                    //后端传递前一页和后一页的链接
                    string prehref = "";
                    string reahref = "";
                    if (PageNumber > 1)
                        prehref = "/search.ashx?PageNumber=" + (PageNumber - 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType;
                    if (PageNumber < pageCount)
                        reahref = "/search.ashx?PageNumber=" + (PageNumber + 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType;

                    var Data = new
                    {
                        searchinfo = searchinfo.Rows,
                        PageData = pageData,
                        TotalCount = totalCount,
                        PageCount = pageCount,
                        PageNumber = PageNumber,
                        prehref = prehref,
                        reahref = reahref
                    };
                    string html = CommonHelper.RenderHtml("search.html", Data);
                    context.Response.Write(html);
                }

                else  if (SearchType == 3)
                {
                    searchinfo = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id asc) as num from  videoinfo where buydate=@buydate) s where s.num>@Start and s.num<@End",
                                            new SqlParameter("@buydate", SearchContent),
                                            new SqlParameter("@Start", (PageNumber - 1) * pagec),
                                            new SqlParameter("@End", PageNumber * pagec + 1));

                    int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from videoinfo where buydate=@buydate", new SqlParameter("@buydate", SearchContent));
                    int pageCount = (int)Math.Ceiling(totalCount / (double)pagec);

                    object[] pageData = new object[pageCount];
                    for (int i = 0; i < pageCount; i++)
                    {
                        pageData[i] = new { Href = "/search.ashx?PageNumber=" + (i + 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType, Title = i + 1 };
                    }
                    //后端传递前一页和后一页的链接
                    string prehref = "";
                    string reahref = "";
                    if (PageNumber > 1)
                        prehref = "/search.ashx?PageNumber=" + (PageNumber - 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType;
                    if (PageNumber < pageCount)
                        reahref = "/search.ashx?PageNumber=" + (PageNumber + 1) + "&searchcontent=" + SearchContent + "&searchtype=" + SearchType;

                    var Data = new
                    {
                        searchinfo = searchinfo.Rows,
                        PageData = pageData,
                        TotalCount = totalCount,
                        PageCount = pageCount,
                        PageNumber = PageNumber,
                        prehref = prehref,
                        reahref = reahref
                    };
                    string html = CommonHelper.RenderHtml("search.html", Data);
                    context.Response.Write(html);
                }

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