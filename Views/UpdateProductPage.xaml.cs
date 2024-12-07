using AppFirebase.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace AppFirebase.Views;

public partial class UpdateProductPage : ContentPage
{
	public UpdateProductPage(ProductoItemViewModel producto)
	{
		InitializeComponent();
		var viewModel = new UpdateProductoViewModel(producto);
		BindingContext = viewModel;
		viewModel.LimpiarImagen += OnLimpiarImagen;
	}

	private async void OnImageTapped(object sender, EventArgs e)
	{
		await CrossMedia.Current.Initialize();

		if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
		{
			await DisplayAlert("Cámara", "Cámara no disponible", "Ok");
			return;
		}

		var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
		{
			Directory = "Sample",
			Name = $"{DateTime.UtcNow},jpg"
		});

		if (file == null)
			return;

		if(BindingContext is UpdateProductoViewModel viewModel)
		{
			viewModel.Producto.Foto = file.Path;
			imagen.Source = ImageSource.FromFile(file.Path);
		}
	}

	private void OnLimpiarImagen()
	{
        imagen.Source = "smiley.png";
    }
}