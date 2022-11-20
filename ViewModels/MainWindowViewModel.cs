using Avalonia.Controls;
using System;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Threading;
using ReactiveUI;


namespace Reative.ViewModels
{
    public class MainWindowViewModel : ViewModelBase,IActivatableViewModel
    {

        private string _greeting = "Hello world";
        public string Greeting {
            get => _greeting;
            set => this.RaiseAndSetIfChanged(ref _greeting, value);
        }
        public ViewModelActivator Activator { get; }

        public MainWindowViewModel()
        {
            Activator = new ViewModelActivator();
            this.WhenActivated((CompositeDisposable disposables) =>
            {

                    // Just log the ViewModel's activation
                    // https://github.com/kentcb/YouIandReactiveUI/blob/master/ViewModels/Samples/Chapter%2018/Sample%2004/ChildViewModel.cs
                    Console.WriteLine(
                        $"[vm {Thread.CurrentThread.ManagedThreadId}]: " +
                        "ViewModel activated");

                    // Asynchronously generate a new greeting message every second
                    // https://reactiveui.net/docs/guidelines/framework/ui-thread-and-schedulers
                    Observable
                        .Timer(
                            TimeSpan.FromMilliseconds(100), // give the view time to activate
                            TimeSpan.FromMilliseconds(1000),
                            RxApp.MainThreadScheduler)
                        .Take(Traits.Length)
                        .Do(
                            t => {
                                var newGreeting = $"Hello, {Traits[t % Traits.Length]} world !";
                                Console.WriteLine(
                                    $"[vm {Thread.CurrentThread.ManagedThreadId}]: " +
                                    $"Timer Observable -> " +
                                    $"Setting greeting to: \"{newGreeting}\"");
                                Greeting = newGreeting;
                            },
                            () =>
                                Console.WriteLine(
                                    "Those are all the greetings, folks! " +
                                    "Feel free to close the window now...\n"))
                        .Subscribe()
                        .DisposeWith(disposables);

                    // Just log the ViewModel's deactivation
                    // https://github.com/kentcb/YouIandReactiveUI/blob/master/ViewModels/Samples/Chapter%2018/Sample%2004/ChildViewModel.cs
                    Disposable
                        .Create(
                            () =>
                                Console.WriteLine(
                                    $"[vm {Thread.CurrentThread.ManagedThreadId}]: " +
                                    "ViewModel deactivated"))
                        .DisposeWith(disposables);
                });

            this
                .WhenAnyValue(vm => vm.Greeting)
                .Skip(1) // ignore the initial NullOrEmpty value of Greeting
                .Do(
                    greeting =>
                        Console.WriteLine(
                            $"[vm {Thread.CurrentThread.ManagedThreadId}]: " +
                            "WhenAnyValue()   -> " +
                            $"Greeting value changed to: \"{greeting}\"\n"))
                .Subscribe();
        }
        private static readonly string[] Traits = {
            "expressive",
            "clear",
            "responsive",
            "concurrent",
            "reactive"
        };

    }
}
