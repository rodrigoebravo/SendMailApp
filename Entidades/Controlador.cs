using System;
using System.Threading;

namespace Entidades
{
    public class Controlador
    {
        private int contadorPorDia { get; set; }
        private int Index;
        private Timer TimerProceso;
        public EstadoProceso Estado { get; private set; }

        #region Constantes
        public static int TIEMPO_ESPERA_SEGUNDOS { get { return new Random().Next(30, 120); } }
        public static int CANTIDAD_MAILS_POR_DIA = 50;
        public static double TIEMPO_ESPERA_24HS = 24;
        #endregion

        internal void SetIndex(int value)
        {
            Instance.Index = value;
        }

        internal int ObtenerIndex()
        {
            return Instance.Index;
        }

        internal EstadoProceso GetEstado()
        {
            return Instance.Estado;
        }

        #region Singleton
        private readonly static Controlador _instance = new Controlador();
        public static Controlador Instance { get { return _instance; } }
        #endregion

        private Controlador() { }

        public void InicializarControl(TimerCallback comenzarEnvioConTimer)
        {
            Instance.SetEstado(EstadoProceso.Iniciado);
            Instance.SetIndex(0);
            Instance.TimerProceso = new Timer(comenzarEnvioConTimer);
            Instance.TimerProceso.Change(TimeSpan.Zero, TimeSpan.Zero);
        }
        public void FinalizarTimer()
        {
            if (Instance.TimerProceso != null)
                Instance.TimerProceso.Dispose();
        }
        public void ReiniciarTimer(TimerCallback comenzarEnvioConTimer)
        {
            Instance.SetEstado(EstadoProceso.Iniciado);
            Instance.SetIndex(Instance.Index);
            Instance.TimerProceso = new Timer(comenzarEnvioConTimer);
            Instance.TimerProceso.Change(TimeSpan.Zero, TimeSpan.Zero);
            var indexGuardado = Instance.LeerIndex();
        }

        private int LeerIndex()
        {
            //TODO: Leer App.Config o donde sea que se haya guardado Instance.Index. Si falla lanzar excepcion
            return 0;
        }
        public void GuardarIndex()
        {
            //TODO: Guardar en App.Config o donde sea Instance.Index. Si falla lanzar excepcion
        }


        internal void SetEstado(EstadoProceso estado)
        {
            Instance.Estado = estado;
        }

        internal void SumarCantidadMailsEnviadosHoy()
        {
            Instance.contadorPorDia++;
        }
        internal int ObtenerCantidadMailsEnviadosHoy()
        {
            return Instance.contadorPorDia;
        }

        internal void InicializarCantidadEnviadosHoy()
        {
            Instance.contadorPorDia = 0;
        }

        internal void Esperar24Horas()
        {
            Instance.TimerProceso.Change(TimeSpan.FromHours(TIEMPO_ESPERA_24HS), TimeSpan.Zero);
        }

        internal void EsperarSegundos()
        {
            Instance.TimerProceso.Change(TimeSpan.FromSeconds(TIEMPO_ESPERA_SEGUNDOS), TimeSpan.Zero);
        }

        internal void EliminarTimer()
        {
            Instance.FinalizarTimer();
            Instance.TimerProceso = null;
        }

        internal bool PausarProcesoPorHoy()
        {
            return Instance.ObtenerCantidadMailsEnviadosHoy() == CANTIDAD_MAILS_POR_DIA;
        }
    }

    public enum EstadoProceso { Iniciado, Procesando, Detenido, Pausado, FinalizadoOk, FinalizadoConError }
}