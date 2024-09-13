﻿using Microsoft.AspNetCore.Http;

namespace ToDo.Microservices.Middleware.Querers
{
    public class QuererHttpClientFactoryOptions
    {
        private Func<HttpContext, HttpClient> _option;

        public QuererHttpClientFactoryOptions(Func<HttpContext, HttpClient> option)
        {
            _option = option;
        }

        public HttpClient Configure(HttpContext context)
        {
            return _option.Invoke(context) ?? new HttpClient();
        }
    }
}
