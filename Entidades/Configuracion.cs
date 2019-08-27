using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
public static class Configuracion
    {
        public static int Concurrentes;
        public static string EmailsEmisor;
        public static string DestinatariosError;
        public static string Host;
        public static int Port;
        public static string Usr;
        public static string Pass;
        public static string PathCarpetaDatos;
        public static int LotePrestamosPendientes;

        static Configuracion()
        {
            LeerConfiguracion();

        }
        private static void LeerConfiguracion()
        {
            PathCarpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "Datos"); 
            Concurrentes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Concurrentes"]);
            EmailsEmisor = System.Configuration.ConfigurationManager.AppSettings["EmailsEmisor"];
            DestinatariosError = System.Configuration.ConfigurationManager.AppSettings["DestinatariosMailError"];
            Host = System.Configuration.ConfigurationManager.AppSettings["Smtp.Host"];
            Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Smtp.Port"]);
            Usr = System.Configuration.ConfigurationManager.AppSettings["Smtp.Usr"];
            var ps = System.Configuration.ConfigurationManager.AppSettings["Smtp.Pass"];
            Pass = ps != string.Empty ? ps : null;
            LotePrestamosPendientes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["LotePrestamosPendientes"]);
        }
    }
}
