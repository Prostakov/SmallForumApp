using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace TestAuthorizationApp.Services
{
    public class SmsService : ISmsSender
    {
        private readonly string _serviceUri;
        private readonly string _senderLogin;
        private readonly string _senderPassword;
        private readonly string _smsAlphaName;

        private const string _payloadXmlTemplate = 
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<request>" +
                "<operation>SENDSMS</operation>" +
                "<message start_time = \"AUTO\" end_time=\"AUTO\" lifetime=\"4\" rate=\"120\" desc=\"\" source=\"{0}\">" +
                    "<body>{1}</body>" +
                    "<recipient>{2}</recipient>" +
                "</message>" +
            "</request>";

        public SmsService(IOptions<Config> config)
        {
            _serviceUri = config.Value.SmsService.ServiceUri;
            _senderLogin = config.Value.SmsService.SenderLogin;
            _senderPassword = config.Value.SmsService.SenderPassword;
            _smsAlphaName = config.Value.SmsService.SmsAlphaName;
        }

        public async Task SendSmsAsync(string recipient, string message)
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
            var request = (HttpWebRequest)WebRequest.Create(_serviceUri);
            request.Method = "POST";
            request.ContentType = "text/xml; encoding='utf-8'";
            request.Accept = "text/xml";

            // Add header for Http authentication
            var encodedCredentials = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_senderLogin + ":" + _senderPassword));
            request.Headers["Authorization"] = "Basic " + encodedCredentials;

            return request;
        }

        private async Task FillRequestPayload(HttpWebRequest request, string message, string recipient)
        {
            byte[] payloadByteArray = Encoding.UTF8.GetBytes(string.Format(_payloadXmlTemplate, _smsAlphaName, message, recipient));

            // Write payload byte array to stream
            using (var stream = await request.GetRequestStreamAsync())
            {
                stream.Write(payloadByteArray, 0, payloadByteArray.Length);
            }
        }
    }
}
