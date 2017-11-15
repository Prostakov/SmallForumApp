using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace TestAuthorizationApp.Services
{
    public class SmsService
    {
        private readonly string _serviceUri;
        private readonly string _senderLogin;
        private readonly string _senderPassword;
        private readonly string _smsAlphaName;

        public SmsService(IOptions<Config> config)
        {
            _serviceUri = config.Value.SmsService.ServiceUri;
            _senderLogin = config.Value.SmsService.SenderLogin;
            _senderPassword = config.Value.SmsService.SenderPassword;
            _smsAlphaName = config.Value.SmsService.SmsAlphaName;
        }

        public async Task SendSmsAsync(string message, string recipient)
        {
            var request = CreateRequest();

            await FillRequestPayload(request, message, recipient);

            var response = (HttpWebResponse) await request.GetResponseAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                // ignored... for now...
            }
        }

        private HttpWebRequest CreateRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_serviceUri);
            request.Method = "POST";
            request.ContentType = "text/xml; encoding='utf-8'";
            request.Accept = "text/xml";

            // Add header for Http authentication
            String encodedCredentials = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_senderLogin + ":" + _senderPassword));
            request.Headers["Authorization"] = "Basic " + encodedCredentials;

            return request;
        }

        private async Task FillRequestPayload(HttpWebRequest request, string message, string recipient)
        {
            var payload = new StringBuilder();
            payload.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            payload.Append("<request>");
            payload.Append("<operation>SENDSMS</operation>");
            payload.Append("<message start_time = \"AUTO\" end_time=\"AUTO\" lifetime=\"4\" rate=\"120\" desc=\"\" source=\"" + _smsAlphaName + "\">");
            payload.Append("<body>" + message + "</body>");
            payload.Append("<recipient>" + recipient + "</recipient>");
            payload.Append("</message>");
            payload.Append("</request>");

            byte[] payloadByteArray = Encoding.UTF8.GetBytes(payload.ToString());

            // Write payload byte array to stream
            using (var stream = await request.GetRequestStreamAsync())
            {
                stream.Write(payloadByteArray, 0, payloadByteArray.Length);
            }
        }
    }
}
