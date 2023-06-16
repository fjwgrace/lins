using EasyHttp.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using Trader.Models;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Trader.Core
{
    internal class DataCenter
    {
        static string loginUrl { get; set; } = string.Empty;
        static string orderUrl { get; set; } = string.Empty;
        static string dealUrl { get; set; } = string.Empty;
        static string baseUrl { get; set; } = string.Empty;
        static string positonSettingUrl { get; set; } = string.Empty;

        public static LoginResponseInfo GlobalLogin { get; set; }

        static DataCenter()
        {
            try
            {
                baseUrl = ConfigurationManager.AppSettings["server"];
                loginUrl = string.Format("{0}/sign_in", baseUrl);
                positonSettingUrl= string.Format("{0}/position_settings", baseUrl);
            }
            catch(Exception e)
            {
                Log.Logger.Error(string.Format("读取地址信息出错,{0}", e.Message));
            }
        }
        public static async Task<LoginResponse> Login(LoginRequestInfo info)
        {
            try
            {
                var http=new EasyHttp.Http.HttpClient();
                HttpResponse response = null;
                LoginResponse result = new LoginResponse();
                await Task.Run(() =>
                {
                    try
                    {
                        response = http.Post(loginUrl, info, HttpContentTypes.ApplicationJson);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(string.Format("登录出错,{0}", ex.Message));
                    }
                });
                if (response != null)
                {
                    result.Status = response.StatusCode;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result.Info = response.StaticBody<LoginResponseInfo>();
                        GlobalLogin = result.Info;
                    }
                    else
                    {
                        result.Error = response.StaticBody<ErrorInfo>();
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("登录出错,{0}", e.Message));
                return null;
            }
        }
        /// <summary>
        /// 获取持仓列表
        /// </summary>
        /// <param name="fundAccountId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<PositionResponse> GetPositions(string fundAccountId,string username)
        {
            try
            {
                CheckToken();
                string url = string.Format("{0}/stock/{1}/{2}/positions", baseUrl, fundAccountId, username);
                var http = new EasyHttp.Http.HttpClient();
                
                http.Request.AddExtraHeader("authorization", string.Format("Bearer {0}", GlobalLogin.Token));
                HttpResponse response = null;
                PositionResponse result = new PositionResponse();
                await Task.Run(() =>
                {
                    try
                    {
                        response = http.Get(url);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(string.Format(" http.Get 获取持仓出错,{0}", ex.Message));
                    }
              
                });
                if (response != null)
                {
                    result.Status = response.StatusCode;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result.Info = response.StaticBody<List<Position>>();
                        for (int i = 0; i < result.Info.Count; i++)
                        {
                            result.Info[i].index = (i + 1).ToString();
                        }
                    }
                    else
                    {
                        result.Error = response.StaticBody<ErrorInfo>();
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("获取持仓出错,{0}", e.Message));
                return null;
            }
        }
        /// <summary>
        /// 获取委托列表
        /// </summary>
        /// <param name="fundAccountId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<OrderResponse> GetOrders(string fundAccountId, string username)
        {
            try
            {
                CheckToken();
                string url = string.Format("{0}/stock/{1}/{2}/orders", baseUrl, fundAccountId, username);
                var http = new EasyHttp.Http.HttpClient();
                http.Request.AddExtraHeader("authorization", string.Format("Bearer {0}", GlobalLogin.Token));
                HttpResponse response = null;
                OrderResponse result = new OrderResponse();
                await Task.Run(() =>
                {
                    try
                    {
                        response = http.Get(url);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(string.Format("http.Get 获取委托出错,{0}", ex.Message));
                    }
                });
                if (response != null)
                {
                    result.Status = response.StatusCode;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result.Info = response.StaticBody<List<Order>>();
                        for (int i = 0; i < result.Info.Count; i++)
                        {
                            result.Info[i].index = i + 1;
                        }
                    }
                    else
                    {
                        result.Error = response.StaticBody<ErrorInfo>();
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("获取委托出错,{0}", e.Message));
                return null;
            }
        }
        /// <summary>
        /// 获取成交列表
        /// </summary>
        /// <param name="fundAccountId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<DealResponse> GetDeals(string fundAccountId, string username)
        {
            try
            {
                CheckToken();
                string url = string.Format("{0}/stock/{1}/{2}/matches", baseUrl, fundAccountId, username);
                var http = new EasyHttp.Http.HttpClient();
                http.Request.AddExtraHeader("authorization", string.Format("Bearer {0}", GlobalLogin.Token));
                HttpResponse response = null;
                DealResponse result = new DealResponse();
                await Task.Run(() =>
                {
                    try
                    {
                        response = http.Get(url);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(string.Format("http.Get 获取成交出错,{0}", ex.Message));
                    }
                });
                if (response != null)
                {
                    result.Status = response.StatusCode;
                    if (response?.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result.Info = response.StaticBody<List<Deal>>();
                        for (int i = 0; i < result.Info.Count; i++)
                        {
                            result.Info[i].index = i + 1;
                        }
                    }
                    else
                    {
                        result.Error = response.StaticBody<ErrorInfo>();
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("获取成交出错,{0}", e.Message));
                return null;
            }
        }

        private static void CheckToken( )
        {
            if (string.IsNullOrEmpty(GlobalLogin?.Token))
            {
                throw new Exception("非法请求，token为空");
            }
        }

        public static async Task<PositionSettingResponse> UpdatePositionSetting(PositionSetting data,string operate)
        {
            try
            {
                PositionSettingResponse result = new PositionSettingResponse();
                HttpRequestMessage hrm = null;
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalLogin.Token);
                if (operate == "POST")
                {
                    hrm = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, positonSettingUrl);
                }
                else if (operate == "PUT")
                {
                    hrm = new HttpRequestMessage(System.Net.Http.HttpMethod.Put, positonSettingUrl);
                }
                else if (operate == "DELETE")
                {
                    hrm = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, positonSettingUrl);
                }
                hrm.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var rps = await client.SendAsync(hrm);
                var rsp = await rps.Content.ReadAsStringAsync();
                result.Status = rps.StatusCode;
                result.Error = JsonConvert.DeserializeObject<ErrorInfo>(rsp);

                return result;
            }
            catch(AggregateException ex)
            {
                Log.Logger.Error(string.Format("操作符{0},分券出错,{1}", operate, ex.Message));
                return null;
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("操作符{0},分券出错,{1}", operate,e.Message));
                return null;
            }
        }

        public static async Task<PositionSettingResponse> GetPositionSetting( )
        {
            try
            {
                var http = new EasyHttp.Http.HttpClient();
                http.Request.AddExtraHeader("authorization", string.Format("Bearer {0}", GlobalLogin.Token));
                HttpResponse response = null;
                PositionSettingResponse result = new PositionSettingResponse();
                await Task.Run(() =>
                {
                    try
                    {
                        response = http.Get(positonSettingUrl);
                    }
                    catch(Exception ex)
                    {
                        Log.Logger.Error(string.Format("获取分券信息出错,{0}", ex.Message));
                    }
                });
                if (response != null)
                {
                    result.Status = response.StatusCode;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result.Info = response.StaticBody<List<PositionSetting>>();
                        for (int i = 0; i < result.Info.Count; i++)
                        {
                            result.Info[i].index = i + 1;
                        }
                    }
                    else
                    {
                        result.Error = response.StaticBody<ErrorInfo>();
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("获取分券信息出错,{0}", e.Message));
                return null;
            }
        }
    }
}
