using APISample.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISample.Filters
{
    class ResultFilter : IAsyncResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            var controller = (BaseController)context.Controller;

            var result = new
            {
                // 取得由 API 返回的狀態碼
                //result.Status = actionExecutedContext.ActionContext.Response.StatusCode;
                Status = controller.Status,

                // 取得由 API 返回的 Result Code
                //result.Result = 0;
                Result = controller.Result,

                // 取得由 API 返回的資料
                //result.Data = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
                Data = controller.Data,

                // 取得由 API 返回的處理訊息
                Message = controller.Message
            };

           
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //throw new NotImplementedException();
            context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {

            await next();

            //處理成功回傳值
            var controller = (BaseController)context.Controller;

            var result = new
            {
                // 取得由 API 返回的狀態碼               
                Status = controller.Status,

                // 取得由 API 返回的 Result Code               
                Result = controller.Result,

                // 取得由 API 返回的資料                
                data = controller.Data,

                // 取得由 API 返回的處理訊息
                Message = controller.Message
            };


            var json = JsonConvert.SerializeObject(result, Formatting.None);

            var bytes = Encoding.UTF8.GetBytes(json);
            int length = bytes.Length;

            var response = context.HttpContext.Response;
            //this must be set before start writing into the stream
            response.ContentLength = length;
            response.ContentType = "application/json";

            await response.Body.WriteAsync(bytes, 0, length);


            

        }
    }
}
