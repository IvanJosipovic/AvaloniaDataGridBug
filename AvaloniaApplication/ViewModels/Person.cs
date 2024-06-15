using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication.ViewModels;

public partial class Person : ObservableObject
{
    [ObservableProperty]
    private string _firstName;

    [ObservableProperty]
    public string _lastName;

    [ObservableProperty]
    public bool _male;

    public Person(string firstName, string lastName, bool male = true)
    {
        FirstName = firstName;
        LastName = lastName;
        Male = male;
    }
}