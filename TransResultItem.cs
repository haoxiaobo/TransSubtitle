using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace TransSrt
{

    public class TransResultItem
    {
        /// <summary>
        /// Source Text
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// Destination Text
        /// </summary>
        public string dst { get; set; }
    }

}