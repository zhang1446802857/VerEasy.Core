using System.Linq.Expressions;

namespace VerEasy.Core.IRepository.Base
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 根据ID查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> QueryById(object id);

        /// <summary>
        /// 根据IDs数组查询实体集
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<T>> QueryByIds(object[] ids);

        /// <summary>
        /// 全量查询
        /// </summary>
        /// <returns></returns>
        Task<List<T>> Query();

        /// <summary>
        /// 拉姆达表达式查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<List<T>> Query(Expression<Func<T, bool>> where);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<List<T>> QueryPage(Expression<Func<T, bool>> where, int index = 1, int size = 20);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<long> Add(T model);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<long>> Add(List<T> model);

        /// <summary>
        /// 分批插入
        /// </summary>
        /// <param name="model">数据</param>
        /// <param name="size">批次大小</param>
        /// <returns></returns>
        Task<int> AddPage(List<T> model, int size);

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteById(object id);

        /// <summary>
        /// 根据IDs删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteByIds(object[] ids);

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Delete(T model);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Delete(List<T> model);

        /// <summary>
        /// 分批删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<bool> DeletePage(List<T> model, int size);

        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> Delete(Expression<Func<T, bool>> where);

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Update(T model);

        /// <summary>
        /// 更新models
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Update(List<T> model);

        /// <summary>
        /// 分批更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> UpdatePage(List<T> model);
    }
}