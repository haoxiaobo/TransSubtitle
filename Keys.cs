using System;
using System.Collections.Generic;
using System.Text;

namespace TransSrt
{
    public partial class Keys
    {
        // please register you own account on baidu cloud, or tencent cloud, and fill the keys in here.
        // remenber: remove this keys when you submit codes into public code lib.
#if !DEBUG
        public static string BAIDU_APP_ID = "";
        public static string BAIDU_SECURITY_KEY = "";

        public readonly static int TMT_PriID = 0;
        public readonly static string TMT_SecretId = "";
        public readonly static string TMT_SecretKey = "";
#endif

    }
}
