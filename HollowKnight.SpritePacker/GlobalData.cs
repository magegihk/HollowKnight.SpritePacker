namespace HollowKnight.SpritePacker
{
    public class GlobalData
    {
        /// <summary>
        /// 系统语言（Chinese（中文），English（英文）。。。）
        /// </summary>
        private static string systemLanguage = "zh-CN";

        public static string SystemLanguage
        {
            get { return systemLanguage; }
            set
            {
                systemLanguage = value;
                globalLanguage = null;
            }
        }

        private static Language globalLanguage;

        public static Language GlobalLanguage
        {
            get
            {
                if (globalLanguage == null)
                {
                    globalLanguage = new Language();
                    return globalLanguage;
                }
                return globalLanguage;
            }
        }
    }
}