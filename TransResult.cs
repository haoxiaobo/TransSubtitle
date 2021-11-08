using System;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Linq;

namespace TransSrt
{
    [Serializable]
    [DataContract]
    public class TransResult
    {
        /// <summary>
        /// 0 is Succ.
        /// </summary>
        public string sCode { get; set; }

        public string sMsg { get; set; }


        /// <summary>
        /// from language code
        /// </summary>
        public string from { get; set; }
  
        /// <summary>
        /// to language code
        /// </summary>
        public string to { get; set; }


        public TransResultItem[] trans_result { get; set; }

        public override string ToString()
        {
            return String.Join("\r\n", (
                from item in this.trans_result
                select item.src + " --> " + item.dst).ToArray());
        }
    }
}