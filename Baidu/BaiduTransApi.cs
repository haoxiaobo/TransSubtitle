
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading.Tasks;

namespace TransSrt.Baidu
{
    public class BaiduTransApi : ITransApi
    {
        private static string TRANS_API_HOST = "https://fanyi-api.baidu.com/api/trans/vip/translate";
        private string appid;
        private string securityKey;

        // 在平台申请的APP_ID 详见 http://api.fanyi.baidu.com/api/trans/product/desktop?req=developer


        public BaiduTransApi()
        {

            this.appid = Keys.BAIDU_APP_ID;
            this.securityKey = Keys.BAIDU_SECURITY_KEY;

        }


        public async Task<TransResult> getTransResult(string query, string from, string to)
        {
            TransResult result = null;
            int iCount = 0;
            do
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        var url = this.BuildUrl(TRANS_API_HOST, query, from, to);
                        var sJson = wc.DownloadString(url);
                        var serial = new DataContractJsonSerializer(typeof(BaiduTransResult));
                        using (var mStream = new MemoryStream(Encoding.UTF8.GetBytes(sJson)))
                        {
                            BaiduTransResult bd_result = (BaiduTransResult)serial.ReadObject(mStream);

                            result = bd_result.Convert2TransResult();
                            result.sCode = "0";
                            result.sMsg = "OK";
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new TransResult()
                    {
                        sCode = "-1",
                        sMsg = ex.Message
                    };
                }
                iCount++;
            } while ((result == null || result.trans_result == null || result.sCode != "0") && iCount < 3);

            return result;
        }



        private string BuildUrl(string sBaseUrl, string query, string from, string to)
        {
            var para = new Dictionary<string, string>(7);
            para["q"] = query;
            para["from"] = from;
            para["to"] = to;
            para["appid"] = this.appid;
            // 随机数
            string salt = DateTime.Now.Ticks.ToString();
            para["salt"] = salt;

            // 签名
            String src = appid + query + salt + securityKey; // 加密前的原文
            var md5 = MD5.Create();
            var datas = Encoding.UTF8.GetBytes(src);
            var md5b = md5.ComputeHash(datas);
            para["sign"] = byteArrayToHex(md5b);
            StringBuilder sb = new StringBuilder(sBaseUrl);
            sb.Append("?");

            int iCount = 0;
            foreach (var kv in para)
            {
                if (iCount != 0)
                    sb.Append("&");
                sb.Append(kv.Key);
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(kv.Value));
                iCount++;
            }
            return sb.ToString();
        }
        private static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd',
            'e', 'f' };

        private static string byteArrayToHex(byte[] byteArray)
        {
            // new一个字符数组，这个就是用来组成结果字符串的（解释一下：一个byte是八位二进制，也就是2位十六进制字符（2的8次方等于16的2次方））
            char[] resultCharArray = new char[byteArray.Length * 2];
            // 遍历字节数组，通过位运算（位运算效率高），转换成字符放到字符数组中去
            int index = 0;
            foreach (byte b in byteArray)
            {
                resultCharArray[index++] = hexDigits[b >> 4 & 0xf];
                resultCharArray[index++] = hexDigits[b & 0xf];
            }

            // 字符数组组合成字符串返回
            return new string(resultCharArray);
        }
    }
}
