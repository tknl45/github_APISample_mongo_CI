using APISample.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISample.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {

        //紀錄
        private readonly ILogger<ExceptionFilter> logger;

        /// <summary>
        /// 起始傳入logger
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="_logger"></param>
        public ExceptionFilter(ILoggerFactory loggerFactory, ILogger<ExceptionFilter> _logger )
        {
             logger = _logger;
        }

        /// <summary>
        /// 自動繼承
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {


            var ex = context.Exception;
            // 構建錯誤信息對象
            var dic = new Dictionary<string, object>
            {
                ["Result"] = -1,
                ["Message"] = ex.Message,
                ["Error_stack"] = ex.StackTrace
            };
            // 設置結果轉為JSON
            context.Result = new JsonResult(dic);
            context.ExceptionHandled = true;

            //log 到 app.log
            logger.LogError(ex,ex.Message);

            //log 到 mongo
            MongoLogger.getInstance().LogError(ex);


            return Task.CompletedTask;
        }


    }
}
