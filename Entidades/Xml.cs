using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Entidades
{
    public class Xml
    {
        #region Metodos
        /// <summary>
        /// Guarda la lista de mails en la ruta especificada en formato XML
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Guardar(List<Email> emails, string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Email>));
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8))
                {
                    ser.Serialize(writer, emails);
                    return true;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Abre el archivo (xml) de la ruta del parametro
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Lista de mails</returns>
        public List<Email> Abrir(string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Email>));
            List<Email> emails = new List<Email>();
            try
            {
                using (XmlTextReader reader = new XmlTextReader(path))
                {
                    emails = (List<Email>)ser.Deserialize(reader);
                    return emails;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
