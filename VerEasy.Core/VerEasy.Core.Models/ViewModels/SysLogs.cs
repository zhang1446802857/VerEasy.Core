using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    ///<summary>
    /// SysLogs
    ///</summary>
    [SugarTable("T_SysLogs")]
    public partial class SysLogs 
    {   
        public SysLogs()
        { }
        /// <summary>
        /// 描述:Message
        /// </summary>           
        public string Message {get;set;}
        /// <summary>
        /// 描述:MessageTemplate
        /// </summary>           
        public string MessageTemplate {get;set;}
        /// <summary>
        /// 描述:Level
        /// </summary>           
        public string Level {get;set;}
        /// <summary>
        /// 描述:TimeStamp
        /// </summary>           
        public DateTime TimeStamp {get;set;}
        /// <summary>
        /// 描述:Exception
        /// </summary>           
        public string Exception {get;set;}
        /// <summary>
        /// 描述:Properties
        /// </summary>           
        public string Properties {get;set;}
    }
}