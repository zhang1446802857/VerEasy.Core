using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace VerEasy.Common.Helper
{
    /// <summary>
    /// appsettings读取类
    /// </summary>
    public class Appsettings
    {
        /// <summary>
        /// 读取AppSetting用[Microsoft.Extensions.Configuration.Json]
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 依赖注入,初始化访问配置文件的IConfiguration
        /// </summary>
        /// <param name="contentPath"></param>
        public Appsettings(string contentPath)
        {
            string PATH = "appsettings.json";
            Configuration = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                .Add(new JsonConfigurationSource
                {
                    Path = PATH,
                    Optional = false,
                    ReloadOnChange = true
                }).Build();
        }

        /// <summary>
        /// 手动注入访问配置文件的IConfiguration
        /// </summary>
        /// <param name="configuration"></param>
        public Appsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 获取配置文件的string内容
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static string App(params string[] paras)
        {
            if (paras.Length != 0)
            {
                return Configuration[string.Join(":", paras)];
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据实体解析配置文件的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static T App<T>(params string[] paras)
        {
            return Configuration.GetSection(string.Join(":", paras)).Get<T>();
        }

        /// <summary>
        /// 根据实体解析配置文件的内容集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static List<T> Apps<T>(params string[] paras)
        {
            return Configuration.GetSection(string.Join(":", paras)).Get<List<T>>();
        }

        /// <summary>
        /// 解析配置文件下键值集合
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> Apps(params string[] paras)
        {
            return Configuration.GetSection(string.Join(":", paras))
                .GetChildren().Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList();
        }
    }
}