using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaApplication.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private ObservableCollection<Person> _people;

    [ObservableProperty]
    private DataGridCollectionView _dataGridObjects;

    private readonly DispatcherTimer _disTimer = new();

    private int _count;

    private int _max = 4; // Change this to 6 to see even more odd behavior, like duplicate items

    public MainViewModel()
    {
        _people = new ObservableCollection<Person>
        {
            new Person("Bpb", "Trea"),
            new Person("Neil", "Armstrong"),
            new Person("Buzz", "Lightyear"),
            new Person("James", "Kirk"),
            new Person("Fem", "Fem", false)
        };

        DataGridObjects = new DataGridCollectionView(_people)
        {
            Filter = item => ((Person)item).Male == true
        };

        _disTimer.Interval = TimeSpan.FromSeconds(1);
        _disTimer.Tick += _disTimer_Tick; ;
        _disTimer.Start();
    }

    private void _disTimer_Tick(object? sender, EventArgs e)
    {
        var rand = RandomString(10);

        if (_count < 4)
        {
            // Update item matching the filter
            _people[_count] = new Person(rand, rand, true);
        }
        else if (_count == 4)
        {
            // Update item not matching the filter
            // Bug, this causes items to be removed from the DataGridCollectionView
            _people[_count] = new Person(rand, rand, false);
        }
        else if (_count == 5)
        {
            // Add new item matching the filter
            _people.Add(new Person(rand, rand, true));
        }
        else if (_count == 6)
        {
            // Add new item not matching the filter
            _people.Add(new Person(rand, rand, false));
        }

        if (_count == _max)
            _count = 0;
        else
            _count++;
    }

    private static Random random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
