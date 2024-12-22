using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    [SugarTable("T_TaskLogs")]
    public class TaskLogs : BaseModel
    {
        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogMessage { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
    }
}