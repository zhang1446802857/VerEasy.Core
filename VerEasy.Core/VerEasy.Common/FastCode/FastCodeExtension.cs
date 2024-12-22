using SqlSugar;

namespace VerEasy.Common.FastCode
{
    public class FastCodeExtension
    {
        /// <summary>
        /// Razor模板传参实体
        /// </summary>
        public class RazorTemplateDataModel
        {
            /// <summary>
            /// 使用基类
            /// </summary>
            public bool UseBaseModel {  get; set; }
            /// <summary>
            /// 列字段
            /// </summary>
            public List<DbColumnInfo> ColumnInfos { get; set; }

            /// <summary>
            /// 类名
            /// </summary>
            public string ClassName { get; set; }

            /// <summary>
            /// 数据库名
            /// </summary>
            public string DbTableName { get; set; }

            /// <summary>
            /// 命名空间
            /// </summary>
            public string NameSpace { get; set; }
        }

        public class FastCodeConfig
        {
            public string NameSpace { get; set; }
            public string Path { get; set; }
            public string[] ModelNames { get; set; }
            public FileTemplateType FileTemplateType { get; set; }
            public bool FastAuto { get; set; }
        }

        public class TemplateInfo
        {
            public string TemplateName { get; set; }
            public string ProjectName { get; set; }
            public string SuffixKey { get; set; }
            public string FolderName { get; set; }
            public string NameSpace { get; set; }
            public string Txt { get; set; }
        }

        public enum FileTemplateType
        {
            IRepository = 0,
            Repository = 1,
            IService = 2,
            Service = 3,
            Controller = 4
        }
    }
}