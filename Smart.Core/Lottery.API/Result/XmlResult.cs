using System.IO;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.API.Result
{
    public class XmlResult: ActionResult
    {
        // 可被序列化的内容
        private object _Data { get; set; }

        // 构造器
        public XmlResult(object data)
        {
            _Data = data;
        }
        public override void ExecuteResult(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.ContentType = "text/xml";
            if (_Data != null)
            {

                XmlSerializer serializer = new XmlSerializer(_Data.GetType());
                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, _Data);
                response.Body.WriteAsync(ms.ToArray());
            }
            base.ExecuteResult(context);
        }
    }
}
