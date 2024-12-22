using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Reflection;
using VerEasy.Common.Helper;
using VerEasy.Extensions.ServiceExtensions.HttpContext;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// sqlsugar注入相关
    /// </summary>
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            services.AddSingleton<ISqlSugarClient>(x =>
            {
                var user = x.GetService<IUser>();
                var databaseSettings = Appsettings.App<DatabaseSettings>("DatabaseSettings");
                SqlSugarClient sqlSugar;
                //多库操作
                if (databaseSettings.EnableMultiple)
                {
                    var enableDbConfigs = databaseSettings.Db.Where(x => x.Enable)
                    .Select(x => new ConnectionConfig
                    {
                        ConfigId = x.Name, // 数据库名称标识
                        DbType = ParseDbType(x.DbTypeName), // 数据库类型
                        IsAutoCloseConnection = true, // 自动关闭连接
                        ConnectionString = x.ConnectionString, // 连接字符串
                        ConfigureExternalServices = new ConfigureExternalServices
                        {
                            EntityService = (c, p) =>
                            {
                                /***高版C#写法***/
                                //支持string?和string
                                if (p.IsPrimarykey == false && new NullabilityInfoContext()
                                 .Create(c).WriteState is NullabilityState.Nullable)
                                {
                                    p.IsNullable = true;
                                }
                            }
                        }
                    }).ToList();
                    sqlSugar = new SqlSugarClient(enableDbConfigs);
                }
                else
                {
                    // 单库配置，加载默认的数据库
                    var defaultDb = databaseSettings.Db.FirstOrDefault(x => x.Name == databaseSettings.DefaultDb);
                    sqlSugar = new(new ConnectionConfig
                    {
                        ConfigId = defaultDb.Name, // 数据库名称标识
                        DbType = ParseDbType(defaultDb.DbTypeName),
                        IsAutoCloseConnection = true,
                        ConnectionString = defaultDb.ConnectionString,
                        ConfigureExternalServices = new ConfigureExternalServices
                        {
                            EntityService = (c, p) =>
                            {
                                /***高版C#写法***/
                                //支持string?和string
                                if (p.IsPrimarykey == false && new NullabilityInfoContext()
                                 .Create(c).WriteState is NullabilityState.Nullable)
                                {
                                    p.IsNullable = true;
                                }
                            }
                        }
                    });
                }

                sqlSugar.Aop.DataExecuting = (oldValue, entityInfo) =>
                {
                    #region 新增

                    if (entityInfo.PropertyName == "CreateBy" && entityInfo.OperationType == DataFilterType.InsertByObject)
                    {
                        entityInfo.SetValue(user.Id);
                    }
                    if (entityInfo.PropertyName == "UpdateBy" && entityInfo.OperationType == DataFilterType.InsertByObject)
                    {
                        entityInfo.SetValue(user.Id);
                    }

                    #endregion 新增

                    #region 更改

                    if (entityInfo.PropertyName == "UpdateBy" && entityInfo.OperationType == DataFilterType.UpdateByObject)
                    {
                        entityInfo.SetValue(user.Id);
                    }

                    #endregion 更改
                };

                return sqlSugar;
            });
        }

        /// <summary>
        /// 转换数据库类型
        /// </summary>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private static DbType ParseDbType(string dbTypeName)
        {
            return dbTypeName.ToLower() switch
            {
                "sqlserver" => DbType.SqlServer,
                "mysql" => DbType.MySql,
                "oracle" => DbType.Oracle,
                "sqlite" => DbType.Sqlite,
                _ => throw new NotSupportedException($"不支持的数据库类型: {dbTypeName}")
            };
        }

        #region 配置类

        public class DatabaseSettings
        {
            public bool EnableMultiple { get; set; } // 是否启用多库操作
            public string DefaultDb { get; set; } // 默认数据库名称
            public List<DbConfig> Db { get; set; } // 数据库列表
        }

        public class DbConfig
        {
            public string Name { get; set; } // 数据库名称
            public string DbTypeName { get; set; } // 数据库类型名称
            public string ConnectionString { get; set; } // 数据库连接字符串
            public bool Enable { get; set; }
        }

        #endregion 配置类
    }
}