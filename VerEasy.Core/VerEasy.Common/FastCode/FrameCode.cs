using RazorEngine;
using RazorEngine.Templating;
using SqlSugar;
using System.Reflection;
using VerEasy.Common.Helper;
using VerEasy.Common.Utils;
using static VerEasy.Common.FastCode.FastCodeExtension;

namespace VerEasy.Common.FastCode
{
    /// <summary>
    /// 快速代码生成
    /// </summary>
    public static class FrameCode
    {
        public static void CreateControllerFile()
        {
        }

        /// <summary>
        /// 根据表结构生成实体
        /// </summary>
        /// <param name="db">DB</param>
        /// <param name="path">生成文件路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="modelNames">指定生成Model集</param>
        public static bool CreateModelsFile(ISqlSugarClient db, string path = null, string nameSpace = null, string[] modelNames = null)
        {
            //模板名称
            var templte = GetTemplate("ModelTemplate");
            try
            {
                //要生成在文件夹下-相对路径
                string relativePath = Path.Combine(path ?? "..\\VerEasy.Core.Models\\ViewModels");
                //真实路径
                string realPath = Path.GetFullPath(relativePath);
                //表信息
                var tables = db.DbMaintenance.GetTableInfoList(false);
                if (modelNames != null)
                {
                    tables = tables.Where(x => modelNames.Contains(x.Name)).ToList();
                }
                var modelFilePath = string.Empty;
                string[] baseModelName = ["Id", "CreateTime", "UpdateTime", "CreateBy", "UpdateBy", "IsDeleted"];
                foreach (var table in tables)
                {
                    modelFilePath = Path.Combine(realPath, $"{table.Name.Replace("T_", "")}.cs");
                    if (!File.Exists(modelFilePath))
                    {
                        //列信息
                        var tableColumnInfos = db.DbMaintenance.GetColumnInfosByTableName(table.Name, false);
                        //razor试图Model
                        var razorModel = new RazorTemplateDataModel
                        {
                            ColumnInfos = tableColumnInfos
                            .Where(x => !baseModelName.Contains(x.DbColumnName))
                            .Select(x =>
                            {
                                x.DataType = MapDbTypeToCSharpType(x.DataType);
                                return x;
                            }).ToList(),
                            DbTableName = table.Name,
                            ClassName = table.Name.Replace("T_", ""),
                            NameSpace = nameSpace ?? "VerEasy.Core.Models.ViewModels",
                            UseBaseModel = Appsettings.App("Seq:WriteToDataBaseTable") != table.Name
                        };
                        string result = Engine.Razor.RunCompile(templte, "EntityClassTemplate", typeof(RazorTemplateDataModel), razorModel);
                        File.WriteAllText(modelFilePath, result);
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        /// <summary>
        /// 根据实体生成表结构
        /// </summary>
        /// <param name="db"></param>
        /// <param name="modelName">model层名称</param>
        /// <param name="filterByNameSpace">根据命名空间过滤</param>
        /// <returns></returns>
        public static bool CreateDataTable(ISqlSugarClient db, string modelName = null, string filterByNameSpace = null)
        {
            try
            {
                //根目录
                var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                //model层程序集
                var assembly = Assembly.LoadFrom(Path.Combine(basePath, $"{modelName ?? "VerEasy.Core.Models"}.dll"));
                var types = assembly.GetTypes().Where(x => x.FullName.Contains($"{filterByNameSpace ?? "ViewModels"}")).ToArray();
                var tables = db.DbMaintenance.GetTableInfoList(false);
                foreach (var table in tables)
                {
                    var tableName = table.Name.Replace("T_", "");
                    types = types.Where(x => x.Name != tableName).ToArray();
                }

                db.CodeFirst.SetStringDefaultLength(200).InitTables(types);
            }
            catch (Exception)
            {
                return false;
                throw;
            }

            return true;
        }

        /// <summary>
        /// 根据表结构生成IRepository
        /// </summary>
        /// <param name="db"></param>
        /// <param name="modelName">model层名称</param>
        /// <param name="filterByNameSpace">根据命名空间过滤</param>
        /// <returns></returns>
        public static bool CreateIRepositoryByModel(ISqlSugarClient db, string path = null, string nameSpace = null, string[] modelNames = null)
        {
            //模板名称
            var templte = GetTemplate("IRepositoryTemplate");
            try
            {
                //要生成在文件夹下-相对路径
                string relativePath = Path.Combine(path ?? "..\\VerEasy.Core.IRepository\\IRepository");
                //真实路径
                string realPath = Path.GetFullPath(relativePath);
                //表信息
                var tables = db.DbMaintenance.GetTableInfoList(false);
                if (modelNames != null)
                {
                    tables = tables.Where(x => modelNames.Contains(x.Name)).ToList();
                }
                var modelFilePath = string.Empty;
                foreach (var table in tables)
                {
                    modelFilePath = Path.Combine(realPath, $"I{table.Name.Replace("T_", "")}Repository.cs");
                    if (!File.Exists(modelFilePath))
                    {
                        var razorModel = new RazorTemplateDataModel
                        {
                            DbTableName = table.Name,
                            ClassName = table.Name.Replace("T_", ""),
                            NameSpace = nameSpace ?? "VerEasy.Core.IRepository.IRepository"
                        };
                        string result = Engine.Razor.RunCompile(templte, Guid.NewGuid().ToString(), typeof(RazorTemplateDataModel), razorModel);
                        File.WriteAllText(modelFilePath, result);
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        /// <summary>
        /// 根据表结构生成Repository
        /// </summary>
        /// <param name="db"></param>
        /// <param name="path"></param>
        /// <param name="nameSpace"></param>
        /// <param name="modelNames"></param>
        /// <returns></returns>
        public static bool CreateRepositoryByModel(ISqlSugarClient db, string path = null, string nameSpace = null, string[] modelNames = null)
        {
            //模板名称
            var templte = GetTemplate("RepositoryTemplate");
            try
            {
                //要生成在文件夹下-相对路径
                string relativePath = Path.Combine(path ?? "..\\VerEasy.Core.Repository\\Repository");
                //真实路径
                string realPath = Path.GetFullPath(relativePath);
                //表信息
                var tables = db.DbMaintenance.GetTableInfoList(false);
                if (modelNames != null)
                {
                    tables = tables.Where(x => modelNames.Contains(x.Name)).ToList();
                }
                var modelFilePath = string.Empty;
                foreach (var table in tables)
                {
                    modelFilePath = Path.Combine(realPath, $"{table.Name.Replace("T_", "")}Repository.cs");
                    if (!File.Exists(modelFilePath))
                    {
                        var razorModel = new RazorTemplateDataModel
                        {
                            DbTableName = table.Name,
                            ClassName = table.Name.Replace("T_", ""),
                            NameSpace = nameSpace ?? "VerEasy.Core.IRepository.IRepository"
                        };
                        string result = Engine.Razor.RunCompile(templte, Guid.NewGuid().ToString(), typeof(RazorTemplateDataModel), razorModel);
                        File.WriteAllText(modelFilePath, result);
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        /// <summary>
        /// 根据表结构生成IService
        /// </summary>
        /// <param name="db"></param>
        /// <param name="path"></param>
        /// <param name="nameSpace"></param>
        /// <param name="modelNames"></param>
        /// <returns></returns>
        public static bool CreateIServiceByModel(ISqlSugarClient db, string path = null, string nameSpace = null, string[] modelNames = null)
        {
            //模板名称
            var templte = GetTemplate("IServiceTemplate");
            try
            {
                //要生成在文件夹下-相对路径
                string relativePath = Path.Combine(path ?? "..\\VerEasy.Core.IService\\IService");
                //真实路径
                string realPath = Path.GetFullPath(relativePath);
                //表信息
                var tables = db.DbMaintenance.GetTableInfoList(false);
                if (modelNames != null)
                {
                    tables = tables.Where(x => modelNames.Contains(x.Name)).ToList();
                }
                var modelFilePath = string.Empty;
                foreach (var table in tables)
                {
                    modelFilePath = Path.Combine(realPath, $"I{table.Name.Replace("T_", "")}Service.cs");
                    if (!File.Exists(modelFilePath))
                    {
                        var razorModel = new RazorTemplateDataModel
                        {
                            DbTableName = table.Name,
                            ClassName = table.Name.Replace("T_", ""),
                            NameSpace = nameSpace ?? "VerEasy.Core.IService.IService"
                        };
                        string result = Engine.Razor.RunCompile(templte, Guid.NewGuid().ToString(), typeof(RazorTemplateDataModel), razorModel);
                        File.WriteAllText(modelFilePath, result);
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        /// <summary>
        /// 根据表结构生成Service
        /// </summary>
        /// <param name="db"></param>
        /// <param name="path"></param>
        /// <param name="nameSpace"></param>
        /// <param name="modelNames"></param>
        /// <returns></returns>
        public static bool CreateServiceByModel(ISqlSugarClient db, string path = null, string nameSpace = null, string[] modelNames = null)
        {
            //模板名称
            var templte = GetTemplate("ServiceTemplate");
            try
            {
                //要生成在文件夹下-相对路径
                string relativePath = Path.Combine(path ?? "..\\VerEasy.Core.Service\\Service");
                //真实路径
                string realPath = Path.GetFullPath(relativePath);
                //表信息
                var tables = db.DbMaintenance.GetTableInfoList(false);
                if (modelNames != null)
                {
                    tables = tables.Where(x => modelNames.Contains(x.Name)).ToList();
                }
                var modelFilePath = string.Empty;
                foreach (var table in tables)
                {
                    modelFilePath = Path.Combine(realPath, $"{table.Name.Replace("T_", "")}Service.cs");
                    if (!File.Exists(modelFilePath))
                    {
                        var razorModel = new RazorTemplateDataModel
                        {
                            DbTableName = table.Name,
                            ClassName = table.Name.Replace("T_", ""),
                            NameSpace = nameSpace ?? "VerEasy.Core.Service.Service"
                        };
                        string result = Engine.Razor.RunCompile(templte, Guid.NewGuid().ToString(), typeof(RazorTemplateDataModel), razorModel);
                        File.WriteAllText(modelFilePath, result);
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        /// <summary>
        /// 根据表结构生成Controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="path"></param>
        /// <param name="nameSpace"></param>
        /// <param name="modelNames"></param>
        /// <returns></returns>
        public static bool CreateControllerByModel(ISqlSugarClient db, string path = null, string nameSpace = null, string[] modelNames = null)
        {
            //模板名称
            var templte = GetTemplate("ControllerTemplate");
            try
            {
                //要生成在文件夹下-相对路径
                string relativePath = Path.Combine(path ?? "..\\VerEasy.Core.Api\\Controllers");
                //真实路径
                string realPath = Path.GetFullPath(relativePath);
                //表信息
                var tables = db.DbMaintenance.GetTableInfoList(false);
                if (modelNames != null)
                {
                    tables = tables.Where(x => modelNames.Contains(x.Name)).ToList();
                }
                var modelFilePath = string.Empty;
                foreach (var table in tables)
                {
                    modelFilePath = Path.Combine(realPath, $"{table.Name.Replace("T_", "")}Controller.cs");
                    if (!File.Exists(modelFilePath))
                    {
                        var razorModel = new RazorTemplateDataModel
                        {
                            DbTableName = table.Name,
                            ClassName = table.Name.Replace("T_", ""),
                            NameSpace = nameSpace ?? "VerEasy.Core.Api.Controllers"
                        };
                        string result = Engine.Razor.RunCompile(templte, Guid.NewGuid().ToString(), typeof(RazorTemplateDataModel), razorModel);
                        File.WriteAllText(modelFilePath, result);
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        /// <summary>
        /// 根据model生成项目文件
        /// </summary>
        /// <param name="db"></param>
        /// <param name="path">生成文件的相对路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="modelNames">要生成的模型名称列表</param>
        /// <param name="templateName">模板名称</param>
        /// <param name="filePrefix">类型前缀</param>
        /// <returns></returns>
        public static bool CreateFilesByModels(ISqlSugarClient db, FastCodeConfig fastCodeConfig = null)
        {
            // 如果没有指定模板类型，则生成所有类型的模板
            var templateTypes = fastCodeConfig == null
                ? Enum.GetValues(typeof(FileTemplateType)).Cast<FileTemplateType>().ToList()
                : [fastCodeConfig.FileTemplateType];

            try
            {
                foreach (var templateType in templateTypes)
                {
                    //获取Razor模板
                    var templateInfo = templateInfos[templateType];
                    var template = GetTemplate(templateInfo.TemplateName);
                    var namePace = templateTypes.Count > 1 ? templateInfo.NameSpace : fastCodeConfig.NameSpace;
                    // 获取文件的真实路径
                    string relativePath = Path.Combine(fastCodeConfig?.Path ?? $"..\\{templateInfo.ProjectName}", templateInfo.FolderName);
                    string realPath = Path.GetFullPath(relativePath);
                    // 确保目标目录存在
                    if (!Directory.Exists(realPath))
                    {
                        Directory.CreateDirectory(realPath);
                    }

                    // 获取数据库表信息
                    var tables = db.DbMaintenance.GetTableInfoList(false);
                    // 如果指定了模型名称，进行筛选
                    if (fastCodeConfig?.ModelNames != null)
                    {
                        tables = tables.Where(x => fastCodeConfig.ModelNames.Contains(x.Name)).ToList();
                    }
                    // 遍历每一张表，生成相应的文件
                    foreach (var table in tables)
                    {
                        // 判断文件名是否需要加上 "I"，即针对 IRepository 和 IService 需要加上 "I"
                        string className = table.Name.Replace("T_", "");
                        string fileName = $"{className}{templateInfo.Txt}.cs";

                        // 如果模板是 IRepository 或 IService 类型，则文件名前加上 "I"
                        if (templateInfo.SuffixKey.Contains('I'))
                        {
                            fileName = $"I{fileName}";
                        }
                        var modelFilePath = Path.Combine(realPath, fileName);

                        // 如果文件不存在，或者文件内容已经更改，生成新的文件
                        if (!File.Exists(modelFilePath))
                        {
                            // 创建 Razor 模板数据模型
                            var razorModel = new RazorTemplateDataModel
                            {
                                DbTableName = table.Name,
                                ClassName = table.Name.Replace("T_", ""),
                                NameSpace = namePace
                            };

                            // 使用 Razor 模板生成文件内容
                            string result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), typeof(RazorTemplateDataModel), razorModel);

                            // 将生成的内容写入文件
                            File.WriteAllText(modelFilePath, result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 模板信息字典
        /// </summary>
        private static readonly Dictionary<FileTemplateType, TemplateInfo> templateInfos = new()
        {
            { FileTemplateType.IRepository, new TemplateInfo{ Txt="Repository",SuffixKey = "IRepository", NameSpace="VerEasy.Core.IRepository.IRepository",TemplateName = "IRepositoryTemplate", FolderName = "IRepository" ,ProjectName="VerEasy.Core.IRepository"} },
            { FileTemplateType.IService, new TemplateInfo{ Txt="Service",SuffixKey = "IService", NameSpace="VerEasy.Core.IService.IService", TemplateName = "IServiceTemplate", FolderName = "IService",ProjectName="VerEasy.Core.IService" } },
            { FileTemplateType.Controller, new TemplateInfo{Txt="Controllers", SuffixKey = "Controllers", NameSpace="VerEasy.Core.Api.Controllers", TemplateName = "ControllerTemplate", FolderName = "Controllers",ProjectName="VerEasy.Core.Api" } },
            { FileTemplateType.Service, new TemplateInfo{ Txt="Service",SuffixKey = "Service", NameSpace="VerEasy.Core.Service.Service", TemplateName = "ServiceTemplate", FolderName = "Service",ProjectName="VerEasy.Core.Service" } },
            { FileTemplateType.Repository, new TemplateInfo{ Txt="Repository",SuffixKey = "Repository", NameSpace="VerEasy.Core.IRepository.IRepository", TemplateName = "RepositoryTemplate", FolderName = "Repository",ProjectName="VerEasy.Core.Repository" } }
        };

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns></returns>
        private static string GetTemplate(string templateName)
        {
            if (!string.IsNullOrEmpty(templateName))
            {
                // 获取当前执行程序的程序集
                var assembly = Assembly.GetExecutingAssembly();

                // 获取嵌入资源的名称，通常是项目命名空间 + 文件名
                var resourceName = $"VerEasy.Common.FastCode.Template.{templateName}.cshtml";

                // 读取嵌入资源流
                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成实体时根据数据库类型转换为C#类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string MapDbTypeToCSharpType(string dbType)
        {
            return dbType.ToUpper() switch
            {
                // 字符串类型，MySQL 和 SQL Server 都适用
                "VARCHAR" or "CHAR" or "TEXT" or "LONGTEXT" or "TINYTEXT" or "MEDIUMTEXT" or "LONGTEXT" => "string",
                "NVARCHAR" or "NCHAR" => "string",  // SQL Server 中的字符串类型
                                                    // 整数类型，适用于 SQL Server 和 MySQL
                "INT" or "INTEGER" or "SMALLINT" or "MEDIUMINT" => "int",
                // 大整数类型
                "BIGINT" => "long",
                // 浮动类型，适用于 SQL Server 和 MySQL，通常使用 decimal 来避免精度丢失
                "DECIMAL" or "NUMERIC" or "FLOAT" or "DOUBLE" or "REAL" => "decimal",
                // 日期时间类型，适用于 SQL Server 和 MySQL
                "DATE" or "DATETIME" or "DATETIME2" or "TIMESTAMP" => "DateTime",
                // 仅 MySQL 中的时间类型，映射为 TimeSpan
                "TIME" => "TimeSpan",
                // 布尔类型，SQL Server 和 MySQL 中都为 BIT 类型
                "BIT" => "bool",
                // 二进制数据类型，适用于 SQL Server 和 MySQL
                "BLOB" or "VARBINARY" => "byte[]",
                // SQL Server 中的 GUID 类型
                "GUID" or "UNIQUEIDENTIFIER" => "Guid",
                // MySQL 中的小整数类型
                "TINYINT" => "byte",
                // MySQL 中的二进制大对象类型（大尺寸的 BLOB 数据）
                "MEDIUMBLOB" or "LONGBLOB" => "byte[]",
                // MySQL 中的枚举类型，映射为 string
                "ENUM" => "string",
                // MySQL 中的 JSON 类型，映射为 string（如果需要可以进一步处理）
                "JSON" => "string",
                // 默认情况，无法识别的类型映射为 object
                _ => "object",
            };
        }
    }
}