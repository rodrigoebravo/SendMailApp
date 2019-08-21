using System;
using System.Text;

namespace Entidades
{
    public class Email
    {
        #region Propiedades
        public string Nombre { get; set; }
        public string DireccionEmail { get; set; }
        public string Description { get; set; }
        public int Avaible { get; set; }
        public int Modificated { get; set; }
        public int New { get; set; }
        public bool Enviado { get; set; }
        public DateTime? HoraProceso { get; set; }
        #endregion

        #region Constructores
        private Email()
        {
            this.Modificated = 0;
            this.New = 1;
            this.Enviado = false;
            this.HoraProceso = null;
        }
        public Email(string nombre, string direccion, string descripcion, int avaible) : this()
        {
            Nombre = nombre;
            DireccionEmail = direccion;
            Description = descripcion;
            Avaible = avaible;
        }
        #endregion

        #region Metodos
        public bool Equals(Email e2)
        {
            return DireccionEmail == e2.DireccionEmail;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} | {1} | {2}", this.DireccionEmail, this.Nombre, this.HoraProceso);
            return sb.ToString();
        }
        #endregion
    }
}
