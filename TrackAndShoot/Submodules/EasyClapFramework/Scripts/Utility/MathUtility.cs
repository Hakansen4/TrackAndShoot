namespace EasyClap.Seneca.Common.Utility
{
    public static class MathUtility
    {
        /// <summary>
        /// Returns 1, -1 or 0 depending on floating number's sign
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int Sign(this float f)
        {
            if (f > 0)
            {
                return 1;
            }
            
            if (f < 0)
            {
                return -1;
            }

            return 0;
        }
    }
}