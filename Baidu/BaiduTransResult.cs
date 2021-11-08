using System;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Linq;

namespace TransSrt.Baidu
{
    [Serializable]
    [DataContract]
    public class BaiduTransResult
    {
        [DataMember]
        public string from { get; set; }

        [DataMember]
        public string to { get; set; }


        [DataMember]
        public BaiduTransResultItem[] trans_result { get; set; }


        /// <summary>
        /// Convert to Common model
        /// </summary>
        /// <returns></returns>
        public TransResult Convert2TransResult()
        {
            TransResult r = new TransResult()
            {
                from = this.from,
                to = this.to,
                trans_result = new TransResultItem[this.trans_result.Length]
            };
            for (int i = 0; i < this.trans_result.Length; i++) {
                r.trans_result[i] = new TransResultItem() {
                    dst = this.trans_result[i].dst,
                    src = this.trans_result[i].src
                };
            }           
            return r;
        }
    }

}