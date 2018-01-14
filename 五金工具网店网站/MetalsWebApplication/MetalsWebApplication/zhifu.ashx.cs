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
    /// zhifu 的摘要说明
    /// </summary>
    public class zhifu : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string acton=context.Request["action"];
            string zhifu="";
            if(acton=="zhifubao")
                 zhifu="支付宝";
            else if(acton=="paypal")
                 zhifu="PAYPAL";
            else
                zhifu="其他";
            string username = (string)context.Session["username"];
            bool isLogin = string.IsNullOrEmpty(username);
            if (isLogin)
                context.Response.Redirect("/Login.ashx");
            else
            {
                string ordernum = "无";
                double totalprice = 0;
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

                var Data = new {username=username,zhifu=zhifu, ordernum = ordernum, totalprice = totalprice };
                string html = CommonHelper.RenderHtml("zhifu.html", Data);
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