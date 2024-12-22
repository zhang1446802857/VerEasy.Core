using SqlSugar;

namespace VerEasy.Core.IRepository.UnitOfWorkManage
{
    public interface IUnitOfWorkManage
    {
        /// <summary>
        /// 获取DB
        /// </summary>
        /// <returns></returns>
        SqlSugarClient GetDbClient();

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();
    }
}