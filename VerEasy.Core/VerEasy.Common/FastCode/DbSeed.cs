using Newtonsoft.Json;
using SqlSugar;
using System.Data;
using System.Reflection;
using System.Text;

namespace VerEasy.Common.FastCode
{
    public static class DbSeed
    {
        /// <summary>
        /// 映射类型
        /// </summary>
        public static readonly Dictionary<string, Type> TypeMap = new Dictionary<string, Type>
        {
            { "int", typeof(int) },
            { "bigint", typeof(long) },
            { "smallint", typeof(short) },
            { "tinyint", typeof(byte) },
            { "varchar", typeof(string) },
            { "nvarchar", typeof(string) },
            { "text", typeof(string) },
            { "char", typeof(string) },
            { "datetime", typeof(DateTime) },
            { "smalldatetime", typeof(DateTime) },
            { "date", typeof(DateTime) },
            { "bit", typeof(bool) },
            { "decimal", typeof(decimal) },
            { "numeric", typeof(decimal) },
            { "float", typeof(float) },
            { "real", typeof(double) },
            { "binary", typeof(byte[]) },
            { "varbinary", typeof(byte[]) },
            // 添加其他数据库类型映射
        };

        /// <summary>
        /// 初始化数据
        /// </summary>
        public static void InitData(ISqlSugarClient db)
        {
            var seeds = GetJsonSeed<TableStructureData>();

            foreach (var seed in seeds)
            {
                var typeBilder = db.DynamicBuilder().CreateClass(seed.TableName, new SugarTable() { });

                //可以循环添加列
                typeBilder.CreateProperty("CreateTime", typeof(DateTime), new SugarColumn() { DefaultValue = DateTime.Now.ToString(), ColumnDescription = "创建时间" });
                typeBilder.CreateProperty("UpdateTime", typeof(DateTime), new SugarColumn() { DefaultValue = DateTime.Now.ToString(), ColumnDescription = "修改时间" });
                typeBilder.CreateProperty("CreateBy", typeof(long), new SugarColumn() { DefaultValue = "0", ColumnDescription = "创建人" });
                typeBilder.CreateProperty("UpdateBy", typeof(long), new SugarColumn() { DefaultValue = "0", ColumnDescription = "修改人" });
                typeBilder.CreateProperty("IsDeleted", typeof(bool), new SugarColumn() { DefaultValue = "0", ColumnDescription = "是否删除" });

                foreach (var column in seed.Columns)
                {
                    Type columnType = typeof(string);
                    if (TypeMap.TryGetValue(column.DataType, out Type value))
                    {
                        columnType = value;
                    }
                    typeBilder.CreateProperty(column.ColumnName, columnType, new SugarColumn() { IsPrimaryKey = column.IsPrimaryKey, Length = column.Length, IsNullable = column.IsNullable, ColumnDescription = column.ColumnDescription });
                }

                //创建类
                var model = typeBilder.BuilderType();//想缓存有typeBilder.WithCache

                //创建表
                db.CodeFirst.InitTables(model);

                if (!string.IsNullOrEmpty(seed.DataSql))
                {
                    db.Ado.ExecuteCommand(seed.DataSql);
                }
            }
        }

        /// <summary>
        /// 生成初始化数据种子
        /// </summary>
        /// <param name="db"></param>
        public static void GenerateInitJsonData(ISqlSugarClient db)
        {
            var ignoredFields = new HashSet<string> { "CreateTime", "UpdateTime", "CreateBy", "UpdateBy", "IsDeleted" };

            // 获取所有表的结构信息
            var tables = db.DbMaintenance.GetTableInfoList()
                .Where(x => x.Name != "T_SysLogs");//serilog自动生成

            var tableStructure = new List<TableStructureData>();
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var fileName = $"TableSeed.json";
            var filePath = Path.Combine(desktopPath, fileName);
            // 遍历每个表，获取表的字段信息
            foreach (var table in tables)
            {
                var tableName = table.Name;

                // 获取该表的列信息
                var columns = db.DbMaintenance.GetColumnInfosByTableName(tableName)
                    .Where(x => !ignoredFields.Contains(x.DbColumnName));

                var columnData = columns.Select(column => new ColumnStructure
                {
                    ColumnName = column.DbColumnName,
                    DataType = column.DataType,
                    IsPrimaryKey = column.IsPrimarykey,
                    IsNullable = column.IsNullable,
                    Length = column.Length,
                    ColumnDescription = column.ColumnDescription
                }).ToList();

                // 查询该表的数据
                var tableData = db.Queryable<dynamic>()
                    .AS(tableName) // 指定表名
                    .ToList();

                var insertSql = string.Empty;
                if (tableData.Count != 0)
                {
                    insertSql = GenerateInsertSql(tableName, columnData, tableData);
                }

                // 将表结构和数据加入结果
                var tableEntry = new TableStructureData
                {
                    TableName = tableName,
                    Columns = columnData,
                    DataSql = insertSql
                };

                tableStructure.Add(tableEntry);
            }

            // 序列化为 JSON 格式
            var json = JsonConvert.SerializeObject(tableStructure, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// 将数据拼接成批量插入的sql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">字段集</param>
        /// <param name="data">数据集</param>
        /// <returns></returns>
        public static string GenerateInsertSql(string tableName, List<ColumnStructure> columns, List<dynamic> data)
        {
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine($"INSERT INTO {tableName} ({string.Join(", ", columns.Select(c => c.ColumnName))}) VALUES");

            var values = new List<string>();

            foreach (var row in data)
            {
                var rowValues = new List<string>();

                if (row is IDictionary<string, object> rowDict)
                {
                    foreach (var column in columns)
                    {
                        var columnValue = rowDict.ContainsKey(column.ColumnName) ? rowDict[column.ColumnName] : null;
                        // 如果列名是 Id
                        if (column.ColumnName == "Id")
                        {
                            //columnValue = IdGenUtils.GenerateSnowflakeId().ToSqlValue();
                        }
                        // 获取动态对象中与列名匹配的值

                        // 如果列值存在，则加引号并添加到 values 中，如果没有则加入 NULL
                        rowValues.Add(columnValue != null ? $"'{columnValue}'" : "NULL");
                    }
                    values.Add($"({string.Join(", ", rowValues)})");
                }
            }

            sqlBuilder.AppendLine(string.Join(", ", values) + ";");

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 获取json种子数据
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        private static List<T> GetJsonSeed<T>()
        {
            // 获取当前执行程序的程序集
            var assembly = Assembly.GetExecutingAssembly();

            // 获取嵌入资源的名称，通常是项目命名空间 + 文件名
            var resourceName = $"VerEasy.Common.FastCode.Seed.TableSeed.json";
            // 读取嵌入资源流
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            var jsonTxt = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<T>>(jsonTxt);
        }

        /// <summary>
        /// 用于存储表结构信息的类
        /// </summary>
        public class TableStructureData
        {
            public string TableName { get; set; }
            public List<ColumnStructure> Columns { get; set; }
            public string DataSql { get; set; }
        }

        /// <summary>
        /// 用于存储列结构信息的类
        /// </summary>
        public class ColumnStructure
        {
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public bool IsPrimaryKey { get; set; }
            public bool IsNullable { get; set; }
            public int Length { get; set; }
            public string ColumnDescription { get; set; }
        }
    }
}