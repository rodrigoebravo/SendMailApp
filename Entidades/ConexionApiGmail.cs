using Google.Apis.Gmail.v1;

namespace Entidades
{
    public class ConexionApiGmail
    {
        static string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailSend };
        static string ApplicationName = "Gmail API .NET Quickstart";
    }
}
