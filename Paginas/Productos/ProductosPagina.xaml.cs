using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Herramientas_Factoria.Paginas.Productos
{
    public partial class ProductosPagina : Window
    {
        public ObservableCollection<Producto> Productos { get; set; }
        public ObservableCollection<Producto> ProductosFiltrados { get; set; }

        public ProductosPagina()
        {
            InitializeComponent();
            Productos = new ObservableCollection<Producto>();
            ProductosFiltrados = new ObservableCollection<Producto>();
            DataContext = this;

            CargarProductos();
            FiltrarProductos(string.Empty);
        }
        private void Button_Volver(object sender, RoutedEventArgs e)
        {

            // Volvemos a la pagina princial
            Index index = new Index();
            index.Show();

            // Cerrar la ventana actual
            this.Close();

        }


        private void CargarProductos()
        {
            try
            {
                // Obtiene el directorio donde se encuentra la aplicación
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                // Construye la ruta completa al archivo Productos.json

                Console.WriteLine(appDirectory);
                string jsonPath = Path.Combine(appDirectory, "Recursos\\Productos.json");
                Console.WriteLine(jsonPath);
                if (File.Exists(jsonPath))
                {
                    string jsonContent = File.ReadAllText(jsonPath);
                    Console.WriteLine(jsonContent);
                    var productos = JsonConvert.DeserializeObject<ObservableCollection<Producto>>(jsonContent);
                    foreach (var producto in productos)
                    {
                        Productos.Add(producto);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró el archivo Productos.json.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void FiltrarProductos(string filtro)
        {
            ProductosFiltrados.Clear();
            var productosFiltrados = Productos.Where(p =>
                p.NIIN.ToLower().Contains(filtro) ||
                p.Descripcion.ToLower().Contains(filtro) ||
                p.Nombre.ToLower().Contains(filtro));

            foreach (var producto in productosFiltrados)
            {
                ProductosFiltrados.Add(producto);
            }

            ProductsListBox.ItemsSource = ProductosFiltrados;
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarProductos(FilterTextBox.Text.ToLower());
        }

    }

    public class Producto
    {
        public string NIIN { get; set; }

        [JsonProperty("Descripción")]
        public string Descripcion { get; set; }

        public string Nombre { get; set; }

        [JsonProperty("Cód. U.Entrega")]
        public string CodUEntrega { get; set; }

        public double Precio { get; set; }

        public ICommand CopyCommand { get; }

        public Producto()
        {
            CopyCommand = new RelayCommand(CopiarNIIN);
        }

        private void CopiarNIIN(object parameter)
        {
            Clipboard.SetText(NIIN);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

    }
}
