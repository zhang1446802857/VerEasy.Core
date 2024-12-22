using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Reflection;
using VerEasy.Common.FastCode;
using VerEasy.Common.Helper;
using VerEasy.Core.Models.Dtos;
using static VerEasy.Common.FastCode.FastCodeExtension;

namespace VerEasy.Core.Api.Controllers.DbFirst
{
    /// <summary>
    /// 代码生成相关
    /// </summary>
    /// <param name="db"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class DbFirstController(ISqlSugarClient db) : ControllerBase
    {
        private readonly ISqlSugarClient db = db;

        /// <summary>
        /// 生成ViewModel文件
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <returns></returns>
        [HttpGet("CreateViewModelsFile")]
        public MessageModel<bool> CreateViewModelsFile(string? path, string? nameSpace)
        {
            if (FrameCode.CreateModelsFile(db, path, nameSpace))
            {
                return MessageModel<bool>.Ok("生成成功!");
            }
            return MessageModel<bool>.Fail("生成失败!");
        }

        /// <summary>
        /// 生成数据库表结构
        /// </summary>
        /// <param name="modelName">model层名称</param>
        /// <param name="filterByNameSpace">根据命名空间过滤</param>
        /// <returns></returns>
        [HttpGet("CreateDataTable")]
        public MessageModel<bool> CreateDataTable(string? modelName, string? filterByNameSpace)
        {
            if (FrameCode.CreateDataTable(db, modelName, filterByNameSpace))
            {
                return MessageModel<bool>.Ok("生成成功!");
            }
            return MessageModel<bool>.Fail("生成失败!");
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetModelNames")]
        public MessageModel<string[]> GetModelNames()
        {
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var assembly = Assembly.LoadFrom(Path.Combine(basePath, $"VerEasy.Core.Models.dll"));
            var types = assembly.GetTypes().Where(x => x.FullName.Contains($"ViewModels")).ToArray();
            var modelNames = types.Select(x => x.Name).ToArray();
            return MessageModel<string[]>.Ok(modelNames);
        }

        /// <summary>
        /// 生成项目文件
        /// </summary>
        /// <param name="fastCodeConfig">单层生成配置</param>
        /// <returns></returns>
        [HttpPost("CreateFilesByModels")]
        public MessageModel<bool> CreateFilesByModels(FastCodeConfig fastCodeConfig)
        {
            if (fastCodeConfig.FastAuto == false)
            {
                if (string.IsNullOrEmpty(fastCodeConfig.NameSpace))
                    throw new ArgumentException("NameSpace is required when FastAuto is false.");
                if (string.IsNullOrEmpty(fastCodeConfig.Path))
                    throw new ArgumentException("Path is required when FastAuto is false.");
                if (fastCodeConfig.ModelNames == null || fastCodeConfig.ModelNames.Length == 0)
                    throw new ArgumentException("ModelNames are required when FastAuto is false.");
            }
            if (FrameCode.CreateFilesByModels(db, fastCodeConfig.FastAuto ? null : fastCodeConfig))
            {
                return MessageModel<bool>.Ok("生成成功!");
            }
            return MessageModel<bool>.Fail("生成失败!");
        }

        /// <summary>
        /// 保存生成项目结构
        /// </summary>
        [HttpGet("SaveProjectStructure")]
        public void SaveProjectStructure()
        {
            ProjectHelper.FileTxt();
            Console.WriteLine("OK");
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        [HttpGet("InitData")]
        public void InitData()
        {
            DbSeed.InitData(db);
        }

        /// <summary>
        /// 生成初始化数据种子
        /// </summary>
        [HttpGet("GenerateInitJsonData")]
        public void GenerateInitJsonData()
        {
            DbSeed.GenerateInitJsonData(db);
        }
    }
}