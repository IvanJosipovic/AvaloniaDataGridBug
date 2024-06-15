using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using KubeUI.Client.Informer;
using Swordfish.NET.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace AvaloniaApplication.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    //public ObservableCollection<Person> People { get; }

    public ConcurrentObservableDictionary<NamespacedName, Person> People2 { get; } = [];

    [ObservableProperty]
    private DataGridCollectionView _dataGridObjects;

    private DispatcherTimer _disTimer = new DispatcherTimer();

    private int count;

    public MainViewModel()
    {
        //var people = new List<Person>
        //{
        //    new Person("Neil", "Armstrong"),
        //    new Person("Buzz", "Lightyear"),
        //    new Person("James", "Kirk"),
        //    new Person("Fem", "Fem", false)
        //};

        //People = new ObservableCollection<Person>(people);

        People2 = new ConcurrentObservableDictionary<NamespacedName, Person>()
        {
            new KeyValuePair<NamespacedName, Person>(new NamespacedName("pp0"), new Person("pp0", "pp0")),
            new KeyValuePair<NamespacedName, Person>(new NamespacedName("pp1"), new Person("pp1", "pp1")),
            new KeyValuePair<NamespacedName, Person>(new NamespacedName("pp2"), new Person("pp2", "pp1")),
            new KeyValuePair<NamespacedName, Person>(new NamespacedName("pp3"), new Person("pp3", "pp1")),
            new KeyValuePair<NamespacedName, Person>(new NamespacedName("pp4"), new Person("pp4", "pp1", false)),
        };

        DataGridObjects = new DataGridCollectionView(People2);

        DataGridObjects.Filter = item => ((KeyValuePair<NamespacedName, Person>)item).Value.Male == true;

        _disTimer.Interval = TimeSpan.FromSeconds(1);
        _disTimer.Tick += _disTimer_Tick; ;
        _disTimer.Start();
    }

    private async void _disTimer_Tick(object? sender, EventArgs e)
    {
        var rand = RandomString(5);

        if (count < 4)
        {
            await Dispatcher.UIThread.InvokeAsync(() => People2[new NamespacedName("pp" + count)] = new Person(rand, rand, true));
        }
        else if (count == 4)
        {
            await Dispatcher.UIThread.InvokeAsync(() => People2[new NamespacedName("pp" + count)] = new Person(rand, rand, false));
        }
        else if (count == 5)
        {
            //await Dispatcher.UIThread.InvokeAsync(() => People2[new NamespacedName(rand)] = new Person(rand, rand, true));
        }
        else if (count == 6)
        {
            //await Dispatcher.UIThread.InvokeAsync(() => People2[new NamespacedName(rand)] = new Person(rand, rand, false));
        }

        count++;

        if (count == 6)
        {
            count = 0;
        }
    }


    private static Random random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

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