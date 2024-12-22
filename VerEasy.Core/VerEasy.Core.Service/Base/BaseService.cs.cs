using System.Linq.Expressions;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.Base;

namespace VerEasy.Core.Service.Base
{
    public class BaseService<T>(IBaseRepository<T> baseRepo) : IBaseService<T> where T : class, new()
    {
        private readonly IBaseRepository<T> _baseRepo = baseRepo;

        public async Task<long> Add(T model) => await _baseRepo.Add(model);

        public async Task<List<long>> Add(List<T> model) => await _baseRepo.Add(model);

        public async Task<int> AddPage(List<T> model, int size) => await _baseRepo.AddPage(model, size);

        public async Task<bool> Delete(T model) => await _baseRepo.Delete(model);

        public async Task<bool> Delete(List<T> model) => await _baseRepo.Delete(model);

        public async Task<bool> Delete(Expression<Func<T, bool>> where) => await _baseRepo.Delete(where);

        public async Task<bool> DeleteById(object id) => await _baseRepo.DeleteById(id);

        public async Task<bool> DeleteByIds(object[] ids) => await _baseRepo.DeleteByIds(ids);

        public async Task<bool> DeletePage(List<T> model, int size) => await _baseRepo.DeletePage(model, size);

        public async Task<List<T>> Query() => await _baseRepo.Query();

        public async Task<List<T>> Query(Expression<Func<T, bool>> where) => await _baseRepo.Query(where);

        public async Task<T> QueryById(object id) => await _baseRepo.QueryById(id);

        public async Task<List<T>> QueryByIds(object[] ids) => await _baseRepo.QueryByIds(ids);

        public async Task<List<T>> QueryPage(Expression<Func<T, bool>> where, int index = 1, int size = 20) => await _baseRepo.QueryPage(where, index, size);

        public async Task<bool> Update(T model) => await _baseRepo.Update(model);

        public async Task<bool> Update(List<T> model) => await _baseRepo.Update(model);

        public async Task<bool> UpdatePage(List<T> model) => await _baseRepo.UpdatePage(model);
    }
}