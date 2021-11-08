using System;
using System.Collections.Generic;
using System.Text;

namespace TransSrt
{
    interface ISubProcessor
    {
        public Subtitle ReadFromFile(string sFileName);
        public void WriteToFile(Subtitle sub, String sFileName);

        public static ISubProcessor GetProcessor(string sFileExt, out string sMsg)
        {
            switch (sFileExt.ToLower())
            {
                case ".srt":
                    sMsg = "OK";
                    return new SrtProcessor();
                default:
                    sMsg = "Not Supported file type "+ sFileExt;
                    return null;
            }
        }
    }
}
