using ForgetItApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgetItApp.ViewModels;
/// <summary>
/// 
/// ViewModel for the main page, managing a collection of trips.
/// <!-- This ViewModel serves as the data context for the MainPage, providing-->
/// </summary>
public class MainViewModel : BaseViewModel
{
    public ObservableCollection<Trip> Trips { get; set; } = new();

    public MainViewModel()
    {
        // Sample data for testing
        Trips.Add(new Trip { Name = "Business Trip to London", DepartureDateTime = DateTime.Now.AddDays(1) });
        Trips.Add(new Trip { Name = "Vacation in Hawaii", DepartureDateTime = DateTime.Now.AddDays(6) });
    }
}
