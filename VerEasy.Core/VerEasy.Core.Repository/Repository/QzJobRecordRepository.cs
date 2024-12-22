using SqlSugar;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Repository.Base;

namespace VerEasy.Core.IRepository.IRepository
{
    public class QzJobRecordRepository(ISqlSugarClient db) : BaseRepository<QzJobRecord>(db), IQzJobRecordRepository
    {
    }
}