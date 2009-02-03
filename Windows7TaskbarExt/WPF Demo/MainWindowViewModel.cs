namespace Huseyint.Windows7.WPF.Demo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    public class MainWindowViewModel : DependencyObject
    {
        public static readonly DependencyProperty OverlayIconProperty = DependencyProperty.Register(
            "OverlayIcon",
            typeof(OverlayIcon),
            typeof(MainWindowViewModel),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty ProgressStateProperty = DependencyProperty.Register(
            "ProgressState",
            typeof(ProgressState),
            typeof(MainWindowViewModel),
            new UIPropertyMetadata(ProgressState.NoProgress));

        public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(
            "ProgressValue",
            typeof(ulong),
            typeof(MainWindowViewModel),
            new UIPropertyMetadata((ulong)0));

        public static readonly DependencyProperty ProgressValueTotalProperty = DependencyProperty.Register(
            "ProgressValueTotal",
            typeof(ulong),
            typeof(MainWindowViewModel),
            new UIPropertyMetadata((ulong)100));

        public MainWindowViewModel()
        {
            this.OverlayIcons = new List<OverlayIcon>()
            {
                new OverlayIcon(null),
                new OverlayIcon(new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/Error.png")), "Error"),
                new OverlayIcon(new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/Info.png")), "Info"),
                new OverlayIcon(new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/New.png")), "New"),
                new OverlayIcon(new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/Warning.png")), "Warning"),
            };

            this.SimulateProgressStatesCommand = new SimpleCommand()
            {
                CanExecuteDelegate = o => true,
                ExecuteDelegate = this.ExecuteSimulateProgressStatesCommand
            };

            this.ThumbnailBarButtons = new List<ThumbnailBarButton>()
            {
                new ThumbnailBarButton()
                {
                    Tooltip = "Foo",
                    Icon = new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/Error.png")),
                    IsHidden = true,
                    IsDisabled = false,
                    IsDismissedOnClick = false,
                    HasBackground = true,
                },
                new ThumbnailBarButton()
                {
                    Icon = new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/Info.png")),
                    Tooltip = "Bar",
                    IsHidden = false,
                    IsDisabled = false,
                    IsDismissedOnClick = false,
                    HasBackground = true,
                },
                new ThumbnailBarButton()
                {
                    Icon = new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/New.png")),
                    Tooltip = "Baz",
                    IsHidden = false,
                    IsDisabled = false,
                    IsDismissedOnClick = true,
                    HasBackground = false,
                },
                new ThumbnailBarButton()
                {
                    Icon = new BitmapImage(new Uri(@"pack://application:,,,/WPFDemo;component/Images/Warning.png")),
                    Tooltip = "Quux",
                    IsHidden = true,
                    IsDisabled = true,
                    IsDismissedOnClick = false,
                    HasBackground = true,
                },
            };

            foreach (var button in this.ThumbnailBarButtons)
            {
                button.Click += this.ThumbnailBarButton_Click;
            }

            this.UpdateThumbBarButtonsCommand = new SimpleCommand()
            {
                CanExecuteDelegate = o => true,
                ExecuteDelegate = this.ExecuteUpdateThumbBarButtonsCommand
            };
        }

        public ICommand SimulateProgressStatesCommand { get; private set; }

        public ICommand UpdateThumbBarButtonsCommand { get; private set; }

        public IList<OverlayIcon> OverlayIcons { get; private set; }

        public OverlayIcon OverlayIcon
        {
            get { return (OverlayIcon)GetValue(OverlayIconProperty); }
            set { SetValue(OverlayIconProperty, value); }
        }

        public ProgressState ProgressState
        {
            get
            {
                return (ProgressState)GetValue(ProgressStateProperty);
            }

            set
            {
                SetValue(ProgressStateProperty, value);
            }
        }

        public ulong ProgressValue
        {
            get
            {
                return (ulong)GetValue(ProgressValueProperty);
            }

            set
            {
                SetValue(ProgressValueProperty, value);
            }
        }

        public ulong ProgressValueTotal
        {
            get { return (ulong)GetValue(ProgressValueTotalProperty); }
            set { SetValue(ProgressValueTotalProperty, value); }
        }

        public IList<ThumbnailBarButton> ThumbnailBarButtons { get; private set; }

        private void ExecuteSimulateProgressStatesCommand(object o)
        {
            var worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += this.Worker_DoWork;
            worker.ProgressChanged += this.Worker_ProgressChanged;

            // Simulate estimated calculation of a long operation
            this.ProgressState = ProgressState.NoProgress;
            this.ProgressState = ProgressState.Indeterminate;

            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            Thread.Sleep(2000);

            int operationCount = 100;

            worker.ReportProgress(0);
                
            for (int i = 1; i < operationCount; i++)
            {
                if (i == 42 || i == 65)
                {
                    var autoResetEvent = new AutoResetEvent(false);
                    worker.ReportProgress(i, autoResetEvent);
                    autoResetEvent.WaitOne();
                }
                else
                {
                    worker.ReportProgress(i);
                }

                if (worker.CancellationPending)
                {
                    break;
                }

                Thread.Sleep(100);
            }

            worker.ReportProgress(operationCount);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var i = (ulong)e.ProgressPercentage;
            var autoResetEvent = e.UserState as AutoResetEvent;

            if (i == 0)
            {
                this.ProgressState = ProgressState.Normal;
            }
            else if (i < 100)
            {
                // SetProgressValue inherently set the progress state to ProgressState.Normal
                this.ProgressValue = i;

                if (i == 42)
                {
                    // Set progress state to Paused (Yellow)
                    this.ProgressState = ProgressState.Paused;

                    // Display a fake warning message
                    var result = MessageBox.Show(
                        "You are warned, do you want to continue?",
                        "Warning",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning,
                        MessageBoxResult.No);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Set state back to Normal (Green)
                        this.ProgressState = ProgressState.Normal;
                    }
                    else
                    {
                        worker.CancelAsync();
                    }

                    autoResetEvent.Set();
                }
                else if (i == 65)
                {
                    // Set progress state to Error (Red)
                    this.ProgressState = ProgressState.Error;

                    // Display a fake error message
                    var result = MessageBox.Show(
                        "A critical error occured, do you want to continue?",
                        "Error Occured",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Error,
                        MessageBoxResult.No);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Set state back to Normal (Green)
                        this.ProgressState = ProgressState.Normal;
                    }
                    else
                    {
                        worker.CancelAsync();
                    }

                    autoResetEvent.Set();
                }
            }
            else
            {
                MessageBox.Show("Operation completed!", "Completed", MessageBoxButton.OK, MessageBoxImage.Information);

                this.ProgressState = ProgressState.NoProgress;
            }
        }

        private void ExecuteUpdateThumbBarButtonsCommand(object o)
        {
            foreach (var button in this.ThumbnailBarButtons)
            {
                button.IsDisabled = !button.IsDisabled;
                button.IsHidden = !button.IsHidden;
                button.HasBackground = !button.HasBackground;
            }
        }

        private void ThumbnailBarButton_Click(object sender, EventArgs e)
        {
            var button = (ThumbnailBarButton)sender;

            MessageBox.Show(string.Format("Button with tooltip '{0}' clicked.", button.Tooltip));
        }
    }
}