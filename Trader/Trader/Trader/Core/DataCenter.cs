﻿using EasyHttp.Http;
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
using System.Security.Cryptography.X509Certificates;
using static System.Net.WebRequestMethods;
using EasyHttp.Http.Abstractions;
using EasyHttp.Http.Injection;
using Flurl.Http.Testing;

namespace Trader.Core
{
    internal class DataCenter
    {
        static string loginUrl { get; set; } = string.Empty;
        static string orderUrl { get; set; } = string.Empty;
        static string dealUrl { get; set; } = string.Empty;
        static string baseUrl { get; set; } = string.Empty;
        static string positonSettingUrl { get; set; } = string.Empty;
        static string syncUrl { get; set; } = string.Empty;
        static string groupUrl { get; set; } = string.Empty;
        static string groupID { get; set; } = string.Empty;

        public static LoginResponseInfo GlobalLogin { get; set; }

        static bool IsSSL;
        public static void Init()
        {
            try
            {
                bool ssl = bool.TryParse(ConfigurationManager.AppSettings["isSslEnable"], out IsSSL);
                if (!ssl)
                {
                    Log.Logger.Error("Ssl Enable 配置有误");
                }
                string ip = ConfigurationManager.AppSettings["ip"];
                string port = ConfigurationManager.AppSettings["port"];
                if (IsSSL)
                {
                    baseUrl = string.Format("https://{0}:{1}", ip, port);
                }
                else
                {
                    baseUrl = string.Format("http://{0}:{1}", ip, port);
                }
                loginUrl = string.Format("{0}/sign_in", baseUrl);
                positonSettingUrl = string.Format("{0}/position_settings", baseUrl);
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("读取地址信息出错,{0}", e.Message));
            }
        }
        //static DataCenter()
        //{
        //    try
        //    {
        //        baseUrl = ConfigurationManager.AppSettings["server"];
        //        loginUrl = string.Format("{0}/sign_in", baseUrl);
        //        positonSettingUrl= string.Format("{0}/position_settings", baseUrl);
        //    }
        //}
        private static  void PackageCrt(EasyHttp.Http.HttpClient client)
        {
            X509Certificate2 cert = new X509Certificate2(@".\server.crt");
            client.Request.ClientCertificates.Add(cert);
        }
        private static System.Net.Http.HttpClient GenerateClient( )
        {
            if(IsSSL)
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback = (s, ce, ca, p) =>
                {
                    var certificate = new X509Certificate2(@".\server.crt");
                    if (ce != null)
                    {
                        return certificate.Thumbprint.Equals(ce.Thumbprint, StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                };
                return new System.Net.Http.HttpClient(handler);
            }
            else
            {
                return new System.Net.Http.HttpClient();
            }
        }
        public static async Task<LoginResponse> Login(LoginRequestInfo info)
        {
            try
            {
                var http=new EasyHttp.Http.HttpClient();
                if (IsSSL)
                {
                    PackageCrt(http);
                }
                HttpResponse response = null;
                LoginResponse result = new LoginResponse();
                await Task.Run(() =>
                {
                    try
                    {
                        http.Request.AddExtraHeader("group", "1");
                        response = http.Get(string.Format("{0}/group/{1}", baseUrl, info.UserName));
                       
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(string.Format("登录出错,获取组信息出错,{0}", ex.Message));
                    }
                });
                if (response != null)
                {
                    result.Status = response.StatusCode;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var groupInfo = response.StaticBody<GroupInfo>();
                        groupID = groupInfo.Group;
                    }
                    else
                    {
                        result.Error = response.StaticBody<ErrorInfo>();
                        return result;
                    }
                }
                await Task.Run(() => { 
                    try
                    {
                        var httptemp = new EasyHttp.Http.HttpClient();
                            if (IsSSL)
                             {
                                PackageCrt(httptemp);
                            }
                        httptemp.Request.AddExtraHeader("group", groupID);
                        response = httptemp.Post(loginUrl, info, HttpContentTypes.ApplicationJson);
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
                if(IsSSL)
                {
                    PackageCrt(http);
                }
                http.Request.AddExtraHeader("group", groupID);
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
                if (IsSSL)
                {
                    PackageCrt(http);
                }
                http.Request.AddExtraHeader("group", groupID);
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
                            result.Info[i].deal_price = result.Info[i].average_price * result.Info[i].filled_quantity;
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
                if (IsSSL)
                {
                    PackageCrt(http);
                }
                http.Request.AddExtraHeader("group", groupID);
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
                            result.Info[i].deal_price = result.Info[i].price * result.Info[i].quantity;
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
                var client = GenerateClient();
                client.DefaultRequestHeaders.Add("group", groupID);
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
                if (IsSSL)
                {
                    PackageCrt(http);
                }
                http.Request.AddExtraHeader("group", groupID);
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

        public static async Task<SyncResponse> SetAdapter()
        {
            try
            {
                var http = new EasyHttp.Http.HttpClient();
                if (IsSSL)
                {
                    PackageCrt(http);
                }
                http.Request.AddExtraHeader("group", groupID);
                http.Request.AddExtraHeader("authorization", string.Format("Bearer {0}", GlobalLogin.Token));
                HttpResponse response = null;
                SyncResponse result = new SyncResponse();
                syncUrl = string.Format("{0}/position_settings/{1}/set_adapter", baseUrl, GlobalLogin.UserName);
                await Task.Run(() =>
                {
                    try
                    {
                        response = http.Post(syncUrl, null, HttpContentTypes.ApplicationJson);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(string.Format("同步出错,{0}", ex.Message));
                    }
                });

                if (response != null)
                {
                    result.Status = response.StatusCode;
                    result.Error = response.StaticBody<ErrorInfo>();
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Logger.Error(string.Format("同步出错,{0}", e.Message));
                return null;
            }
        }
    }
}
