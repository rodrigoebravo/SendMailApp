using System.Collections.Generic;

namespace Entidades
{
    public class ManejadorEmails
    {
        #region Propiedades
        public List<Email> Emails { get; set; }
        public int Cant { get; set; }
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
        #endregion
    }
}
