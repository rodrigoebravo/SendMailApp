using System;
using System.Threading;

namespace Entidades
{
    public class Controlador
    {
        private int contadorPorDia { get; set; }
        private int Index;
        private Timer TimerProceso;
        private EstadoProceso Estado { get; set; }

        #region Constantes
        private static int TIEMPO_ESPERA_SEGUNDOS { get { return new Random().Next(30, 120); } }
        private static int CANTIDAD_MAILS_POR_DIA = 50;
        private static double TIEMPO_ESPERA_24HS = 24;
        #endregion

		/// <summary>
		/// Setea valor al indice
		/// </summary>
		/// <param name="value"></param>
        internal void SetIndex(int value)
        {
            Instance.Index = value;
        }
		/// <summary>
		/// Obtiene valor de indice
		/// </summary>
		/// <returns></returns>
        internal int ObtenerIndex()
        {
            return Instance.Index;
        }
		/// <summary>
		/// Obtiene estado del proceso
		/// </summary>
		/// <returns></returns>
        internal EstadoProceso GetEstado()
        {
            return Instance.Estado;
        }

        #region Singleton
        private readonly static Controlador _instance = new Controlador();
        public static Controlador Instance { get { return _instance; } }
        #endregion

        private Controlador() { }

		/// <summary>
		/// Inicializador de las propiedades
		/// </summary>
		/// <param name="comenzarEnvioConTimer"></param>
        public void InicializarControl(TimerCallback comenzarEnvioConTimer)
        {
            Instance.SetEstado(EstadoProceso.Iniciado);
            Instance.SetIndex(0);
            Instance.TimerProceso = new Timer(comenzarEnvioConTimer);
            Instance.TimerProceso.Change(TimeSpan.Zero, TimeSpan.Zero);
        }
		/// <summary>
		/// Ejecuta el dispose del timer
		/// </summary>
        public void FinalizarTimer()
        {
            if (Instance.TimerProceso != null)
                Instance.TimerProceso.Dispose();
        }
		/// <summary>
		/// Toma el timer inicializarlo y vuelve a setear un tiempo para iniciar
		/// </summary>
		/// <param name="comenzarEnvioConTimer"></param>
        public void ReiniciarTimer(TimerCallback comenzarEnvioConTimer)
        {
            Instance.SetEstado(EstadoProceso.Iniciado);
            Instance.SetIndex(Instance.Index);
            Instance.TimerProceso = new Timer(comenzarEnvioConTimer);
            Instance.TimerProceso.Change(TimeSpan.Zero, TimeSpan.Zero);
            var indexGuardado = Instance.LeerIndex();
        }
		/// <summary>
		/// Lee el valor del index ubicado en App.Config
		/// </summary>
		/// <returns></returns>
		private int LeerIndex()
        {
            //TODO: Leer App.Config o donde sea que se haya guardado Instance.Index. Si falla lanzar excepcion
            return 0;
        }
		/// <summary>
		/// Guarda en App.Config el valor de index
		/// </summary>
		public void GuardarIndex()
        {
            //TODO: Guardar en App.Config o donde sea Instance.Index. Si falla lanzar excepcion
        }
		/// <summary>
		/// Set estado del proceso
		/// </summary>
		/// <param name="estado"></param>
        internal void SetEstado(EstadoProceso estado)
        {
            Instance.Estado = estado;
        }
		/// <summary>
		/// Sumador de la cantidad de mails
		/// </summary>
        internal void SumarCantidadMailsEnviadosHoy()
        {
            Instance.contadorPorDia++;
        }
		/// <summary>
		/// Obtiene la cantidad de mails enviados en el día
		/// </summary>
		/// <returns></returns>
        internal int ObtenerCantidadMailsEnviadosHoy()
        {
            return Instance.contadorPorDia;
        }
		/// <summary>
		/// Inicializa la cantidad de mails que se envian
		/// </summary>
        internal void InicializarCantidadEnviadosHoy()
        {
            Instance.contadorPorDia = 0;
        }
		/// <summary>
		/// Set tiempo de espera 24horas al timer de la clase
		/// </summary>
        internal void Esperar24Horas()
        {
            Instance.TimerProceso.Change(TimeSpan.FromHours(TIEMPO_ESPERA_24HS), TimeSpan.Zero);
        }
		/// <summary>
		/// Set tiempo de espera segundos random al timer de la clase
		/// </summary>
		internal void EsperarSegundos()
        {
            Instance.TimerProceso.Change(TimeSpan.FromSeconds(TIEMPO_ESPERA_SEGUNDOS), TimeSpan.Zero);
        }
		/// <summary>
		/// Elimina por completo el timer, liberando recursos, permitiendo iniciar de cero
		/// </summary>
        internal void EliminarTimer()
        {
            Instance.FinalizarTimer();
            Instance.TimerProceso = null;
        }
		/// <summary>
		/// Evalua si es valido continuar con el proceso
		/// </summary>
		/// <returns></returns>
        internal bool PausarProcesoPorHoy()
        {
            return Instance.ObtenerCantidadMailsEnviadosHoy() == CANTIDAD_MAILS_POR_DIA;
        }
    }

    public enum EstadoProceso { Iniciado, Procesando, Detenido, Pausado, FinalizadoOk, FinalizadoConError }
}