using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Entidades
{
    public class ManejadorEmails
    {
        #region Propiedades
        public List<Email> Emails { get; set; }
        public int Cant { get; set; }
        private static Timer timer = new Timer(comenzarEnvioConTimer);
        private const int TIEMPO_ESPERA_SEGUNDOS = 15;
        private static int index = 0;
        private static List<Email> EmailsAux { get; set; }
        #endregion

        #region Constructores
        public ManejadorEmails()
        {
            Emails = new List<Email>();
            Cant = 0;
        }
        #endregion

        #region Metodos
        public static bool operator +(ManejadorEmails manejador, Email email)
        {
            foreach (Email e in manejador.Emails)
            {
                if (email.Equals(e))
                    return false;
            }
            manejador.Emails.Add(email);
            manejador.Cant++;
            return true;
        }
        public static bool operator -(ManejadorEmails manejador, Email email)
        {
            foreach (Email e in manejador.Emails)
            {
                if (email.Equals(e))
                {
                    email.Avaible = 0;
                    manejador.Cant--;
                    return true;
                }
            }
            return false;
        }

        public void ComenzarEnvioSinTimer()
        {
            for (int i = 0; i < this.Emails.Count; i++)
            {
                this.Emails[i].Enviado = ConexionApiGmail.SendMail(this.Emails[i].DireccionEmail, "pathTemplate");

                if (this.Emails[i].Enviado)
                    this.Emails[i].HoraEnvio = DateTime.Now;

                //TODO: Loggear el envio.

                //Espera 15 segundos antes de enviar el siguiente
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }

        public void ComenzarConTimer()
        {
            //Necesario usar EmailsAux ya que es estatico
            EmailsAux = Emails;
            timer = new Timer(comenzarEnvioConTimer);
            timer.Change(TimeSpan.Zero, TimeSpan.Zero);
        }
        private static void comenzarEnvioConTimer(object state)
        {
            bool continuar = true;
            try
            {
                var contacto = TraerProximoContacto();
                continuar = !string.IsNullOrEmpty(contacto);
                if (!continuar)
                {
                    timer.Dispose();
                    return;
                }
                EmailsAux[index - 1].Enviado = ConexionApiGmail.SendMail(contacto, string.Empty);
                EmailsAux[index - 1].HoraEnvio = DateTime.Now;
            }
            catch (Exception)
            {
                // LOGGGER --> 
            }
            finally
            {
                if (continuar)
                    timer.Change(TimeSpan.FromSeconds(TIEMPO_ESPERA_SEGUNDOS), TimeSpan.Zero);
            }
        }
        private static string TraerProximoContacto()
        {
            if (index < EmailsAux.Count)
            {
                var contacto = EmailsAux[index].DireccionEmail;
                index++;
                return contacto;
            }
            return string.Empty;
        }
        #endregion
    }
}
