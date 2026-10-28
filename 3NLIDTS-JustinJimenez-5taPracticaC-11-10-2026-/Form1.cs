using MySql.Data.MySqlClient; // Libreria para la conexión y uso de MySQL
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // Libreria para lectura de archivos de TXT
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions; // Libreria para evaluaciones de estrucuturas de texto
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _3NLIDTS_JustinJimenez_5taPracticaC_11_10_2026_
{
    public partial class Form1 : Form
    {
        // string SQLConection = "Server=localhost; Port=3306; Database=programacion; Uid=root; Pwd=;";
        string SQLConection = "Server=localhost; Port=3306; Database=programacion; Uid=root; Pwd=admin;";

        public Form1()
        {
            InitializeComponent();

            // Declaracion de los manejadores de eventos
            tbEdad.TextChanged += ValidarEdad;
            tbApellidos.TextChanged += ValidarApellidos;
            tbNombre.TextChanged += ValidarNombre;
            tbEstatura.TextChanged += ValidarEstatura;
            tbTelefono.Leave += ValidarTelefono;
        }

        private void insertarRegistros(string nombre, string apellidos, int edad, decimal estatura, string telefono, string genero)
        {
            using (MySqlConnection conectar = new MySqlConnection(SQLConection))
            {
                conectar.Open();
                string insertQuery = "INSERT INTO registros (nombre, apellidos, telefono, estatura, edad, genero) " +
                                     "VALUES (@nombre, @apellidos, @telefono, @estatura, @edad, @genero)";

                using (MySqlCommand comando = new MySqlCommand(insertQuery, conectar))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@apellidos", apellidos);
                    comando.Parameters.AddWithValue("@telefono", telefono);
                    comando.Parameters.AddWithValue("@estatura", estatura);
                    comando.Parameters.AddWithValue("@edad", edad);
                    comando.Parameters.AddWithValue("@genero", genero);
                    comando.ExecuteNonQuery();
                }
                conectar.Close();
            }
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox cajaNombre = (TextBox)sender;
            if (!EsTextoValido(cajaNombre.Text))
            {
                MessageBox.Show("Ingrese valores correctos para el Nombre", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cajaNombre.Clear();
            }
        }

        private void ValidarApellidos(object sender, EventArgs e)
        {
            TextBox cajaApellidos = (TextBox)sender;
            if (!EsTextoValido(cajaApellidos.Text))
            {
                MessageBox.Show("Ingrese valores correctos para el apellido", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cajaApellidos.Clear();
            }
        }

        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$");
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textboxEdad = (TextBox)sender;
            if (!EsEnteroValido(textboxEdad.Text))
            {
                MessageBox.Show("Ingrese valores correctos para la edad", "Error edad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxEdad.Clear();
            }
        }

        private bool EsEnteroValido(String valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textBoxEstatura = (TextBox)sender;
            if (!EsFlotanteValido(textBoxEstatura.Text))
            {
                MessageBox.Show("Ingrese valores correctos para la estatura", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEstatura.Clear();
            }
        }

        private bool EsFlotanteValido(String valor)
        {
            float resultado;
            return float.TryParse(valor, out resultado);
        }

        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string input = textBox.Text;
            if (input.Length > 10)
            {
                if (!EsEnteroValidoDe10Digitos(input))
                {
                    textBox.BackColor = Color.Red;
                }
            }
            else if (!EsEnteroValidoDe10Digitos(input))
            {
                textBox.BackColor = Color.Yellow;
            }
            else
            {
                textBox.BackColor = Color.SeaGreen;
            }
        }

        private bool EsEnteroValidoDe10Digitos(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = tbNombre.Text;
            string apellidos = tbApellidos.Text;
            string edad = tbEdad.Text;
            string estatura = tbEstatura.Text;
            string telefono = tbTelefono.Text;
            string genero = "";

            if (rbMasculino.Checked)
            {
                genero = "Masculino";
            }
            else if (rbFemenino.Checked)
            {
                genero = "Femenino";
            }

            // Validar que los campos tengan el formato correcto
            if (EsEnteroValido(edad) && EsFlotanteValido(estatura) && EsEnteroValidoDe10Digitos(telefono) && EsTextoValido(nombre) && EsTextoValido(apellidos))
            {
                // Crear una cadena con los datos.
                string datos = $"Nombres: {nombre}\r\nApellidos: {apellidos}\r\nTeléfono: {telefono} " +
                               $"kg\r\nEstatura: {estatura} cm\r\nEdad: {edad} años\r\nGénero: {genero}\r\n";

                // Guardar los datos en un archivo de texto
                string rutaArchivo = "C:\\Users\\omnib\\OneDrive\\Documents\\3NAgoDic26.txt";
                bool archivoExiste = File.Exists(rutaArchivo);

                if (archivoExiste == false)
                {
                    File.WriteAllText(rutaArchivo, datos);
                }
                else
                {
                    // Verificar si el archivo ya existe
                    using (StreamWriter writer = new StreamWriter(rutaArchivo, true))
                    {
                        if (archivoExiste)
                        {
                            // Si el archivo existe, añadir un separador antes del nuevo registro
                            writer.WriteLine();
                        }

                        writer.WriteLine(datos);

                        // string nombre, string apellidos, int edad, decimal estatura, string telefono, string genero) {
                        insertarRegistros(nombre, apellidos, int.Parse(edad), decimal.Parse(estatura), telefono, genero);

                        MessageBox.Show("Datos insertados en la Base de Datos: \n\n" + datos, "Información BD", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Mostrar un mensaje con los datos capturados
                //MessageBox.Show("Datos guardados con éxito:\n\n" + datos, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, ingrese datos válidos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            tbApellidos.Clear();
            tbNombre.Clear();
            tbEstatura.Clear();
            tbEdad.Clear();
            tbTelefono.Clear();
            rbFemenino.Checked = false;
            rbMasculino.Checked = false;
        }
    }
}