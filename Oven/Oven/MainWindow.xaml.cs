using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Subjects;
using System.Reactive;
using System.Windows.Threading;
using System.Windows.Media.Effects;

namespace Oven
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int Temperature = 20;
        private HoodSystem hoodSystem;
        private HoodDisplay hoodDisplay;
        private OvenSystem ovenSystem;
        private OvenDisplay ovenDisplay;

        public MainWindow()
        {
            //Initialixation
            InitializeComponent();
            
            //Label Init
            CurrentTimeLabel.Content = DateTime.Now.ToString("HH:mm");
            DataLabel.Content = DateTime.Now.ToString("MM-dd-yyyy");
            TemperatureLabel.Content = Convert.ToString(Temperature);

            //Grids Init
            MenuGrid.Visibility = Visibility.Visible;
            HoodGrid.Visibility = Visibility.Hidden;
            OvenGridN.Visibility = Visibility.Hidden;
            PizzaGrid.Visibility = Visibility.Hidden;
            MeatGrid.Visibility = Visibility.Hidden;

            //Observable interface
            hoodSystem = new HoodSystem();
            hoodDisplay = new HoodDisplay(HoodCheckLab, HoodElipse);
            hoodSystem.Subscribe(hoodDisplay);

            ovenSystem = new OvenSystem();
            ovenDisplay = new OvenDisplay(OvenTempLabel, TimerLabel);
            ovenSystem.Subscribe(ovenDisplay);

        }
        #region Menu
        private void Hood_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Hidden;
            HoodGrid.Visibility = Visibility.Visible;
        }

        private void OvenClick(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Hidden;
            OvenGridN.Visibility = Visibility.Visible;
        }
        #endregion

        #region Hood
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Visible;
            HoodGrid.Visibility = Visibility.Hidden;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TurnHood_Click(object sender, RoutedEventArgs e)
        {
            hoodSystem.Toggle();
        }
        #endregion

        #region Oven
        private void OvenMenuClick(object sender, RoutedEventArgs e)
        {   
            MenuGrid.Visibility = Visibility.Visible;
            OvenGridN.Visibility = Visibility.Hidden;
        }


        private void PizzaClick(object sender, RoutedEventArgs e)
        {
            PizzaGrid.Visibility = Visibility.Visible;
            OvenGridN.Visibility = Visibility.Hidden;
        }

        private void MeatButtonClick(object sender, RoutedEventArgs e)
        {
            MeatGrid.Visibility = Visibility.Visible;
            OvenGridN.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Pizza
        private void TimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimePicker.SelectedItem == null)
                PizzaTimerElipse.Fill = Brushes.Red;
            else
                PizzaTimerElipse.Fill = Brushes.Green;
            if (TimePicker.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedTime = selectedItem.Content.ToString();
            }
        }

        private void PizzaBakePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PizzaBakePicker_SelectionChanged == null)
                PizzaBakeElipse.Fill = Brushes.Red;
            else
                PizzaBakeElipse.Fill = Brushes.Green;
            if (BakeTypePicker.SelectedItem is ComboBoxItem selectedItem)
            {
                string pizzaBakeSelectedItem = selectedItem.Content.ToString();
            }

        }

        private void PizzaTemperatureValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PizzaTemperatureLabel != null)
            {
                PizzaTemperatureLabel.Content = $"{e.NewValue:F0}°C";
            }
        }
        private void PizzaBackClick(object sender, RoutedEventArgs e)
        {
            OvenGridN.Visibility = Visibility.Visible;
            PizzaGrid.Visibility = Visibility.Hidden;
        }

        private void PizzaStartButtonClick(object sender, RoutedEventArgs e)
        {
            int ovenTemperature = (int)PizzaTemperature.Value;

            if (TimePicker.SelectedItem is ComboBoxItem selectedItem &&
                int.TryParse(selectedItem.Content.ToString().Replace(" minutes", ""), out int time) &&
                BakeTypePicker.SelectedItem is ComboBoxItem pizzaBakeSelectedItem)
            {
                ovenSystem.StartBaking(ovenTemperature, time);
                MenuGrid.Visibility = Visibility.Visible;
                PizzaGrid.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region Meat
        private void MeatBackClick(object sender, RoutedEventArgs e)
        {
            MeatGrid.Visibility = Visibility.Hidden;
            OvenGridN.Visibility = Visibility.Visible;
        }
        private void MeatTimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MeatTimePicker_SelectionChanged == null)
                MeatTimerElipse.Fill = Brushes.Red;
            else
                MeatTimerElipse.Fill = Brushes.Green;
            if (MeatTimePicker.SelectedItem is ComboBoxItem selectedItem)
            {
                string MeatSelectedTime = selectedItem.Content.ToString();
            }

        }
        private void MeatTemperatureValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MeatTemperatureLabel != null)
            {
                MeatTemperatureLabel.Content = $"{e.NewValue:F0}°C";
            }
        }
        #endregion
    }

    #region HoodClasses
    public class HoodSystem : IObservable<bool>
    {
        private bool isOn;
        private readonly BehaviorSubject<bool> _subject;


        public HoodSystem() 
        {
            _subject = new BehaviorSubject<bool>(isOn);
        }

        public void Toggle()
        {
            isOn = !isOn;
            _subject.OnNext(isOn); 
        }


        public IDisposable Subscribe(IObserver<bool> observer)
        {
            return _subject.Subscribe(observer);
        }
    }

    public class HoodDisplay : IObserver<bool>
    {
        private Label label;
        private Shape elipse;
            
        public HoodDisplay(Label label, Shape elipse)
        {
            this.label = label;
            this.elipse = elipse;
        }

        public void OnNext(bool isOn)
        {
            if (isOn)
            {
                this.elipse.Fill = Brushes.Green;
                label.Foreground = new SolidColorBrush(Colors.Green);
                label.Content = "ON";

                label.Effect = new DropShadowEffect
                {
                    Color = Colors.Lime,
                    BlurRadius = 20,
                    ShadowDepth = 0
                };
            }
            else
            {
                this.elipse.Fill = Brushes.Red;
                label.Foreground = new SolidColorBrush(Colors.Red);
                label.Content = "OFF";

                label.Effect = new DropShadowEffect
                {
                    Color = Colors.Red,
                    BlurRadius = 20,
                    ShadowDepth = 0
                };
            }
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region OvenSystem
    public class OvenSystem : IObservable<(string ovenTemperature, string timeLeft)>
    {
        private readonly BehaviorSubject<(string, string)> _subject;
        private DispatcherTimer _timer;
        private int _ovenTemperature;
        private int _timeLeft;

        public OvenSystem()
        {
            _ovenTemperature = -1; 
            _timeLeft = -1;
            _subject = new BehaviorSubject<(string, string)>(("- - -", "- - - - -"));

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) 
            };
            _timer.Tick += TimerTick;
        }

        public void StartBaking(int ovenTemperature, int time)
        {
            _ovenTemperature = ovenTemperature;
            _timeLeft = time;
            UpdateSubscribers();

            _timer.Start();
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            if (_timeLeft > 0)
            {
                _timeLeft--;
                UpdateSubscribers();
            }
            else
            {
                _timer.Stop();
            }
        }

        private void UpdateSubscribers()
        {
            string tempDisplay = _ovenTemperature > 0 ? $"{_ovenTemperature}°C" : "---";
            string timeDisplay = _timeLeft >= 0 ? $"{_timeLeft} min" : "---";
            _subject.OnNext((tempDisplay, timeDisplay));
        }

        public IDisposable Subscribe(IObserver<(string ovenTemperature, string timeLeft)> observer)
        {
            return _subject.Subscribe(observer);
        }
    }



    public class OvenDisplay : IObserver<(string ovenTemperature, string timeLeft)>
    {
        private Label _temperatureLabel;
        private Label _timeLabel;

        public OvenDisplay(Label temperatureLabel, Label timeLabel)
        {
            _temperatureLabel = temperatureLabel;
            _timeLabel = timeLabel;
        }

        public void OnNext((string ovenTemperature, string timeLeft) value)
        {
            _temperatureLabel.Content = value.ovenTemperature;
            _timeLabel.Content = value.timeLeft;
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
    }


    #endregion
}
