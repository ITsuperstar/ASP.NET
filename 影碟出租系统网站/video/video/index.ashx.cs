using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace video
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            var Data = new { };
            string html = CommonHelper.RenderHtml("index.html", Data);
            context.Response.Write(html);
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