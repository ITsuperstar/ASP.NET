using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MetalsWebApplication
{
    /// <summary>
    /// shoppingcarDeal 的摘要说明
    /// </summary>
    public class shoppingcarDeal : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"];
            string username = (string)context.Session["username"];
            bool isLogin = string.IsNullOrEmpty(username);
            if (isLogin)
                context.Response.Write("nologin");
            else
            {
                int carid = int.Parse(context.Request["carid"]);
                DateTime currentTime = DateTime.Now;
                if (action == "subOne")
                {
                    SqlHelper.ExecuteNonQuery("update shoppingcar set  quantity= quantity-1,time=@currentTime  where carid=@carid",
                     new SqlParameter("@carid", carid), new SqlParameter("@currentTime", currentTime));
                    int laterQuantity = (int)SqlHelper.ExecuteScalar("select quantity from shoppingcar where carid=@carid",
                         new SqlParameter("@carid", carid));
                    if (laterQuantity <= 0)
                    {
                        SqlHelper.ExecuteNonQuery("delete  from shoppingcar where carid=@carid",
                           new SqlParameter("@carid", carid));
                        context.Response.Write("none");
                    }
                    else
                    {
                        context.Response.Write(laterQuantity);
                    }
                }

                else if (action == "addOne")
                {
                    SqlHelper.ExecuteNonQuery("update shoppingcar set  quantity= quantity+1,time=@currentTime  where carid=@carid",
                     new SqlParameter("@carid", carid), new SqlParameter("@currentTime", currentTime));
                    context.Response.Write("success");
                }

                else if (action == "quantity")
                {
                    if (string.IsNullOrEmpty(context.Request["inputQuantity"]))
                    {
                        int laterQuantity = (int)SqlHelper.ExecuteScalar("select quantity from shoppingcar where carid=@carid",
                                      new SqlParameter("@carid", carid));
                        context.Response.Write(laterQuantity);
                    }
                    else
                    {
                        int inputQuantity = int.Parse(context.Request["inputQuantity"]);
                        if (inputQuantity == 0)
                        {
                            SqlHelper.ExecuteNonQuery("delete  from shoppingcar where carid=@carid",
                                   new SqlParameter("@carid", carid));
                            context.Response.Write("delete");
                        }
                        else
                        {
                            SqlHelper.ExecuteNonQuery("update shoppingcar set  quantity=@quantity ,time=@currentTime  where carid=@carid",
                                 new SqlParameter("@quantity", inputQuantity), new SqlParameter("@carid", carid), new SqlParameter("@currentTime", currentTime));
                            context.Response.Write("success");
                        }
                    }
                }

                else if (action == "remove")
                {
                    SqlHelper.ExecuteNonQuery("delete  from shoppingcar where carid=@carid",
                                    new SqlParameter("@carid", carid));
                    context.Response.Write("success");
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