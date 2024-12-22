using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    [SugarTable("T_QzJobRecord")]
    public class QzJobRecord : BaseModel
    {
        /// <summary>
        /// Job任务ID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 任务执行信息
        /// </summary>
        public string JobExcuteMsg { get; set; }

        /// <summary>
        /// 任务执行时间
        /// </summary>
        public DateTime JobExcuteTime { get; set; }
    }
}