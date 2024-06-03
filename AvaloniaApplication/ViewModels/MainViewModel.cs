using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvaloniaApplication.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Person> People { get; }

    [ObservableProperty]
    private DataGridCollectionView _dataGridObjects;

    private DispatcherTimer _disTimer = new DispatcherTimer();

    public MainViewModel()
    {
        var people = new List<Person>
        {
            new Person("Neil", "Armstrong"),
            new Person("Buzz", "Lightyear"),
            new Person("James", "Kirk"),
            new Person("Fem", "Fem", false)
        };

        People = new ObservableCollection<Person>(people);

        DataGridObjects = new DataGridCollectionView(People);

        DataGridObjects.Filter = item => ((Person)item).Male == true;

        _disTimer.Interval = TimeSpan.FromSeconds(1);
        _disTimer.Tick += _disTimer_Tick; ;
        _disTimer.Start();
    }

    private void _disTimer_Tick(object? sender, EventArgs e)
    {
        People.Add(new Person("test", "test", false));
    }
}

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Male { get; set; } = true;

    public Person(string firstName, string lastName, bool male = true)
    {
        FirstName = firstName;
        LastName = lastName;
        Male = male;
    }
}