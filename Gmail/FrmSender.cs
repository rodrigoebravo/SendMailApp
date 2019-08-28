using System;
using System.Windows.Forms;
using System.IO;
using Entidades;

namespace Gmail
{
	public partial class FrmSender : Form
	{
		private ManejadorEmails manejador;
		public FrmSender()
		{
			InitializeComponent();
			this.txtContraseña.UseSystemPasswordChar = true;

			manejador = new ManejadorEmails();
			CargaChk();
		}
		public void CargaChk()
		{
			string path = Environment.CurrentDirectory + "\\base_Xml.xml";
			if (Directory.Exists(path))
				return;
			try
			{
				manejador.Emails = new Xml().Abrir(path);
				foreach (var item in manejador.Emails)
				{
					item.Enviado = false;
					item.HoraProceso = null;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			foreach (Email email in manejador.Emails)
			{
				if (email.Avaible != 0)
				{
					chkEmails.Items.Add(email);
					manejador.Cant++;
				}
			}
		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			FrmAgregar frm = new FrmAgregar(manejador);
			frm.Show();
			frm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ActualizarChk);
		}

		private void ActualizarChk(object sender, EventArgs e)
		{
			chkEmails.Items.Clear();
			foreach (Email email in manejador.Emails)
			{
				if (email.Avaible != 0)
					chkEmails.Items.Add(email);
			}
		}

		private void BrnEliminar_Click(object sender, EventArgs e)
		{
			if (chkEmails.CheckedItems.Count == 0)
				MessageBox.Show("Debe seleccionar un elemento");
			else
			{
				foreach (Email email in chkEmails.CheckedItems)
				{
					if (manejador - email)
						MessageBox.Show(email.DireccionEmail + " --Eliminado--");
				}
				ActualizarChk(sender, e);
			}
		}

		private void BtnSelectAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < manejador.Cant; i++)
			{
				chkEmails.SetItemChecked(i, true);
			}
		}

		private void BtnCancelAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < manejador.Cant; i++)
			{
				chkEmails.SetItemChecked(i, false);
			}
		}

		private void FrmSender_FormClosing(object sender, FormClosingEventArgs e)
		{
			string path = Environment.CurrentDirectory + "\\base_Xml.xml";
			new Xml().Guardar(manejador.Emails, path);
		}

		private void btnIniciar_Click(object sender, EventArgs e)
		{
			if (this.manejador.Estado == EstadoProceso.Detenido)
				return;
			if (this.btnIniciar.Text.Equals("Pausar"))
			{
				this.manejador.PausarProceso();
				this.btnIniciar.Text = "Reiniciar";
				return;
			}
			if (this.btnIniciar.Text.Equals("Reiniciar"))
			{
				this.manejador.ReiniciarProceso();
				this.btnIniciar.Text = "Pausar";
				return;
			}
			if (this.manejador.Cant > 0)
			{
				//this.tmrProceso.Enabled = true;
				this.manejador.Comenzar();
				this.btnIniciar.Text = "Pausar";
				return;
			}
		}

		private void HabilitarControles()
		{
			this.btnAdd.Enabled = true;
			this.btnCancelAll.Enabled = true;
			this.btnConectar.Enabled = true;
			this.btnDesconectar.Enabled = true;
			this.btnDetener.Enabled = true;
			this.btnSelectAll.Enabled = true;
			this.brnEliminar.Enabled = true;
		}

		private void DeshabilitarControles()
		{
			this.btnAdd.Enabled = false;
			this.btnCancelAll.Enabled = false;
			this.btnConectar.Enabled = false;
			this.btnDesconectar.Enabled = false;
			this.btnDetener.Enabled = true;
			this.btnSelectAll.Enabled = false;
			this.brnEliminar.Enabled = false;
		}

		private void tmrProceso_Tick(object sender, EventArgs e)
		{
			if (this.manejador.Estado == EstadoProceso.Procesando)
			{
				DeshabilitarControles();
				richLog.Text = string.Empty;
				if (ManejadorEmails.EmailsAux != null)
					foreach (var item in ManejadorEmails.EmailsAux)
					{
						if (item.Enviado)
							richLog.AppendText(item.ToString() + Environment.NewLine);
					}
			}
			else
			{
				HabilitarControles();
				//this.tmrProceso.Enabled = false;
			}
		}

		private void btnDetener_Click(object sender, EventArgs e)
		{
			this.manejador.DetenerProceso();
		}

		private void txtContraseña_MouseClick(object sender, MouseEventArgs e)
		{
			this.txtContraseña.UseSystemPasswordChar = !this.txtContraseña.UseSystemPasswordChar;
		}
	}

}
