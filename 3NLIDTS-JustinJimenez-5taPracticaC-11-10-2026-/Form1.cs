using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3NLIDTS_JustinJimenez_5taPracticaC_11_10_2026_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            tbNombre.TextChanged += ValidarNombre;
            tbApellidos.TextChanged += ValidarApellido;
            tbEdad.TextChanged += ValidarEdad;
            tbEstatura.TextChanged += ValidarEstatura;
            tbTelefono.Leave += ValidarTelefono;

        }

        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!TelefonoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un telefono valido", "Error Telefono", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.BackColor = System.Drawing.Color.IndianRed;
            }
            else
            {
                textboxLocal.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsDecimalValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba una estatura valida", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textLocal = (TextBox)sender;
            if (!EsEnteroValido(textLocal.Text))
            {
                MessageBox.Show("Escriba una edad valida", "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textLocal.Clear();
            }
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un nombre valido", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un apellido valido", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }





        private bool EsTextoValido(string Valor)
        {
            return Regex.IsMatch(Valor, @"^[a-zA-Z\s]+$");
        }

        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }

        private bool TelefonoValido(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombres = tbNombre.Text;
            string apellidos = tbApellidos.Text;
            string tel = tbTelefono.Text;
            string edad = tbEdad.Text;
            string estatura = tbEstatura.Text;
            string genero = "";

            if (rbFemenino.Checked)
            {
                genero = "Femenino";
            }
            else if (rbMasculino.Checked)
            {
                genero = "Masculino";
            }
            else if (rbotro.Checked)
            {
                genero = "Otros";
            }

            string Mensaje = $"Nombre: {nombres}\nApellidos: {apellidos}\nTelefono: {tel}\n" +
                             $"Edad: {edad}\nEstatura: {estatura}\nGenero: {genero}";

            string rutaArchivo = "C:\\Users\\omnib\\OneDrive\\Documents\\3NAgoDic26.txt";

            bool ArchivoExiste = File.Exists(rutaArchivo);

            using (StreamWriter escritor = new StreamWriter(rutaArchivo, true))
            {
                if (ArchivoExiste)
                {
                    escritor.WriteLine();
                }
                escritor.WriteLine(Mensaje);
            }

            MessageBox.Show(Mensaje, "Confirmación de Registro", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tbApellidos.Clear();
            tbNombre.Clear();
            tbEdad.Clear();
            tbEstatura.Clear();
            tbTelefono.Clear();
            tbTelefono.BackColor = System.Drawing.Color.White;
            rbFemenino.Checked = false;
            rbMasculino.Checked = false;
            rbotro.Checked = false;
        }
    }
}
