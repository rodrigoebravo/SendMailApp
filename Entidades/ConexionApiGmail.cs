using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Entidades
{
    public class ConexionApiGmail
    {
        static string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailSend };

        public static bool SendMail(string contacto, string pathTemplate)
        {
            try
            {
                UserCredential credential = GetCredential();
                GmailService service = GetService(credential);

                //string plainText = $"To: {contacto}\r\n" +
                //               $"Subject: {ProcesoPrincipal.Instancia.ProcesoMensaje.Asunto} Test\r\n" +
                //               "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                //               $"<h1>{ProcesoPrincipal.Instancia.ProcesoMensaje.CuerpoMensaje}</h1>" +
                //               $"{ConvertDocToHtml(Path.Combine(Directory.GetCurrentDirectory(), @"Templates\Template.docx"), Path.Combine(Directory.GetCurrentDirectory(), @"Templates\Template.html"))}";
                string plainText = $"To: {contacto}\r\n" + "Subject: Test\r\n" + ObtenerTemplate(pathTemplate);

                var newMsg = new Google.Apis.Gmail.v1.Data.Message();
                newMsg.Raw = Base64UrlEncode(plainText.ToString());
                //service.Users.Messages.Send(newMsg, "me").Execute();
                return true;

            }
            catch (Exception)
            {
                //TODO: Acá loggear en ruta especifica de log el motivo real
                return false;
            }
        }

        private static string ObtenerTemplate(string pathTemplate)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (File.Exists(pathTemplate))
                {
                    using (StreamReader sr = new StreamReader(pathTemplate, Encoding.Default))
                    {
                        while (!sr.EndOfStream)
                        {
                            sb.AppendLine(sr.ReadLine());
                        }
                    }
                }//TODO:Eliminar el siguiente else
                else
                {
                    sb.AppendLine("<h1>Test</h1>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static GmailService GetService(UserCredential credential)
        {
            string ApplicationName = "Send Mail App .NET Gmail";
            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        private static UserCredential GetCredential()
        {
            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            return credential;
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}
