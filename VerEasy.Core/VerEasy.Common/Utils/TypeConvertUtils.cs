namespace VerEasy.Common.Utils
{
    public static class TypeConvertUtils
    {
        /// <summary>
        /// 转换bool
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool ObjToBool(this object value)
        {
            return value != null && bool.TryParse(value.ToString(), out bool result) && result;
        }

        /// <summary>
        /// 转换string
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string ObjToString(this object value)
        {
            return value?.ToString().Trim() ?? string.Empty;
        }

        public static long ObjToLong(this object value)
        {
            long reval = 0;
            if (value == null) return 0;
            if (value != DBNull.Value && long.TryParse(value.ToString(), out reval))
            {
                return reval;
            }

            return reval;
        }
    }
}