using AppFirebase.Models;
using AppFirebase.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppFirebase.ViewModels
{
    public class UpdateProductoViewModel : INotifyPropertyChanged
    {
        private readonly ServiceProducto _serviceProducto;
        private Producto _producto;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action LimpiarImagen;

        public Producto Producto
        {
            get { return _producto; }
            set
            {
                _producto = value;
                OnPropertyChanged();
            }
        }

        public ICommand ActualizarProductoCommand { get; }

        public UpdateProductoViewModel(Producto producto) 
        {
            _serviceProducto = new ServiceProducto();
            Producto = producto;
            ActualizarProductoCommand = new Command(async () => await ActualizarProducto());
        }

        private async Task ActualizarProducto()
        {
            if (Producto == null || string.IsNullOrEmpty(Producto.Id)) return;
            await _serviceProducto.UpdateProducto(Producto.Id, new
            {
                Producto.Nombre,
                Producto.Descripción,
                Producto.Precio,
                Producto.Foto
            });

            LimpiarImagen?.Invoke();
            await Application.Current.MainPage.DisplayAlert("Éxito", "Producto actualizado con éxito", "Ok");
            MessagingCenter.Send(this, "Producto Actualizado");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
