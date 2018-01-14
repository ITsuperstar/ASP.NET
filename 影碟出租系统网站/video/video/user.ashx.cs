using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace video
{
    /// <summary>
    /// user 的摘要说明
    /// </summary>
    public class user : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string clientid=(string)context.Session["username"];
            bool isLogin = !string.IsNullOrEmpty(clientid);
            //没有登录，重定向到登录页
            if (isLogin==false)
            {
                context.Response.Redirect("/Login.ashx"); 
            }
            //已登录，渲染user.html
            else
            {
                //已借影碟的数据信息，有分页
                int pagec = int.Parse(ConfigurationManager.AppSettings["pagecount"].ToString());  //从webconfig中获取每页展示的列数
                int PageNumber = 1;
                DataTable borrowinfo = new DataTable();
                if (context.Request["PageNumber"] != null)
                {
                    Regex r = new Regex(@"^\d*$");
                    if (r.IsMatch(context.Request["PageNumber"]))
                        PageNumber = int.Parse(context.Request["PageNumber"].ToString());
                }
                try
                {
                    borrowinfo = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id desc) as num from  hireinfo where clientid=@clientid) s,videoinfo where videoinfo.videoid=s.videoid and s.num>@Start and s.num<@End",
                                            new SqlParameter("@clientid", clientid),
                                            new SqlParameter("@Start", (PageNumber - 1) * pagec),
                                            new SqlParameter("@End", PageNumber * pagec + 1));
                }
                catch (Exception e) { context.Response.Write("访问数据库出错！"); }

                int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from hireinfo where clientid=@clientid", new SqlParameter("@clientid", clientid));
                int pageCount = (int)Math.Ceiling(totalCount / (double)pagec);

                object[] pageData = new object[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    pageData[i] = new { Href = "/user.ashx?PageNumber=" + (i + 1), Title = i + 1 };
                }
                //后端传递前一页和后一页的链接
                string prehref = "";
                string reahref = "";
                if (PageNumber > 1)
                    prehref = "/user.ashx?PageNumber=" + (PageNumber - 1);
                if (PageNumber < pageCount)
                    reahref = "/user.ashx?PageNumber=" + (PageNumber + 1);
                
                //未归还影碟的数据信息
                DataTable  noreturninfo = new DataTable();
                try
                {
                    noreturninfo = SqlHelper.ExecuteDataTable("select * from hireinfo,videoinfo where hireinfo.clientid=@clientid and hireinfo.isreturn=0 and videoinfo.videoid=hireinfo.videoid",
                                            new SqlParameter("@clientid", clientid));
                }
                catch (Exception e) { context.Response.Write("访问数据库出错！"); }

                //评论列表的数据信息
                DataTable review = new DataTable();
                try
                {
                    string email = (string)SqlHelper.ExecuteScalar("select email from clientinfo where clientid=@clientid", new SqlParameter("@clientid", clientid));
                    review = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by id desc) as num from  review where email=@email) s,videoinfo where videoinfo.videoid=s.videoid",
                                            new SqlParameter("@email", email) );
                }
                catch (Exception e) { context.Response.Write("访问数据库出错！"); }

                var Data = new
                {
                    borrowinfo = borrowinfo.Rows,
                    PageData = pageData,
                    TotalCount = totalCount,
                    PageCount = pageCount,
                    PageNumber = PageNumber,
                    prehref = prehref,
                    reahref = reahref,
                    noreturninfo=noreturninfo.Rows,
                    review=review.Rows,
                    clientid=clientid
                };
                string html = CommonHelper.RenderHtml("user.html", Data);
                context.Response.Write(html);
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