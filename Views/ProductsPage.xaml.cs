using AppFirebase.ViewModels;

namespace AppFirebase.Views;

public partial class ProductsPage : ContentPage
{
	private readonly ProductoViewModel _productoViewModel;
	public ProductsPage()
	{
		InitializeComponent();
		_productoViewModel = new ProductoViewModel();
		BindingContext = _productoViewModel;

	}
}