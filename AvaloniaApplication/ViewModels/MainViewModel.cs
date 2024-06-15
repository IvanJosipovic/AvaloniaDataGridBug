using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaApplication.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Person> People { get; }

    [ObservableProperty]
    private DataGridCollectionView _dataGridObjects;

    private readonly DispatcherTimer _disTimer = new DispatcherTimer();

    private int count;

    public MainViewModel()
    {
        People = new ObservableCollection<Person>
        {
            new Person("Bpb", "Trea"),
            new Person("Neil", "Armstrong"),
            new Person("Buzz", "Lightyear"),
            new Person("James", "Kirk"),
            new Person("Fem", "Fem", false)
        };

        DataGridObjects = new DataGridCollectionView(People)
        {
            Filter = item => ((Person)item).Male == true
        };

        _disTimer.Interval = TimeSpan.FromSeconds(1);
        _disTimer.Tick += _disTimer_Tick; ;
        _disTimer.Start();
    }

    private async void _disTimer_Tick(object? sender, EventArgs e)
    {
        var max = 4; // Change this to 6 to see even more odd behavior, like duplicate items

        var rand = RandomString(10);

        if (count < 4)
        {
            // Update item matching the filter
            await Dispatcher.UIThread.InvokeAsync(() => People[count] = new Person(rand, rand, true));
        }
        else if (count == 4)
        {
            // Update item not matching the filter
            await Dispatcher.UIThread.InvokeAsync(() => People[count] = new Person(rand, rand, false));
        }
        else if (count == 5)
        {
            // Add new item matching the filter
            await Dispatcher.UIThread.InvokeAsync(() => People.Add(new Person(rand, rand, true)));
        }
        else if (count == 6)
        {
            // Add new item not matching the filter
            await Dispatcher.UIThread.InvokeAsync(() => People.Add(new Person(rand, rand, false)));
        }

        if (count == max)
        {
            count = 0;
        }
        else
        {
            count++;
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
