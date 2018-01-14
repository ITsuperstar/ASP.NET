using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// StudentEdit 的摘要说明
    /// </summary>
    public class StudentEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string sid=context.Request["EditID"];
            if (!string.IsNullOrEmpty(sid)) 
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select student.*,classname  from student,class where sid=@sid and student.cid=class.cid ", new SqlParameter("@sid", sid));
                DataTable dtclass = SqlHelper.ExecuteDataTable("select cid,classname  from class");
                string sexOptions = "";
                string specialtiesOptions = "";
                if(dt.Rows[0].ItemArray[2].ToString()=="男")
                {
                    sexOptions = "女";
                }
                else if (dt.Rows[0].ItemArray[2].ToString() == "女")
                {
                    sexOptions = "男";
                }
                if (dt.Rows[0].ItemArray[6].ToString() == "是")
                {
                    specialtiesOptions = "否";
                }
                else if (dt.Rows[0].ItemArray[6].ToString() == "否")
                {
                    specialtiesOptions = "是";
                }
                for (int i = 0; i < dtclass.Rows.Count;i++ )
                    if (dtclass.Rows[i].ItemArray[0].ToString()==dt.Rows[0].ItemArray[5].ToString())
                    {
                        dtclass.Rows.Remove(dtclass.Rows[i]);
                    }
                var Data = new { Student = dt.Rows[0], classnames = dtclass.Rows, sexOptions = sexOptions, specialtiesOptions = specialtiesOptions };
                string html = CommonHelper.RenderHtml("StudentEdit.html", Data);
                context.Response.Write(html);
            }
            else if (!string.IsNullOrEmpty(context.Request["UpdateID"])) 
            {
                string upid=context.Request["UpdateID"];
                string name = context.Request["name"];
                string sex = context.Request["sex"];
                string birthday = context.Request["birthday"];
                string height = context.Request["height"];
                int cid =int.Parse(context.Request["classname"]);
                string specialties = context.Request["specialties"];
                try
                {
   SqlHelper.ExecuteNonQuery("update  student set name=@name,sex=@sex,birthday=@birthday,height=@height,cid=@cid,specialties=@specialties where sid=@sid",
                        new SqlParameter("@name", name), new SqlParameter("@sex", sex), new SqlParameter("@birthday", birthday)
                       ,new SqlParameter("@height", height), new SqlParameter("@cid", cid), new SqlParameter("@specialties", specialties)
                       ,new SqlParameter("@sid",upid));
                }
                catch (Exception e)
                {
                    context.Response.Write("访问数据库错误！");
                }
                context.Response.Redirect("StudentMain.ashx");

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