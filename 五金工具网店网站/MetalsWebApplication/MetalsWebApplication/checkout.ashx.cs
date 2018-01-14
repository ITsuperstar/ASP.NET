using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MetalsWebApplication
{
    /// <summary>
    /// checkout 的摘要说明
    /// </summary>
    public class checkout : IHttpHandler, IRequiresSessionState
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
                double totalprice = 0;
                string ordernum = "无";
                string proordernum = (string)context.Session["proordernum"];
                //计算总价，如果Session中有订单号则从Session中取得订单号计算，否则根据userid从数据库中读取订单号
                if (!string.IsNullOrEmpty(proordernum))
                {
                    DataTable orderDetails = SqlHelper.ExecuteDataTable("select * from proorderdetails where proordernum=@proordernum",
                        new SqlParameter("@proordernum", proordernum));
                    if (orderDetails.Rows.Count > 0)
                    {
                        double sum = 0;
                        for (int i = 1; i <= 6; i++)
                        {
                            string proidColumn = "proid" + i;
                            string quantityColumn = "quantity" + i;
                            bool proidnull = (orderDetails.Rows[0][proidColumn] == System.DBNull.Value);
                            if (!proidnull)
                            {
                                int proid = (int)orderDetails.Rows[0][proidColumn];
                                int quantity = (int)orderDetails.Rows[0][quantityColumn];
                                double price = (double)SqlHelper.ExecuteScalar("select proprice from proinfo where proid=@proid",
                                     new SqlParameter("@proid", proid));
                                sum = sum + quantity * price;
                            }
                        }
                        totalprice = sum;
                        ordernum = proordernum;
                    }
                }
                else
                {
                    int userid = (int)SqlHelper.ExecuteScalar("select userid from userlogin where username=@username",
                                      new SqlParameter("@username", username));
                    DataTable proordernumdt = SqlHelper.ExecuteDataTable("select proordernum from proorder where userid=@userid and state=0",
                                  new SqlParameter("@userid", userid));
                    //表示有未支付的订单
                    if (proordernumdt.Rows.Count > 0)
                    {
                        string proordernum1 = (string)proordernumdt.Rows[0]["proordernum"];
                        if (!string.IsNullOrEmpty(proordernum1))
                        {
                            context.Session["proordernum"] = proordernum1;
                            DataTable orderDetails = SqlHelper.ExecuteDataTable("select * from proorderdetails where proordernum=@proordernum",
                                     new SqlParameter("@proordernum", proordernum1));
                            double sum = 0;
                            for (int i = 1; i <= 6; i++)
                            {
                                string proidColumn = "proid" + i;
                                string quantityColumn = "quantity" + i;
                                bool proidnull = (orderDetails.Rows[0][proidColumn] == System.DBNull.Value);
                                if (!proidnull)
                                {
                                    int proid = (int)orderDetails.Rows[0][proidColumn];
                                    int quantity = (int)orderDetails.Rows[0][quantityColumn];
                                    double price = (double)SqlHelper.ExecuteScalar("select proprice from proinfo where proid=@proid",
                                         new SqlParameter("@proid", proid));
                                    sum = sum + quantity * price;
                                }
                            }
                            totalprice = sum;
                            ordernum = proordernum1;
                        }
                    }
                }

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

                var Data = new { menufirsts = menufirst.Rows, proclasses = proclass.Rows, linkclassData = linkclassData, totalprice = totalprice, ordernum = ordernum };
                string html = CommonHelper.RenderHtml("checkout.html", Data);
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