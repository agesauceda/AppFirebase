using AppFirebase.Models;
using AppFirebase.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppFirebase.ViewModels
{
    public class CreateProductoViewModel : INotifyPropertyChanged
    {
        private readonly ServiceProducto _serviceProducto;
        private Producto _producto;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action LimpiarImagen;

        public Producto Producto { 
            get { return _producto;  }
            set 
            { 
                _producto = value;
                OnPropertyChanged(nameof(Producto));
            }
        }

        public ICommand CrearProductoCommand { get; }

        public CreateProductoViewModel() 
        { 
            _serviceProducto = new ServiceProducto();
            Producto = new Producto();
            CrearProductoCommand = new Command(async () => await CrearProducto());
        }

        private async Task CrearProducto()
        {
            Debug.WriteLine($"Crear Producto");
            try
            {
                if (Producto == null)
                {
                    Debug.WriteLine("Producto nulo");
                    throw new ArgumentNullException(nameof(Producto));
                }

                if (string.IsNullOrWhiteSpace(Producto.Nombre) ||
                    string.IsNullOrWhiteSpace(Producto.Descripción) ||
                    Producto.Precio == null ||
                    string.IsNullOrWhiteSpace(Producto.Foto))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "Ok");
                    return;
                }

                Debug.WriteLine("Todos los campos están completos.");
                await _serviceProducto.CreateProducto(Producto);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Producto creado con éxito.", "Ok");

                Producto = new Producto();
                OnPropertyChanged(nameof(Producto));

                LimpiarImagen?.Invoke();

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {

            }
            Debug.WriteLine("CrearProducto: Fin del método.");

        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
