using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Swordfish.NET.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaApplication.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Person> People { get; }

    public ConcurrentObservableDictionary<string, Person> People2 { get; } = [];

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

        People2 = new ConcurrentObservableDictionary<string, Person>()
        {
            new KeyValuePair<string, Person>("pp1", new Person("pp1", "pp1")),
            new KeyValuePair<string, Person>("pp2", new Person("pp2", "pp1")),
            new KeyValuePair<string, Person>("pp3", new Person("pp3", "pp1")),
            new KeyValuePair<string, Person>("pp4", new Person("pp4", "pp1", false)),
        };

        DataGridObjects = new DataGridCollectionView(People2);

        DataGridObjects.Filter = item => ((KeyValuePair<string, Person>)item).Value.Male == false;

        _disTimer.Interval = TimeSpan.FromSeconds(1);
        _disTimer.Tick += _disTimer_Tick; ;
        _disTimer.Start();
    }

    private void _disTimer_Tick(object? sender, EventArgs e)
    {
        var rand = RandomString(5);

        People2.Add(new KeyValuePair<string, Person>(rand, new Person(rand, rand, true)));

        //People2.RemoveAt(0);
    }


    private static Random random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
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