using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;

namespace AppPokemon
{
    public partial class FrmAltaPokemon : Form
    {
        private Pokemon Pokemon = null;

        private OpenFileDialog archivo = null;

        public FrmAltaPokemon()
        {
            InitializeComponent();
        }
        public FrmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.Pokemon = pokemon;
            Text = "Modificar Pokemon";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Pokemon poke = new Pokemon();
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                if (Pokemon == null)
                    Pokemon = new Pokemon();

                Pokemon.Numero = int.Parse(txtNumero.Text);
                Pokemon.Nombre = txtNombre.Text;
                Pokemon.Descripcion = txtDescripcion.Text;
                Pokemon.UrlImagen = txtUrlImagen.Text;
                Pokemon.Tipo = (Elemento)cbxTipo.SelectedItem;
                Pokemon.Debilidad = (Elemento)cbxDebilidad.SelectedItem;

                if(Pokemon.Id != 0)
                {

                    negocio.modificar(Pokemon);
                    MessageBox.Show("Modificado Correctamente");
                }
                else
                {
                    negocio.agregar(Pokemon);
                    MessageBox.Show("Agregado Correctamente");

                }

                //guardo la imagen si la levanto localmente
                if(archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))//si uso toUpper debo fijarme q el contains este en mayusculas 
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                }
                
                Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();

            try
            {
                cbxTipo.DataSource = elementoNegocio.listar();
                cbxTipo.ValueMember = "Id";//aqui le doy un valor clave a buscar//
                cbxTipo.DisplayMember = "Descripcion";//aqui le digo lo q quiero q me muestre//
                cbxDebilidad.DataSource = elementoNegocio.listar();
                cbxDebilidad.ValueMember = "Id";
                cbxDebilidad.DisplayMember = "Descripcion";

                if (Pokemon != null)//precargamos los datos al momento de modificar//
                {
                    txtNumero.Text = Pokemon.Numero.ToString();
                    txtNombre.Text = Pokemon.Nombre;
                    txtDescripcion.Text = Pokemon.Descripcion;
                    txtUrlImagen.Text = Pokemon.UrlImagen;
                    cargarImagen(Pokemon.UrlImagen);
                    cbxTipo.SelectedValue = Pokemon.Tipo.Id;
                    cbxDebilidad.SelectedValue = Pokemon.Debilidad.Id;//aqui termino de completar la carga de los desplegables//

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pcbPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pcbPokemon.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                //guardar la imagen

                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName );
            }
        }
    }
}
