using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LumTomofunCustomization.API_Helper
{
    public class LUMAPIHelper
    {
        public static string SerialzeJSONString<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeJSONString<T>(string json) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonSerializationException)
            {
                return null;
            }
        }

        protected ILUMAPIConfig _config;
        protected Dictionary<string, string> _formDataDic;

        public LUMAPIHelper(ILUMAPIConfig config, Dictionary<string, string> formDataDic)
        {
            _config = config;
            _formDataDic = formDataDic;
        }

        public LUMAPIResults GetResults()
        {
            using (HttpClient client = new HttpClient())
            {
                // Accept 用於宣告客戶端要求服務端回應的文件型態
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                if (!string.IsNullOrEmpty(_config.OrgName))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("org_name", _config.OrgName);
                }

                if (!string.IsNullOrEmpty(_config.AuthType))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_config.AuthType, _config.Token);
                }

                // 強型別用法 https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/nameof
                //Dictionary<string, string> formDataDic = new Dictionary<string, string>()
                //{
                //    {"Token", config.Token }
                //};

                // https://msdn.microsoft.com/zh-tw/library/system.net.http.formurlencodedcontent(v=vs.110).aspx
                var formData = new FormUrlEncodedContent(_formDataDic);

                HttpResponseMessage response = _config.RequestMethod == HttpMethod.Post ? client.PostAsync(_config.RequestUrl, formData).GetAwaiter().GetResult() : client.GetAsync(_config.RequestUrl).GetAwaiter().GetResult();

                // Return Result
                return new LUMAPIResults()
                {
                    StatusCode    = response.StatusCode,
                    Content       = response.Content,
                    ContentResult = response.Content.ReadAsStringAsync().Result
                };
            }
        }

        public LUMAPIResults GetResults(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = null;

                if (_config.RequestMethod == HttpMethod.Get)
                {
                    foreach (var key in _formDataDic.Keys)
                    {
                        _formDataDic.TryGetValue(key, out string value);

                        // Content-Type 用於宣告遞送給對方的文件型態
                        client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_config.AuthType, _config.Token);

                    response = client.GetAsync($"{_config.RequestUrl}{request}").GetAwaiter().GetResult();
                }
                else
                {
                    var content = new StringContent(request, System.Text.Encoding.UTF8, "application/json");

                    response = client.PostAsync(_config.RequestUrl, content).GetAwaiter().GetResult();
                }

                return new LUMAPIResults()
                {
                    StatusCode    = response.StatusCode,
                    Content       = response.Content,
                    ContentResult = response.Content.ReadAsStringAsync().Result
                };
            }
        }
    }
}
