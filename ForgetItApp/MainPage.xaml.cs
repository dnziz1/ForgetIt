using System.Threading.Tasks;

namespace ForgetItApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async Task OnCreateTripBtnClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddTripPage));
    }
}
