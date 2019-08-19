using System;
using System.Net;
using System.Net.Mail;

namespace Entidades
{
    public class Conexion
    {
        #region Propiedades
        public string User { get; set; }
        public string Pass { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        #endregion

        #region Constructores
        public Conexion(string user, string pass, int port, string host)
        {
            this.User = user;
            this.Pass = pass;
            this.Port = port;
            this.Host = host;
        }
        #endregion

        #region Metodos
        public bool Conectar(string to, string subjet, string body)
        {
            SmtpClient smtp = new SmtpClient(Host, Port);
            NetworkCredential cert = new NetworkCredential(User, Pass);
            smtp.Credentials = cert;
            smtp.EnableSsl = true;

            MailMessage mail = new MailMessage(User, to, subjet, body);
            try
            {
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}