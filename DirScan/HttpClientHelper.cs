using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DirScan
{
    public class HttpClientHelper
    {


        private static HttpClientHelper httpClientHelper = null;

        private HttpClient httpClient;

        /// <summary>
        /// 构造方法私有，用于单例
        /// </summary>
        private HttpClientHelper() { }

        /// <summary>
        /// 获取当前类的实例
        /// </summary>
        /// <returns></returns>
        public static HttpClientHelper GetInstance()
        {
            if (httpClientHelper != null)
            {
                return httpClientHelper;
            }
            else
            {
                HttpClientHelper httpClientHelper = new HttpClientHelper();

                //取消使用默认的Cookies
                HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
                httpClientHelper.httpClient = new HttpClient(handler);
                return httpClientHelper;
            }
        }


        public HttpResponseMessage Head(string url, List<KeyValuePair<string, string>> headers = null) {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Head,
            };

            if (headers != null && headers.Count > 0)
            {
                request.Headers.Clear();

                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);

                }
            }
            HttpResponseMessage response = httpClient.SendAsync(request).Result;
            return response;
        }

        /// <summary>
        /// Get方法请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public HttpResponseMessage Get(string url, List<KeyValuePair<string, string>> headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            if (headers != null && headers.Count > 0)
            {
                request.Headers.Clear();

                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);

                }
            }
            HttpResponseMessage response = httpClient.SendAsync(request).Result;
            return response;
        }

        /// <summary>
        /// Get方法请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string url, List<KeyValuePair<string, string>> headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            if (headers != null && headers.Count > 0)
            {
                request.Headers.Clear();
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            return await httpClient.SendAsync(request);
        }

        /// <summary>
        /// Post方法请求 application/x-www-form-urlencoded格式
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="paramList">参数集合</param>
        /// <returns></returns>
        public HttpResponseMessage Post(string url, List<KeyValuePair<String, String>> paramList, List<KeyValuePair<string, string>> headers = null)
        {
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(paramList);
            if (headers != null && headers.Count > 0)
            {
                formUrlEncodedContent.Headers.Clear();
                foreach (var header in headers)
                {
                    formUrlEncodedContent.Headers.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = httpClient.PostAsync(new Uri(url), formUrlEncodedContent).Result;
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, List<KeyValuePair<String, String>> paramList, List<KeyValuePair<string, string>> headers = null)
        {
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(paramList);
            if (headers != null && headers.Count > 0)
            {
                formUrlEncodedContent.Headers.Clear();
                foreach (var header in headers)
                {
                    formUrlEncodedContent.Headers.Add(header.Key, header.Value);
                }
            }
            return await httpClient.PostAsync(new Uri(url), formUrlEncodedContent);
        }

        /// <summary>
        /// Post方法请求 raw data
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">raw data</param>
        /// <returns></returns>
        public HttpResponseMessage Post(string url, string content, List<KeyValuePair<string, string>> headers = null)
        {
            StringContent stringContent = new StringContent(content, Encoding.UTF8);
            if (headers != null && headers.Count > 0)
            {
                stringContent.Headers.Clear();
                foreach (var header in headers)
                {
                    stringContent.Headers.Add(header.Key, header.Value);
                }
            }

            HttpResponseMessage response = httpClient.PostAsync(new Uri(url), stringContent).Result;
            return response;
        }

        /// <summary>
        /// Post方法请求 raw data
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">raw data</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string url, string content, List<KeyValuePair<string, string>> headers = null)
        {
            StringContent stringContent = new StringContent(content, Encoding.UTF8);
            if (headers != null && headers.Count > 0)
            {
                stringContent.Headers.Clear();
                foreach (var header in headers)
                {
                    stringContent.Headers.Add(header.Key, header.Value);
                }
            }
            return await httpClient.PostAsync(new Uri(url), stringContent);

        }

        /// <summary>
        /// 释放httpclient
        /// </summary>
        public void Release()
        {
            httpClient.Dispose();
        }



        /// <summary>
        /// 设置默认请求头
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetDefaultHeaders(string name, string value)
        {
            httpClient.DefaultRequestHeaders.Add(name, value);
        }

        /// <summary>
        /// 删除默认请求头
        /// </summary>
        /// <param name="name"></param>
        public void RemoveDefaultHeaders(string name)
        {
            httpClient.DefaultRequestHeaders.Remove(name);
        }
    }
}
