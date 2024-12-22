using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    [SugarTable("T_QzJobPlan")]
    public class QzJobPlan : BaseModel
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(ColumnDescription = "任务名称")]
        public string JobName { get; set; }

        /// <summary>
        /// 任务分组
        /// </summary>
        [SugarColumn(ColumnDescription = "任务分组")]
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务定时时间规则[Cron表达式]
        /// </summary>
        [SugarColumn(ColumnDescription = "任务分组")]
        public string JobCron { get; set; }

        /// <summary>
        /// 任务所在类名
        /// </summary>
        [SugarColumn(ColumnDescription = "任务所在类名")]
        public string JobClassName { get; set; }

        /// <summary>
        /// 任务所在程序集名称
        /// </summary>
        [SugarColumn(ColumnDescription = "任务所在程序集名称")]
        public string JobAssemblyName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [SugarColumn(ColumnDescription = "任务描述")]
        public string JobDescription { get; set; }

        /// <summary>
        /// 任务传参
        /// </summary>
        [SugarColumn(ColumnDescription = "任务传参")]
        public string JobParams { get; set; }

        /// <summary>
        /// 启动状态
        /// </summary>
        [SugarColumn(ColumnDescription = "启动状态")]
        public bool Enable { get; set; }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        [SugarColumn(ColumnDescription = "任务开始时间")]
        public DateTime? JobBeginTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        [SugarColumn(ColumnDescription = "任务结束时间")]
        public DateTime? JobEndTime { get; set; }
    }
}