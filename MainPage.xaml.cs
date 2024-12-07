using AppFirebase.Views;

namespace AppFirebase
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCrearProductoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateProductoPage());
        }

        private async void OnVerProductosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductsPage());
        }

        private void OnSalirClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

    }

}
