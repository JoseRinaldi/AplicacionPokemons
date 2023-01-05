using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace AppPokemon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<Pokemon> listaPokemon;

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            //PokemonNegocio negocio = new PokemonNegocio();
            //try
            //{
            //    listaPokemon = negocio.listar();
            //    dgvPokemons.DataSource = listaPokemon;
            //    dgvPokemons.Columns["UrlImagen"].Visible = false;
            //    cargarImagen(listaPokemon[0].UrlImagen);

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.ToString());
            //}
            cboCampo.Items.Add("Numero");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
            
        }

        private void cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                ocultarColumnas();
                cargarImagen(listaPokemon[0].UrlImagen);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void ocultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;

        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvPokemons.CurrentRow != null)
            {
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pcbImagen.Load(imagen);
            }
            catch (Exception ex)
            {
                pcbImagen.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAltaPokemon alta = new FrmAltaPokemon();
            alta.ShowDialog();
            cargar();

        }

        private void btnModifocar_Click(object sender, EventArgs e)
        {
            Pokemon selecionado;
            selecionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

            FrmAltaPokemon modificar = new FrmAltaPokemon(selecionado);
            modificar.ShowDialog();
            cargar();

        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
          
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logico = false)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;

            try
            {
                DialogResult resultado = MessageBox.Show("¿ Esta seguro de eliminar este archivo ?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    
                    if(logico)
                        negocio.eliminarlogico(seleccionado.Id);
                    else
                        negocio.eliminar(seleccionado.Id);
                    
                    cargar();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



        }

        private bool validar_filtro()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, ingrese el campo para filtrar");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, ingrese el criterio para filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Numero")
            {
                if (string.IsNullOrEmpty(cboFiltro.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numericos...");
                    return true;

                }
                if (!(Solo_numeros(cboFiltro.Text)))
                {
                    MessageBox.Show("Solo N° para filtrar por una campo numerico...");
                    return true;    

                }
            }
            
            return false;
        }

        private bool Solo_numeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)//aqui se configura el evento del boton burcar del filtro
        {
            //List<Pokemon> listaFiltrada;
            //string filtro = txtFiltro.Text;


            //if(filtro != "")
            //{

            //    //listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper() == filtro.ToUpper());//findall necesita ciertos parametros los cuales se dan con una exprecion lambda la cual se ve entre los parentesis//
            //    // la x q esta en la expresion lambda puede tener cualquier nombre en este caso se llamo x//
            //    listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            //    //la prop contains permite buscar cadenas q contengan coincidencias de texto aunque no se ingrese toda la palabra//
            //    //toUpper o toLower pasan toda la cadena a mayus. o minus.//
            //}
            //else
            //{
            //    listaFiltrada = listaPokemon;
            //}

            //dgvPokemons.DataSource = null;// para cargar la nueva listaa en dgv debemos limpiarla ante x eso se lleva la dgv a null en este paso//
            //dgvPokemons.DataSource = listaFiltrada;// y aqui cargamos la lista filtrada//
            //ocultarColumnas();
            PokemonNegocio negocio = new PokemonNegocio();


            try
            {
                if(validar_filtro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = cboFiltro.Text;
                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)//aqui se configura el evento buscar automatico sin la necesidad de usar el boton buscar//
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltro.Text;


            if (filtro != "")//se puede cambiar la condicion para que busque x ejemplo cuando se ingresen 3 caracteres (filtro.lenght => 3) asi seria la sintaxis//
            {

               
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
                
            }
            else
            {
                listaFiltrada = listaPokemon;
            }

            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumnas();

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Numero")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");

            }
        }
    }
}
