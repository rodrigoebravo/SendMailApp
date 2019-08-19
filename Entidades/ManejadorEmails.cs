using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Agrega a la lista de Emails de manejador el Email del parametro si no existe.
        /// </summary>
        /// <param name="manejador"></param>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Quita de la lista de Emails de manejador, el Email pasado por parametro.
        /// </summary>
        /// <param name="manejador"></param>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Itera sobre la lista de mails de esta instancia de manejador, enviando cada uno en intervalos de 15 segundos
        /// </summary>
        private void ComenzarSinTimer()
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

        /// <summary>
        /// Itera sobre la lista de mails, utilizando el index de la lista, enviando en intervalos de 15 segundos
        /// </summary>
        private void ComenzarConTimer()
        {
            //Necesario usar EmailsAux ya que es estatico
            EmailsAux = Emails;
            timer = new Timer(comenzarEnvioConTimer);
            timer.Change(TimeSpan.Zero, TimeSpan.Zero);
        }

        /// <summary>
        /// Comenzará el proceso con o sin timer segun se indique
        /// </summary>
        /// <param name="timer"></param>
        public void Comenzar(bool timer)
        {
            if (timer)
                this.ComenzarConTimer();
            else
                this.ComenzarSinTimer();
        }

        /// <summary>
        /// Metodo con firma requerida por el objeto timer, la misma busca el ultimo contacto al que se le envió mails para enviar.
        /// </summary>
        /// <param name="state"></param>
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
        /// <summary>
        /// Trae, por index, el siguiente contacto guardando el valor para el proximo contacto;
        /// </summary>
        /// <returns></returns>
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
