using TransSrt;
using TencentCloud.Common;
using System;
using System.Threading.Tasks;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;


namespace TransSrt.Tencent
{
    public class TMTWarpper : ITransApi
    {


        public async Task<TransResult> getTransResult(string sSourceText, string FromLang, string ToLang)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = Keys.TMT_SecretId,
                    SecretKey = Keys.TMT_SecretKey
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                TmtClient client = new TmtClient(cred, "ap-beijing", clientProfile);
                TextTranslateRequest req = new TextTranslateRequest();
                req.SourceText = sSourceText;
                req.Source = FromLang;
                req.Target = ToLang;
                req.ProjectId = Keys.TMT_PriID;
                TextTranslateResponse resp = null;
                try
                {
                    resp = await client.TextTranslate(req);
                    // Console.WriteLine(AbstractModel.ToJsonString(resp));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return new TransResult()
                    {
                        sMsg = ex.Message,
                        sCode = "FFFF"
                    };
                }
                return new TransResult()
                {
                    sCode = "0",
                    from = FromLang,
                    to = ToLang,
                    trans_result = new TransResultItem[]{
                        new TransResultItem(){
                            src =  sSourceText, dst = resp.TargetText
                        } },
                    sMsg = "ok",
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new TransResult()
                {
                    sMsg = e.Message,
                    sCode = "FFFF"
                };
            }

        }
    }
}
