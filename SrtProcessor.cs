using System.IO;
using System.Collections.Generic;
using System;
namespace TransSrt
{

    public class SrtProcessor : ISubProcessor
    {
        public Subtitle ReadFromFile(string sFileName)
        {
            List<string> lstLines = new List<string>();
            using (StreamReader rd = new StreamReader(sFileName))
            {
                do
                {
                    var sLine = rd.ReadLine();
                    if (sLine == null)
                    {
                        break;
                    }
                    lstLines.Add(sLine);
                } while (true);
                rd.Close();
            }
            var sub = new Subtitle();
            SubtitleItem itemLast = null;
            int status = 0;
            int iIndex = 0;
            // 0: looking for next item;
            // 1: in text;
            for (int i = 0; i < lstLines.Count; i++)
            {
                if (status == 0)
                { // in empty line. finding next item.
                    if (string.IsNullOrWhiteSpace(lstLines[i]))
                    {
                        continue;
                    }
                    iIndex++;

                    itemLast = new SubtitleItem();
                    itemLast.Index = iIndex;

                    i++;
                    if (i >= lstLines.Count)
                    {
                        Console.WriteLine("line " + i.ToString() + " error! no time line!!");
                        break;
                    }
                    String timeInfo = lstLines[i];
                    String[] times = timeInfo.Split("-->");
                    if (times.Length != 2)
                    {
                        Console.WriteLine("line " + i + " error! time line format error: " + timeInfo);
                        i++;
                        status = 0;
                        itemLast = null;
                        continue;
                    }

                    itemLast.TimeFrom = times[0];
                    itemLast.TimeTo = times[1];

                    status = 1;
                }
                else if (status == 1)
                { // read text, util blank item;
                    if (string.IsNullOrWhiteSpace(lstLines[i]))
                    {
                        sub.Items.Add(itemLast);
                        status = 0;
                        itemLast = null;
                        continue;
                    }
                    itemLast.Texts.Add(lstLines[i]);
                }
            } // end for;
            if (itemLast != null)
            {
                sub.Items.Add(itemLast);
            }

            return sub;
        }

        public void WriteToFile(Subtitle sub, String sFileName)
        {
            using (var wt = new StreamWriter(sFileName, false))
            {
                int iIndex = 0;
                foreach (var item in sub.Items)
                {
                    // index
                    iIndex++;
                    wt.WriteLine(iIndex.ToString());
                    // time
                    wt.WriteLine(item.TimeFrom + "-->" + item.TimeTo);

                    // texts
                    foreach (var text in item.Texts)
                    {
                        wt.WriteLine(text);

                    }
                    wt.WriteLine();
                }
                wt.Close();
            }
        }
    }
}