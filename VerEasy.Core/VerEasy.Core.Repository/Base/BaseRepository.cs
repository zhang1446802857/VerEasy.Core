using SqlSugar;
using System.Linq.Expressions;
using VerEasy.Core.IRepository.Base;

namespace VerEasy.Core.Repository.Base
{
    public class BaseRepository<T>(ISqlSugarClient db) : IBaseRepository<T> where T : class, new()
    {
        private readonly ISqlSugarClient _db = db;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> Add(T model)
        {
            return await _db.Insertable(model).ExecuteReturnSnowflakeIdAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<long>> Add(List<T> model)
        {
            return await _db.Insertable(model).ExecuteReturnSnowflakeIdListAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<int> AddPage(List<T> model, int size)
        {
            return await _db.Fastest<T>().PageSize(size).BulkCopyAsync(model);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> Delete(T model)
        {
            return await _db.Deleteable(model).IsLogic().ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Delete(List<T> model)
        {
            return await _db.Deleteable(model).IsLogic().ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Expression<Func<T, bool>> where)
        {
            return await _db.Deleteable<T>().Where(where).IsLogic().ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            return await _db.Deleteable<T>().In(id).IsLogic().ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await _db.Deleteable<T>().In(ids).IsLogic().ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<bool> DeletePage(List<T> model, int size)
        {
            return await _db.Deleteable(model).PageSize(size).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> Query()
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<List<T>> Query(Expression<Func<T, bool>> where)
        {
            return await _db.Queryable<T>().Where(where).ToListAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public async Task<T> QueryById(object id)
        {
            return await _db.Queryable<T>().InSingleAsync(id);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryByIds(object[] ids)
        {
            return await _db.Queryable<T>().In(ids).ToListAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="where"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryPage(Expression<Func<T, bool>> where, int index = 1, int size = 20)
        {
            RefAsync<int> total = 0;
            return await _db.Queryable<T>().WhereIF(where != null, where).ToPageListAsync(index, size, total);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Update(T model)
        {
            return await _db.Updateable(model).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Update(List<T> model)
        {
            return await _db.Updateable(model).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePage(List<T> model)
        {
            return await _db.Fastest<T>().BulkUpdateAsync(model) > 0;
        }
    }
}