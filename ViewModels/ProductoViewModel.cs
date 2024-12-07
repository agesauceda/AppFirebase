using AppFirebase.Models;
using AppFirebase.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppFirebase.ViewModels
{
    public class ProductoViewModel : INotifyPropertyChanged
    {
        private readonly ServiceProducto _serviceProducto;
        private ObservableCollection<ProductoItemViewModel> _productos;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ProductoItemViewModel> Productos
        {
            get { return _productos; }
            set { _productos = value ?? new ObservableCollection<ProductoItemViewModel>();
                OnPropertyChanged();
            }
        }

        public ICommand EliminarProductoCommand { get; }
        public ICommand ActualizarProductoCommand { get; }

        public ProductoViewModel()
        {
            _serviceProducto = new ServiceProducto();
            Productos = new ObservableCollection<ProductoItemViewModel>();
            EliminarProductoCommand = new Command<ProductoItemViewModel>(async (producto) => await DeleteProducto(producto));
            ActualizarProductoCommand = new Command<ProductoItemViewModel>(async (producto) => await UpdateProducto(producto));
            LoadProductos();

            MessagingCenter.Subscribe<UpdateProductoViewModel>(this, "Producto Actualizado", async (sender) => await LoadProductos());
        }

        private async Task LoadProductos()
        {
            try
            {
                var productos = await _serviceProducto.GetProductos();
                var productosViewModel = productos.Select(x => new ProductoItemViewModel(x)).ToList();
                Productos = new ObservableCollection<ProductoItemViewModel>(productosViewModel ?? new List<ProductoItemViewModel>());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al recolectar productos: {ex.Message}");
                Productos = new ObservableCollection<ProductoItemViewModel>();
            }
        }

        private async Task DeleteProducto(ProductoItemViewModel producto)
        {
            if (producto == null || string.IsNullOrEmpty(producto.Id)) return;

            bool confirm = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Confirmación", "¿Desea eliminar el producto?", "Si", "No");
            if (confirm)
            {
                await _serviceProducto.DeleteProducto(producto.Id);
                Productos.Remove(producto);
            }
        }

        private async Task UpdateProducto(ProductoItemViewModel producto)
        {
            if (producto == null || string.IsNullOrEmpty(producto.Id)) return;
            await Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PushAsync(new Views.UpdateProductPage (producto));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ProductoItemViewModel : Producto 
    { 
        public ProductoItemViewModel(Producto producto) 
        {   
            Id = producto.Id;
            Nombre = producto.Nombre;
            Descripción = producto.Descripción;
            Precio = producto.Precio;
            Foto = producto.Foto;
        }

        public ImageSource ImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(Foto))
                {
                    return null;
                }
                try
                {
                    return ImageSource.FromFile(Foto);
                }
                catch (Exception ex) 
                {
                    Debug.WriteLine($"Error al cargar la imagen desde la ruta: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
