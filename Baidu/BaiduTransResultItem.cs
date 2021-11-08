using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace TransSrt.Baidu
{
    [DataContract]
    [Serializable]
    public class BaiduTransResultItem
    {
        /// <summary>
        /// Source Text
        /// </summary>
        [DataMember]
        public string src { get; set; }

        /// <summary>
        /// Destination Text
        /// </summary>
        [DataMember]
        public string dst { get; set; }
    }

}