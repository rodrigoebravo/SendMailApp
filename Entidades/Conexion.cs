using System;
using System.Net;
using System.Net.Mail;
using System.Threading;

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
	public static class EnvioMailHelper
	{
		public static bool Enviar(string from, string fromName, string to, string subject, string body, bool isBodyHtml = true, MailPriority mailPriority = MailPriority.Normal, string[] adjuntos = null)
		{
			var threadSendMails = new Thread(delegate ()
			{
				try
				{
					using (var mail = new MailMessage())
					{
						// De
						mail.From = new MailAddress(from, fromName);

						// Para
						foreach (var address in to.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
						{
							if (!string.IsNullOrEmpty(address.Trim()))
								mail.To.Add(address);
						}

						// Asunto
						mail.Subject = subject;
						mail.SubjectEncoding = System.Text.Encoding.UTF8;

						mail.Priority = mailPriority;

						if (adjuntos != null)
						{
							foreach (var adjunto in adjuntos)
								mail.Attachments.Add(new Attachment(adjunto));
						}

						// Contenido
						mail.Body = body;
						mail.BodyEncoding = System.Text.Encoding.UTF8;
						mail.IsBodyHtml = isBodyHtml;

						using (var smtpClient = new SmtpClient(Configuracion.Host, Configuracion.Port)
						{
							DeliveryMethod = SmtpDeliveryMethod.Network,
							Credentials = new NetworkCredential(Configuracion.Usr, Configuracion.Pass)
						})
						{
							smtpClient.Send(mail);
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			})
			{
				IsBackground = true
			};

			threadSendMails.Start();
			return true;
		}
	}
}