
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading.Tasks;

namespace TransSrt
{

    public interface ITransApi
    {
        Task<TransResult> getTransResult(string sSourceText, string FromLang, string ToLang);

        public static ITransApi GetProvider(string sDriverName){
            switch (sDriverName.ToLower()) {
                case "bd":
                case "baidu":
                    return new Baidu.BaiduTransApi();

                case "qq":
                case "tmt":
                case "tencent":
                    return new Tencent.TMTWarpper();

                default:
                    return new Tencent.TMTWarpper();
            }
        }

    }
}