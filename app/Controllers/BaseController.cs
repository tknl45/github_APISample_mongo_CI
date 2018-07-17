using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace APISample.Controllers
{
    public class BaseController : Controller
    {
        public int Result { get; set; } = 1;
        public object Data { get; set; } = null;
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "成功";



        /// <summary>
        /// 紀錄呼叫data，用在filter時寫入log_XXX_YYYY的資料表中
        /// </summary>
        public Dictionary<string, object> LogData { get; set; } = null;

        /// <summary>
        /// 訊息紀錄
        /// </summary>
        public string LogDetailMessage { get; set; } = "";

        
    }
}
