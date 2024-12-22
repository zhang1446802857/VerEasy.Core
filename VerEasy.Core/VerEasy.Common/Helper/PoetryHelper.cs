using RestSharp;

namespace VerEasy.Common.Helper
{
    public static class poetryHelper
    {
        private static readonly string apiUrl = "https://v1.hitokoto.cn?encode=text"; // Hitokoto API 地址

        public static string Hstring()
        {
            //创建一个RestSharp请求实例，并且设置Url。
            RestClient restClient = new(apiUrl);
            RestRequest restRequest = new RestRequest("", Method.Post);
            //发送请求(还有几种请求的方式，例如restClient.Post(restRequest)，个人觉得Execute和ExecuteAsny就可以了，请求方式上面已经设置过了)
            //response就是请求结果，response.Count返回内容，response.Code 请求状态
            var response = restClient.Execute(restRequest);
            return string.IsNullOrEmpty(response.Content) ? "我即是太阳" : response.Content;
        }

        public class Rootobject
        {
            public int id { get; set; }
            public string uuid { get; set; }
            public string hitokoto { get; set; }
            public string type { get; set; }
            public string from { get; set; }
            public object from_who { get; set; }
            public string creator { get; set; }
            public int creator_uid { get; set; }
            public int reviewer { get; set; }
            public string commit_from { get; set; }
            public string created_at { get; set; }
            public int length { get; set; }
        }
    }
}