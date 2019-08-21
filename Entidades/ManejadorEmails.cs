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

        public static int Index { get { return Controlador.Instance.ObtenerIndex(); } private set { Controlador.Instance.SetIndex(value); } }

        public static List<Email> EmailsAux { get; set; }
        public EstadoProceso Estado { get { return Controlador.Instance.GetEstado(); } }
        #endregion


        #region Constructores
        public ManejadorEmails()
        {
            this.Emails = new List<Email>();
            this.Cant = 0;
        }
        #endregion
        #region OperadoresSobrecargados
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
        #endregion

        #region Metodos

        public void PausarProceso()
        {
            Controlador.Instance.SetEstado(EstadoProceso.Pausado);
            Controlador.Instance.FinalizarTimer();
        }
        public void ReiniciarProceso()
        {
            Controlador.Instance.SetEstado(EstadoProceso.Iniciado);
            Controlador.Instance.ReiniciarTimer(comenzarEnvioConTimer);
        }
        public void DetenerProceso()
        {
            Controlador.Instance.SetEstado(EstadoProceso.Detenido);
            Controlador.Instance.EliminarTimer();
        }

        /// <summary>
        /// Comenzará el proceso con o sin timer segun se indique
        /// </summary>
        /// <param name="usarTimer"></param>
        public void Comenzar()
        {
            //Necesario usar EmailsAux ya que es estatico
            EmailsAux = Emails;
            Controlador.Instance.InicializarControl(comenzarEnvioConTimer);
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
                Controlador.Instance.SetEstado(EstadoProceso.Procesando);
                var contacto = TraerProximoContacto();
                continuar = !string.IsNullOrEmpty(contacto);
                if (!continuar)
                {
                    Controlador.Instance.FinalizarTimer();
                    Controlador.Instance.SetEstado(EstadoProceso.FinalizadoOk);
                    return;
                }
                //TODO: descomentar linea para enviar correos
                EmailsAux[Index - 1].Enviado = true;// ConexionApiGmail.SendMail(contacto, string.Empty);
                EmailsAux[Index - 1].HoraProceso = DateTime.Now;
                Controlador.Instance.SumarCantidadMailsEnviadosHoy();
                Controlador.Instance.GuardarIndex();
            }
            catch (Exception)
            {
                // TODO: Guardar en archivo LOG
            }
            finally
            {
                if (Controlador.Instance.PausarProcesoPorHoy())
                {
                    Controlador.Instance.InicializarCantidadEnviadosHoy();
                    Controlador.Instance.Esperar24Horas();
                }
                else if (continuar)
                {
                    Controlador.Instance.EsperarSegundos();
                }
            }
        }

        /// <summary>
        /// Trae, por index, el siguiente contacto guardando el valor para el proximo contacto;
        /// </summary>
        /// <returns></returns>
        private static string TraerProximoContacto()
        {
            if (Index < EmailsAux.Count)
            {
                var contacto = EmailsAux[Index].DireccionEmail;
                Index++;
                return contacto;
            }
            return string.Empty;
        }
        #endregion
    }
}
