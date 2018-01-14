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
    /// orderCreat 的摘要说明
    /// </summary>
    public class orderCreat : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = (string)context.Session["username"];
            bool isLogin = string.IsNullOrEmpty(username);
            if (isLogin)
                context.Response.Write("nologin");
            else
            {
                int userid = (int)SqlHelper.ExecuteScalar("select userid from userlogin where username=@username",
                    new SqlParameter("@username", username));

                DataTable proordernumdt = SqlHelper.ExecuteDataTable("select proordernum from proorder where userid=@userid and state=0",
                                     new SqlParameter("@userid", userid));
                //表示有未支付的订单
                if (proordernumdt.Rows.Count > 0)
                {
                    context.Response.Write("haveorder");
                }
                else
                {
                    DateTime currentTime = DateTime.Now;
                    string proordernum = OrderIdHelper.GenerateId();

                    int count = int.Parse(context.Request["checkedcount"]);
                    int[] car = new int[count];          //选择购买的产品的carid，在shoppingcar表中
                    int[] proid = new int[count];       //每个产品的proid，quantity  
                    int[] quantity = new int[count];
                    //订单成功后，在把购物车的数据删除或者改状态
                    for (int i = 1; i <= count; i++)
                    {
                        string caridtemp = "carid" + i;
                        car[(i - 1)] = int.Parse(context.Request[caridtemp]);
                    }

                    for (int i = 0; i < count; i++)
                    {
                        int carid = car[i];
                        DataTable dt = SqlHelper.ExecuteDataTable("select *  from shoppingcar where carid=@carid",
                            new SqlParameter("@carid", carid));
                        proid[i] = (int)dt.Rows[0]["proid"];
                        quantity[i] = (int)dt.Rows[0]["quantity"];
                    }
                    //订单state值为0时表示“待支付”，1时表示“成功支付”，-1时表示“已取消”
                    string value1 = "'" + proordernum + "'," + userid + "," + "'" + currentTime + "'," + 0;
                    string sqlstring1 = "insert into proorder(proordernum,userid,creattime,state) values(" + value1 + ")";
                    string value2 = "'" + proordernum + "'";
                    string insertColumn = "proordernum";
                    for (int i = 0; i < count; i++)
                    {
                        value2 = value2 + "," + proid[i] + "," + quantity[i];
                        insertColumn = insertColumn + ",proid" + (i + 1) + ",quantity" + (i + 1);
                    }
                    string sqlstring2 = "insert into proorderdetails(" + insertColumn + ")  values(" + value2 + ")";

                    //事务操作，对数据库中的2个表同时操作
                    SqlConnection con = SqlHelper.OpenConnection();
                    //先实例SqlTransaction类，使用这个事务使用的是con 这个连接，使用BeginTransaction这个方法来开始执行这个事务
                    SqlTransaction tran = con.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.Transaction = tran;
                    try
                    {
                        //在try{} 块里执行sqlcommand命令，
                        cmd.CommandText = sqlstring1;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = sqlstring2;
                        cmd.ExecuteNonQuery();
                        tran.Commit();     //如果两个sql命令都执行成功，则执行commit这个方法，执行这些操作
                        context.Session["proordernum"] = proordernum;      //写订单号Session
                    }
                    catch
                    {
                        tran.Rollback();   //如何执行不成功，发生异常，则执行rollback方法，回滚到事务操作开始之前；
                    }
                    finally
                    {
                        con.Close();
                    }
                    context.Response.Write("success");
                }
            }
        }

        //事务操作
        /*private  static void transaction()
        {
            //SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;database=aaaa;uid=sa;pwd=jcx");
            //con.Open();
            SqlConnection con=SqlHelper.OpenConnection();
            SqlTransaction tran = con.BeginTransaction();  //先实例SqlTransaction类，使用这个事务使用的是con 这个连接，使用BeginTransaction这个方法来开始执行这个事务
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Transaction = tran;
            try
            {
                //在try{} 块里执行sqlcommand命令，
                cmd.CommandText = "update bb set moneys=moneys-'" + "' where ID='1'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "update bb set moneys=moneys+' aa ' where ID='2'";
                cmd.ExecuteNonQuery();
                tran.Commit();    //如果两个sql命令都执行成功，则执行commit这个方法，执行这些操作
            }
            catch
            {
                tran.Rollback();   //如何执行不成功，发生异常，则执行rollback方法，回滚到事务操作开始之前；
            }
        }*/

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}