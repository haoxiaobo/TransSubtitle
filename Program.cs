using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TransSrt
{
    class Program
    {
        [MTAThread]
        public static async Task Main(string[] args)
        {
            Options opts = null;

            ParserResult<Options> result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    opts = o;
                    //past the option object out of clouser.
                    //can not run await method at here. becuse ParseArguments() is not async.
                })
                .WithNotParsed(errs => {

                    return;
                });

            if (result.Tag == ParserResultType.Parsed)
            {   
               await DoTrans(opts);
            };

        }

        public static async Task DoTrans(Options opts)
        {
            System.Console.Write("Reading Srt File...");

            #region create subtitle file processor 
            var sExtName = Path.GetExtension(opts.ReadFile);
            string sMsg;
            var processor = ISubProcessor.GetProcessor(sExtName, out sMsg);
            if (processor == null)
            {
                Console.WriteLine("error: "+ sMsg);
                return;
            }

            #endregion

            Subtitle sub = processor.ReadFromFile(opts.ReadFile);
            System.Console.WriteLine("OK!");

            ITransApi api = ITransApi.GetProvider(opts.TransProvider);

            System.Console.Write("Start Translate with driver: {0} ...", api.GetType().Name);

            int iCount = 1;
            foreach (var item in sub.Items)
            {
                var sSrcText = string.Join(" ", item.Texts);
                if (opts.Verbose)
                {
                    Console.Write("{0}/{1}: ", iCount, sub.Items.Count);
                    System.Console.WriteLine("from:" + sSrcText);
                }
                else
                {
                    if (iCount % 100 == 0)
                    {
                        Console.WriteLine("{0}%. {1}/{2} lines",
                            iCount * 100 / sub.Items.Count, iCount, sub.Items.Count);
                    }
                }
                do
                {
                    System.Threading.Thread.Sleep(opts.SleepMs);

                    TransResult result = await api.getTransResult(sSrcText, opts.FromLangCode, opts.ToLangCode);

                    string sDst; //result.trans_result[0].dst
                    if (result == null || result.trans_result == null || result.sCode != "0")
                    {

                        //sDst = result.sMsg;
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("error:{0}, retrying...",
                            result.sMsg);
                        Console.ResetColor();
                    }
                    else
                    {
                        sDst = result.trans_result.Length > 0 ? result.trans_result[0].dst : "(无翻译)";
                        if (opts.Verbose)
                        {
                            Console.WriteLine("to: {0}", sDst);
                        }
                        var lstTranTexts = new List<string>(2);
                        lstTranTexts.Add(sDst);
                        lstTranTexts.Add(sSrcText);

                        item.Texts = lstTranTexts;
                        break;
                    }

                } while (true);
                iCount++;
            }
            string sOutFileName = Path.ChangeExtension(opts.ReadFile, string.Format(
                "{0}.{1}.srt", opts.ToLangCode, opts.TransProvider));
            processor.WriteToFile(sub, sOutFileName);
            Console.WriteLine("file {0} has be written.", sOutFileName);
        }
    }

    class Options
    {
        [Value(0, MetaName = "input_file",
           HelpText = "Input file to be processed.",
           Required = true)]
        public string ReadFile { get; set; }

        [Option('p', "trans_provider", Default = "tencent", Required = false,
            HelpText = "Select Translater Provider. baidu, or tencent")]
        public string TransProvider { get; set; }

        [Option(
            'v', "Verbose",
          Default = true,
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('f', "from_lang",
          Default = "auto",
          HelpText = "The Language code Translater from. use 'auto', 'en'...")]
        public string FromLangCode { get; set; }

        [Option('t', "to_lang",
          Default = "zh",
          HelpText = "The Language code Translater to. use 'zh', ... ")]
        public string ToLangCode { get; set; }


        [Option('s', "sleep_ms",
          Default = 100,
          HelpText = "sleep time in ms after every line.")]
        public int SleepMs { get; set; }

    }
}
