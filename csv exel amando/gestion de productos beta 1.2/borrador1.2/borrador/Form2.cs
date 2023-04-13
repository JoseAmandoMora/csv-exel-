using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;//importante

namespace borrador
{
    public partial class Form2 : Form
    {
        public static List<Productos> productos = new List<Productos>();

        public int n = 0;
        public class Productos
        {
            public string _codigo { get; set; }
            public string _nombre { get; set; }
            public double _precio { get; set; }

            public Productos(string codigo, string nombre, double precio)
            {
                _codigo = codigo;
                _nombre = nombre;
                _precio = precio;
            }
        }

        public Form2()
        {
            InitializeComponent();
            lblform.Text = " ";
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {


            try
            {

                string codigo = (productos.Count() + 1).ToString().PadLeft(5, '0');
                txtCodigo.Text = codigo;
                string nombre = txtNombre.Text;
                double precio = Convert.ToDouble(txtPrecio.Text);

                if (precio == 0)
                {
                    MessageBox.Show("El nombre del producto no puede ser vacio!");
                    //throw new Exception("El precio no puede ser 0");
                }
                else if (precio < 0)
                {
                    MessageBox.Show("El nombre del producto no puede ser vacio!");
                    //throw new Exception("El precio no puede ser negativo");
                }

                if (txtPrecio.Text == null)
                {
                    MessageBox.Show("El nombre del producto no puede ser vacio!");
                    //throw new Exception("el precio no puede ser vacio");
                }


                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("El nombre del producto no puede ser vacio!");
                    //throw new Exception("El nombre del producto no puede ser vacio!");
                }


                int n = dtgvProductos.Rows.Add();
                Productos prod = new Productos(codigo, nombre, precio);
                productos.Add(prod);
                dtgvProductos.Rows[n].Cells[0].Value = codigo;
                dtgvProductos.Rows[n].Cells[1].Value = nombre;
                dtgvProductos.Rows[n].Cells[2].Value = precio + "$";
                GrabarDatos();
                /*txtCodigo.Clear();*/
                txtPrecio.Clear();
                txtNombre.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }



        private void dtgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            n = e.RowIndex;

            if (n != -1)
            {
                lblform.Text = " ";
            }
        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (n != -1 && dtgvProductos.Rows.Count > 0)
                {
                    dtgvProductos.Rows.RemoveAt(n);

                }
                else
                {
                    throw new Exception("No se pueden eliminar más artículos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GrabarBorrado();
        }

        private void GrabarBorrado()
        {
            StreamWriter archivo = new StreamWriter("Productos.txt");
            for (int x = 0; x < dtgvProductos.Rows.Count; x++)
            {
                archivo.WriteLine(dtgvProductos.Rows[x].Cells[0].Value/*ToString()*/);
                archivo.WriteLine(dtgvProductos.Rows[x].Cells[1].Value/*ToString(*/);
                archivo.WriteLine(dtgvProductos.Rows[x].Cells[2].Value/*ToString()*/);
            }
            archivo.Close();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Productos Importados");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Productos Exportados");
        }

        private void Form2_Load(object sender, EventArgs e)
        {//using systen.io
            if (!File.Exists("Productos.txt")) //compruebo que este el documento 
            {
                StreamWriter archivo = new StreamWriter("Productos.txt");
                archivo.Close();
            }
            else
            {
                StreamReader archivo = new StreamReader("Productos.txt");
                while (!archivo.EndOfStream)
                {
                    string codigo = archivo.ReadLine();
                    string nombre = archivo.ReadLine();
                    string precio = archivo.ReadLine();
                    dtgvProductos.Rows.Add(codigo, nombre, precio);
                }
                archivo.Close();
            }
        }
        private void GrabarDatos()
        {
            StreamWriter archivo = new StreamWriter("Productos.txt", true);
            archivo.WriteLine(txtCodigo.Text);
            archivo.WriteLine(txtNombre.Text);
            archivo.WriteLine(txtPrecio.Text + "$");
            archivo.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ruta_aplicacion = Path.GetDirectoryName(Application.ExecutablePath);
            
            //InformacionGeneral info = new InformacionGeneral();
            //info.nombre = "Pedro";
            
            //string ruta_archivo = Path.Combine(ruta_aplicacion, "info.json");

            //string json_info = JsonConvert.SerializeObject(info);
            //File.WriteAllText(ruta_archivo, json_info);

            //InformacionGeneral info_cargada = JsonConvert.DeserializeObject<InformacionGeneral>(File.ReadAllText(ruta_archivo));

            //MessageBox.Show(info_cargada.nombre);

            string ruta_csv = Path.Combine(ruta_aplicacion, "prueba.csv");

            string[] csv_contenido = File.ReadAllLines(ruta_csv);

            for (int i = 0; i < csv_contenido.Length; i++)
            {
                if (i == 0) continue;

                string linea = csv_contenido[i];
                string[] contenido = linea.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                MessageBox.Show($"codigo: {contenido[0]} nombre: {contenido[1]} precio {contenido[2]}");
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("003;");
            builder.Append("papel;");
            builder.Append("400");
            builder.AppendLine();

            MessageBox.Show(builder.ToString());
        }
    }
}

