using IdGen;
using SqlSugar;

namespace VerEasy.Core.Models.Base
{
    public class BaseModel(long id = 0)
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键")]
        public long Id { get; set; } = id != 0 ? id : new IdGenerator(0).CreateId();

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnDescription = "修改时间")]
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnDescription = "创建人")]
        public long CreateBy { get; set; }

        [SugarColumn(ColumnDescription = "修改人")]
        public long UpdateBy { get; set; }

        [SugarColumn(ColumnDescription = "是否删除")]
        public bool IsDeleted { get; set; } = false;
    }
}