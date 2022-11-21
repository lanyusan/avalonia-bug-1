using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Data;
using Reative.ViewModels;

namespace Reative.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();

            // ViewModel's WhenActivated block will also get called.
            this.WhenActivated(disposables => {
                // Jut log the View's activation
                Console.WriteLine(
                    $"[v  {Thread.CurrentThread.ManagedThreadId}]: " +
                        "View activated\n");

                // Just log the View's deactivation
                Disposable
                    .Create(
                    () =>
                    Console.WriteLine(
                        $"[v  {Thread.CurrentThread.ManagedThreadId}]: " +
                            "View deactivated"))
                    .DisposeWith(disposables);

                // https://reactiveui.net/docs/handbook/events/#how-do-i-convert-my-own-c-events-into-observables
                // Observable
                //     .FromEventPattern(wndMain, nameof(wndMain.Closing))
                //     .Subscribe(
                //     _ => {
                //         Console.WriteLine(
                //             $"[v  {Thread.CurrentThread.ManagedThreadId}]: " +
                //                 "Main window closing...");
                //     })
                //     .DisposeWith(disposables);

                // https://reactiveui.net/docs/handbook/data-binding/
                // Console.WriteLine("==> {0}", ViewModel);

                var tb = this.FindControl<TextBlock>(nameof(tbGreetingLabel));
                
                tb.Bind(TextBlock.TextProperty, new Binding(nameof(ViewModel.Greeting), BindingMode.OneWay))
                    .DisposeWith(disposables);
            });
            AvaloniaXamlLoader.Load(this);
            // InitializeComponent();
        }
        // private Window wndMain => this.FindControl<Window>("wndMain");
        // private TextBlock tbGreetingLabel => this.FindControl<TextBlock>("tbGreetingLabel");
    }
}
