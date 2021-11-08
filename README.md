
# TransSrt.Net

## how to use

 translater a subtitle file to double language subtitle file.

usage:

```
TransSrt.exe <input_file> [<options>]

Options:
  -p, --trans_provider    (Default: tencent) Select Translater Provider. baidu, or tencent
  -v, --Verbose           (Default: true) Prints all messages to standard output.
  -f, --from_lang         (Default: auto) The Language code Translater from. use 'auto', 'en'...
  -t, --to_lang           (Default: zh) The Language code Translater to. use 'zh', ...
  -s, --sleep_ms          (Default: 100) sleep time in ms after every line.
  --help                  Display this help screen.
  --version               Display version information.
  input_file (pos. 0)     Required. Input file to be processed.

Example:
    TransSrt.exe testEng.srt
    TransSrt.exe testEng.srt -p tencent -s 200 -f auto -t zh -v
or
    dotnet run testEng.srt
    dotnet run testEng.srt -p tencent -s 200 -f auto -t zh -v

```

## source structure description

- interface **ISubProcessor**: A subtitle file processor interface, read and write subtitle file.
- class **SrtProcessor**: Explament from ISubProcessor, for .srt file;
- interface **ITransApi**: A Translator Engieen interface.
- class **Baidu.BaiduTransApi**: implement from ITransApi for baidu translate driver;
- class **Tencent.BaiduTransApi**: implement from ITransApi for tencent translate driver;
- **Keys.cs**: put you api sercert keys. you can get it by register on baidu or tencent cloud.
