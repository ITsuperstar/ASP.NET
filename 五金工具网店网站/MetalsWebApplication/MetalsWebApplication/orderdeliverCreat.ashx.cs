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
    /// orderdeliverCreat 的摘要说明
    /// </summary>
    public class orderdeliverCreat : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = (string)context.Session["username"];
            bool isLogin = string.IsNullOrEmpty(username);
            if (isLogin)
                context.Response.Redirect("/Login.ashx");
            else
            {
                string name = context.Request["name"];
                string email = context.Request["email"];
                string address = context.Request["address"];
                string telphone = context.Request["telphone"];
                string proordernum = (string)context.Session["proordernum"];
                //如果Session中有订单号则从Session中取得订单号
                if (!string.IsNullOrEmpty(proordernum))
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery("insert into orderdeliver(proordernum,name,email,address,telphone)  values(@proordernum,@name,@email,@address,@telphone)",
                         new SqlParameter("@proordernum", proordernum), new SqlParameter("@name", name), new SqlParameter("@email", email),
                         new SqlParameter("@address", address), new SqlParameter("@telphone", telphone));
                    }
                    catch (Exception e)
                    {
                        context.Response.Write("failure");
                        return;
                    }
                    context.Response.Write("success");
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
                            try
                            {
                                SqlHelper.ExecuteNonQuery("insert into orderdeliver(proordernum,name,email,address,telphone)  values(@proordernum,@name,@email,@address,@telphone)",
                                 new SqlParameter("@proordernum", proordernum1), new SqlParameter("@name", name), new SqlParameter("@email", email),
                                 new SqlParameter("@address", address), new SqlParameter("@telphone", telphone));
                            }
                            catch (Exception e)
                            {
                                context.Response.Write("failure");
                                return;
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