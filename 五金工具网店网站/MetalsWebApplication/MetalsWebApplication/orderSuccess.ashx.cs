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
    /// orderSuccess 的摘要说明
    /// </summary>
    public class orderSuccess : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //订单成功后，改变订单的state=1，把订单号的Session清除，把订单的数据从购物车中删除删除
            string username = (string)context.Session["username"];
            bool isLogin = string.IsNullOrEmpty(username);
            if (isLogin)
                context.Response.Write("nologin");
            else
            {
                int userid = (int)SqlHelper.ExecuteScalar("select userid from userlogin where username=@username",
                  new SqlParameter("@username", username));
                string proordernum = (string)context.Session["proordernum"];
                //如果Session中有订单号则从Session中取得订单号，否则根据userid从数据库中读取订单号
                if (!string.IsNullOrEmpty(proordernum))
                {
                    //改变订单的state=1
                    SqlHelper.ExecuteNonQuery("update proorder set state=1 where proordernum=@proordernum",
                          new SqlParameter("@proordernum", proordernum));
                    context.Session["proordernum"] = null;       //清除订单号的Session
                    //把订单的数据从购物车中删除删除
                    DataTable orderDetails = SqlHelper.ExecuteDataTable("select * from proorderdetails where proordernum=@proordernum",
                        new SqlParameter("@proordernum", proordernum));
                    if (orderDetails.Rows.Count > 0)
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            string proidColumn = "proid" + i;
                            bool proidnull = (orderDetails.Rows[0][proidColumn] == System.DBNull.Value);
                            if (!proidnull)
                            {
                                int proid = (int)orderDetails.Rows[0][proidColumn];
                                SqlHelper.ExecuteNonQuery("delete from shoppingcar where userid=@userid and proid=@proid",
                                       new SqlParameter("@userid", userid), new SqlParameter("@proid", proid));
                            }
                        }
                    }
                    context.Response.Write("success");
                }
                else
                {

                    DataTable proordernumdt = SqlHelper.ExecuteDataTable("select proordernum from proorder where userid=@userid and state=0",
                                  new SqlParameter("@userid", userid));
                    //表示有未支付的订单
                    if (proordernumdt.Rows.Count > 0)
                    {
                        string proordernum1 = (string)proordernumdt.Rows[0]["proordernum"];
                        if (!string.IsNullOrEmpty(proordernum1))
                        {
                            //改变订单的state=1
                            SqlHelper.ExecuteNonQuery("update proorder set state=1 where proordernum=@proordernum",
                                  new SqlParameter("@proordernum", proordernum1));
                            context.Session["proordernum"] = null;       //清除订单号的Session

                            DataTable orderDetails = SqlHelper.ExecuteDataTable("select * from proorderdetails where proordernum=@proordernum",
                                     new SqlParameter("@proordernum", proordernum1));
                            for (int i = 1; i <= 6; i++)
                            {
                                string proidColumn = "proid" + i;
                                bool proidnull = (orderDetails.Rows[0][proidColumn] == System.DBNull.Value);
                                if (!proidnull)
                                {
                                    int proid = (int)orderDetails.Rows[0][proidColumn];
                                    SqlHelper.ExecuteNonQuery("delete from shoppingcar where userid=@userid and proid=@proid",
                                           new SqlParameter("@userid", userid), new SqlParameter("@proid", proid));
                                }
                            }
                            context.Response.Write("success");
                        }
                    }
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