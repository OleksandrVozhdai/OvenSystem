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
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Oven
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int Temperature = 20;
        bool MeatChecked = false;

        //IObservable interface
        private HoodSystem hoodSystem;
        private HoodSystem HoodSystemRightUp;
        private HoodSystem HoodSystemLeftUp;
        private HoodSystem HoodSystemRightDown;
        private HoodSystem HoodSystemLeftDown;

        private OvenSystem ovenSystem;

        //IObserver objects
        private HoodDisplay hoodDisplay;
        private HoodDisplay stoveRightUp;
        private HoodDisplay stoveLeftUp;
        private HoodDisplay stoveRightDown;
        private HoodDisplay stoveLeftDown;

        private OvenDisplay BakeOvenDisplay;
        private OvenDisplay MeatOvenDisplay;
        private OvenDisplay VegetableOvenDisplay;
        private OvenDisplay FishOvenDisplay;

        //MediaPlayer
        private static MediaPlayer _player = new MediaPlayer();

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
            VegetableGrid.Visibility = Visibility.Hidden;
            FishGrid.Visibility = Visibility.Hidden;
            StoveGrid.Visibility = Visibility.Hidden;

            //IObservable objects
            hoodSystem = new HoodSystem();
            HoodSystemRightUp = new HoodSystem();
            HoodSystemLeftUp = new HoodSystem();
            HoodSystemRightDown = new HoodSystem();
            HoodSystemLeftDown = new HoodSystem();

            hoodDisplay = new HoodDisplay(HoodCheckLab, HoodElipse);
            stoveRightUp = new HoodDisplay(StoveCheckLabRightUp, StoveElipseRightUp);
            stoveLeftUp = new HoodDisplay(StoveCheckLabLeftUp, StoveElipseLeftUp);
            stoveRightDown = new HoodDisplay(StoveCheckLabRightDown, StoveElipseRightDown);
            stoveLeftDown = new HoodDisplay(StoveCheckLabLeftDown, StoveElipseLeftDown);

            hoodSystem.Subscribe(hoodDisplay);
            HoodSystemRightUp.Subscribe(stoveRightUp);
            HoodSystemLeftUp.Subscribe(stoveLeftUp);
            HoodSystemRightDown.Subscribe(stoveRightDown);
            HoodSystemLeftDown.Subscribe(stoveLeftDown);

            ovenSystem = new OvenSystem(OvenButton);
            BakeOvenDisplay  = new OvenDisplay(OvenTempLabel, TimerLabel, OvenButton);
            MeatOvenDisplay = new OvenDisplay(OvenTempLabel, TimerLabel, OvenButton);
            VegetableOvenDisplay = new OvenDisplay(OvenTempLabel, TimerLabel, OvenButton);
            FishOvenDisplay = new OvenDisplay(OvenTempLabel, TimerLabel, OvenButton);

            ovenSystem.Subscribe(BakeOvenDisplay);
            ovenSystem.Subscribe(MeatOvenDisplay);
            ovenSystem.Subscribe(VegetableOvenDisplay);
            ovenSystem.Subscribe(FishOvenDisplay);
            

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

        private void StoveButtonClick(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Hidden;
            StoveGrid.Visibility = Visibility.Visible;
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

        private void VeggetableButtonClick(object sender, RoutedEventArgs e)
        {
            VegetableGrid.Visibility = Visibility.Visible;
            OvenGridN.Visibility = Visibility.Hidden;
        }
        private void FishButtonClick(object sender, RoutedEventArgs e)
        { 
            FishGrid.Visibility = Visibility.Visible;
            OvenGridN.Visibility= Visibility.Hidden;
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
            int ovenTemperature = Convert.ToInt32(PizzaTemperature.Value);

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
        private void MeatStartButtonClick(object sender, RoutedEventArgs e)
        {
            int ovenTemperature = Convert.ToInt32(MeatTemperature.Value);

            if (MeatTimePicker.SelectedItem is ComboBoxItem MeatSelectedTime &&
               int.TryParse(MeatSelectedTime.Content.ToString().Replace(" minutes", ""), out int time) && MeatChecked)
            {
                ovenSystem.StartBaking(ovenTemperature, time);
                MenuGrid.Visibility = Visibility.Visible;
                MeatGrid.Visibility = Visibility.Hidden;
            }
        }

        private void Meat_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton clickedButton)
            {
                MeatChecked = true;
                ChickenButton.IsChecked = clickedButton == ChickenButton;
                BeefButton.IsChecked = clickedButton == BeefButton;
                SausageButton.IsChecked = clickedButton == SausageButton;
            }
        }
        #endregion

        #region Vegetables
        private void VegetablesBackClick(object sender, RoutedEventArgs e)
        { 
            OvenGridN.Visibility = Visibility.Visible;
            VegetableGrid.Visibility = Visibility.Hidden;
        }
        private void VegetableTimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VegetableTimePicker_SelectionChanged == null)
                VegetableTimerElipse.Fill = Brushes.Red;
            else
                VegetableTimerElipse.Fill = Brushes.Green;
            if (VegetableTimePicker.SelectedItem is ComboBoxItem selectedItem)
            {
                string VegetableSelectedTime = selectedItem.Content.ToString();
            }

        }
        private void VegetableTemperatureValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (VegetableTemperatureLabel != null)
            {
                VegetableTemperatureLabel.Content = $"{e.NewValue:F0}°C";
            }
        }
        private void VegetableStartButtonClick(object sender, RoutedEventArgs e)
        {
            int ovenTemperature = Convert.ToInt32(VegetableTemperature.Value);

            if (VegetableTimePicker.SelectedItem is ComboBoxItem VegetableSelectedTime &&
               int.TryParse(VegetableSelectedTime.Content.ToString().Replace(" minutes", ""), out int time))
            {
                ovenSystem.StartBaking(ovenTemperature, time);
                VegetableGrid.Visibility = Visibility.Hidden;
                MenuGrid.Visibility = Visibility.Visible;
            }    
        }
        #endregion

        #region Fish
        private void FishBackClick(object sender, RoutedEventArgs e)
        {
            FishGrid.Visibility = Visibility.Hidden;
            OvenGridN.Visibility = Visibility.Visible;
        }
        private void FishTimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FishTimePicker_SelectionChanged == null)
                FishTimerElipse.Fill = Brushes.Red;
            else
                FishTimerElipse.Fill = Brushes.Green;
            if (FishTimePicker.SelectedItem is ComboBoxItem selectedItem)
            {
                string FishSelectedTime = selectedItem.Content.ToString();
            }
        }
        private void FishTemperatureValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (FishTemperatureLabel != null)
            {
                FishTemperatureLabel.Content = $"{e.NewValue:F0}°C";
            }
        }
        private void FishStartButtonClick(object sender, RoutedEventArgs e)
        {
            int ovenTemperature = Convert.ToInt32(FishTemperature.Value);

            if (FishTimePicker.SelectedItem is ComboBoxItem FishSelectedTime &&
               int.TryParse(FishSelectedTime.Content.ToString().Replace(" minutes", ""), out int time))
            {
                ovenSystem.StartBaking(ovenTemperature, time);
                FishGrid.Visibility = Visibility.Hidden;
                MenuGrid.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region
        private void StoveBackClick(object sender, RoutedEventArgs e)
        {
            StoveGrid.Visibility = Visibility.Hidden;
            MenuGrid.Visibility = Visibility.Visible;
        }

        private void StoveButtonRightUp(object sender, RoutedEventArgs e)
        {
            HoodSystemRightUp.Toggle();
        }
        private void StoveButtonLeftUp(object sender, RoutedEventArgs e)
        {
            HoodSystemLeftUp.Toggle();
        }
        private void StoveButtonRightDown(object sender, RoutedEventArgs e)
        {
            HoodSystemRightDown.Toggle();
        }
        private void StoveButtonLeftDown(object sender, RoutedEventArgs e)
        {
            HoodSystemLeftDown.Toggle();
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
    public class OvenSystem : IObservable<(string ovenTemperature, string timeLeft, bool isButtonDisabled)>
    {
        private readonly BehaviorSubject<(string, string, bool)> _subject;
        private DispatcherTimer _timer;
        private int _ovenTemperature;
        private int _timeLeft;
        private Button _ovenButton;

        public OvenSystem(Button ovenButton)
        {
            _ovenTemperature = -1;
            _timeLeft = -1;
            _ovenButton = ovenButton;

            _subject = new BehaviorSubject<(string, string, bool)>(("- - -", "- - - - -", false));

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
            string tempDisplay = _timeLeft > 0 ? $"{_ovenTemperature}°C" : "---";
            string timeDisplay = _timeLeft > 0 ? $"{_timeLeft} min" : "---";

            bool isButtonDisabled = _timeLeft > 0;

            _subject.OnNext((tempDisplay, timeDisplay, isButtonDisabled));
        }

        public IDisposable Subscribe(IObserver<(string ovenTemperature, string timeLeft, bool isButtonDisabled)> observer)
        {
            return _subject.Subscribe(observer);
        }
    }

    public class OvenDisplay : IObserver<(string ovenTemperature, string timeLeft, bool isButtonDisabled)>
    {
        private Label _temperatureLabel;
        private Label _timeLabel;
        private Button _ovenButton;

        public OvenDisplay(Label temperatureLabel, Label timeLabel, Button ovenButton)
        {
            _temperatureLabel = temperatureLabel;
            _timeLabel = timeLabel;
            _ovenButton = ovenButton;
        }

        public void OnNext((string ovenTemperature, string timeLeft, bool isButtonDisabled) value)
        {
            _temperatureLabel.Content = value.ovenTemperature;
            _timeLabel.Content = value.timeLeft;
            _ovenButton.IsEnabled = !value.isButtonDisabled;
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
    }



    #endregion
}
