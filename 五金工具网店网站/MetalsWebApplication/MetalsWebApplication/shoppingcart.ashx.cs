using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace MetalsWebApplication
{
    /// <summary>
    /// shoppingcart 的摘要说明
    /// </summary>
    public class shoppingcart : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
           string username = (string)context.Session["username"];
            bool isLogin = string.IsNullOrEmpty(username);
            if (isLogin)
                context.Response.Redirect("/Login.ashx");
            else
            {
                int userid = (int)SqlHelper.ExecuteScalar("select userid from userlogin where username=@username",
                                   new SqlParameter("@username", username));
                int PageNumber = 1;
                if (context.Request["PageNumber"] != null)
                {
                    Regex r = new Regex(@"^\d*$");
                    if (r.IsMatch(context.Request["PageNumber"]))
                        PageNumber = int.Parse(context.Request["PageNumber"].ToString());
                }

                DataTable menufirst = new DataTable();
                DataTable proclass = new DataTable();
                DataTable pros = new DataTable();
                try
                {
                    menufirst = SqlHelper.ExecuteDataTable("select * from menufirst ");
                    proclass = SqlHelper.ExecuteDataTable("select * from proclassinfo ");
                    pros = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by quantity desc) as num from  shoppingcar where userid=@userid and state='购物车') s,proinfo where s.num>@Start and s.num<@End and proinfo.proid=s.proid",
                                 new SqlParameter("@userid", userid),
                                 new SqlParameter("@Start", (PageNumber - 1) * 6),
                                 new SqlParameter("@End", PageNumber * 6+ 1));
                }
                catch (Exception e) { context.Response.Write("访问数据库出错！"); }

                int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from shoppingcar where userid=@userid and state='购物车'", new SqlParameter("@userid", userid));
                int pageCount = (int)Math.Ceiling(totalCount / 6.0);
                object[] pageData = new object[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    pageData[i] = new { Href = "/shoppingcart.ashx?PageNumber=" + (i + 1), Title = i + 1 };
                }


                DataTable linkclass = SqlHelper.ExecuteDataTable("select * from linkclass ");
                int linkclassCount = (int)SqlHelper.ExecuteScalar("select count(*) from linkclass");
                int linknumber = 1;
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

                var Data = new { menufirsts = menufirst.Rows, proclasses = proclass.Rows, pros = pros.Rows, linkclassData = linkclassData,
                                           PageData = pageData, TotalCount = totalCount, PageCount = pageCount, PageNumber = PageNumber};
                string html = CommonHelper.RenderHtml("shoppingcart.html", Data);
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