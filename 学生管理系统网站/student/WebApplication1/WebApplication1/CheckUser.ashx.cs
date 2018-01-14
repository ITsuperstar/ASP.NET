using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication1
{
    /// <summary>
    /// CheckUser 的摘要说明
    /// </summary>
    public class CheckUser : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string act = context.Request["action"];
            bool isLogin = !string.IsNullOrEmpty((string)context.Session["username"]) && !string.IsNullOrEmpty((string)context.Session["password"]);
            if (isLogin) { context.Response.Redirect("main.ashx"); }
            else{
           //第一次加载！
            if (act == "checkName")
            {
                string username = context.Request["username"];
                int n = 0;
                try
                {
                    n = (int)SqlHelper.ExecuteScalar("select count(*) from userLogin where username=@username"
                                        , new SqlParameter("@username", username));
                }
                catch (Exception e)
                { context.Response.Write("访问数据库错误！"); }

                if (n < 1)
                { context.Response.Write("error"); }
                else
                { context.Response.Write("exist"); }
            }

            else if (act == "checkPassword")
            {
                string username = context.Request["username"];
                string password = context.Request["password"];
                int n = 0, m = 0;
                try
                {
                    n = (int)SqlHelper.ExecuteScalar("select count(*) from userLogin where username=@username"
                                        , new SqlParameter("@username", username));
                }
                catch (Exception e)
                { context.Response.Write("访问数据库错误！"); }

                if (n < 1)
                { context.Response.Write("usernameError"); }
                else
                {
                    try
                    {
                        m = (int)SqlHelper.ExecuteScalar("select count(*) from userLogin where username=@username and password=@password"
                                            , new SqlParameter("@username", username), new SqlParameter("@password", password));
                    }
                    catch (Exception e)
                    { context.Response.Write("访问数据库错误！"); }
                    if (m < 1)
                    { context.Response.Write("passwordError"); }
                    else { context.Response.Write("exist"); }
                }
            }

            else if (act == "checkYZM")
            {
                string idCode = context.Request["idCode"];
                if (context.Session["check"].ToString() != idCode)
                { context.Response.Write("error"); }
                else
                { context.Response.Write("exist"); }
            }

            else if (act == "checkLogin")
            {
                string username = context.Request["username"];
                string password = context.Request["password"];
                string idCode = context.Request["idCode"];
                int m = 0;
                try
                {
                    m = (int)SqlHelper.ExecuteScalar("select count(*) from userLogin where username=@username and password=@password"
                                        , new SqlParameter("@username", username), new SqlParameter("@password", password));
                }
                catch (Exception e)
                { context.Response.Write("访问数据库错误！"); }
                if (m < 1)
                { context.Response.Write("error"); }
                else
                {
                    if (context.Session["check"].ToString() != idCode)
                    { context.Response.Write("YZMerror"); }
                    else
                    {
                        context.Session["username"] = username;
                        context.Session["password"] = password;
                        context.Response.Write("exist");
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