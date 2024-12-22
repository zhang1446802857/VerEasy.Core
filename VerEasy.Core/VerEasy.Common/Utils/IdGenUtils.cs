using IdGen;

namespace VerEasy.Common.Utils
{
    public static class IdGenUtils
    {
        private static IdGenerator _idGenerator;

        static IdGenUtils() => _idGenerator = new IdGenerator(0);

        /// <summary>
        /// 设置机器ID
        /// </summary>
        /// <param name="machId"></param>
        public static void SetMachineId(int machId) => _idGenerator = new IdGenerator(machId);

        /// <summary>
        /// 生成雪花ID
        /// </summary>
        /// <returns></returns>
        public static long GenerateSnowflakeId() => _idGenerator.CreateId();

        /// <summary>
        /// 获取当前的Id生成器
        /// </summary>
        public static IdGenerator GetGenerator => _idGenerator;
    }
}