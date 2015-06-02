using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using GoPiGo;
using GoPiGo.Sensors;
using GoPiGoApp.Controllers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoPiGoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static bool IsRobot = true; 
        private static readonly IBuildGoPiGoDevices DeviceFactory = global::GoPiGo.DeviceFactory.Build;

        public static Stopwatch Stopwatch;
        public IGoPiGo GoPiGo;
        private DispatcherTimer _timer;
        private readonly ILed _leftLed;
        private readonly ILed _rightLed;

        public MainPage()
        {
            GoPiGo = DeviceFactory.BuildGoPiGo();
            GoPiGo.MotorController().EnableServo();
            _leftLed = DeviceFactory.BuildLed(Pin.LedLeft);
            _rightLed = DeviceFactory.BuildLed(Pin.LedRight);
            this.InitializeComponent();
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        private void button_Forward_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(GoPiGoCommand.Forward);
        }

        private void button_TurnLeft_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(GoPiGoCommand.Left);
        }

        private void button_Stop_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(GoPiGoCommand.Stop);
        }

        private void button_TurnRight_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(GoPiGoCommand.Right);
        }


        private void button_Backwards_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(GoPiGoCommand.Backward);
        }

        private void button_LeftRotate_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(GoPiGoCommand.RotateLeft);
        }

        private void button_RightRotate_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(GoPiGoCommand.RotateLeft);
        }

        private void SetRightMotorSpeed(object sender, object e)
        {
            SendCommand(GoPiGoCommand.SetRightMotorSpeed, (int)this.slider_RightMotorSpeed.Value);
            if (checkBox_SyncLeftRightSpeed.IsChecked.Value)
                SendCommand(GoPiGoCommand.SetLeftMotorSpeed, (int)this.slider_LeftMotorSpeed.Value);
            _timer.Stop();
            _timer.Tick -= SetRightMotorSpeed;
        }
        private void SetLeftMotorSpeed(object sender, object e)
        {
            SendCommand(GoPiGoCommand.SetLeftMotorSpeed, (int)this.slider_LeftMotorSpeed.Value);
            if (this.checkBox_SyncLeftRightSpeed.IsChecked.Value)
                SendCommand(GoPiGoCommand.SetRightMotorSpeed, (int)this.slider_RightMotorSpeed.Value);
            _timer.Stop();
            _timer.Tick -= SetLeftMotorSpeed;
        }

        private void slider_RightMotorSpeed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_timer != null)
                _timer.Stop();

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += SetRightMotorSpeed;
            _timer.Start();

            if (checkBox_SyncLeftRightSpeed != null && this.checkBox_SyncLeftRightSpeed.IsChecked.Value)
                this.slider_LeftMotorSpeed.Value = this.slider_RightMotorSpeed.Value;
        }

        private void slider_LeftMotorSpeed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_timer != null)
                _timer.Stop();

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += SetLeftMotorSpeed;
            _timer.Start();

            if (checkBox_SyncLeftRightSpeed != null && this.checkBox_SyncLeftRightSpeed.IsChecked.Value)
                this.slider_RightMotorSpeed.Value = this.slider_LeftMotorSpeed.Value;
        }

        private void SendCommand(GoPiGoCommand cmd, int value = 0)
        {
            CommandParser.GoPiGo = GoPiGo;
            CommandParser.ParseCommand(cmd, value);
        }

        private void radioButton_LLedOn_Checked(object sender, RoutedEventArgs e)
        {
            CommandParser.LeftLed = _leftLed;
            SendCommand(GoPiGoCommand.SwitchLeftLed , 1);
            this.UiLLed.Fill = new SolidColorBrush(Colors.Red);
        }

        private void radioButton_LLedOff_Checked(object sender, RoutedEventArgs e)
        {
            CommandParser.LeftLed = _leftLed;
            SendCommand(GoPiGoCommand.SwitchLeftLed);
            if (UiLLed != null) this.UiLLed.Fill = new SolidColorBrush(Colors.Gray);
        }

        private void radioButton_RLedOn_Checked(object sender, RoutedEventArgs e)
        {
            CommandParser.RightLed = _rightLed;
            SendCommand(GoPiGoCommand.SwitchRightled, 1);
            this.UiRLed.Fill = new SolidColorBrush(Colors.Red);
        }

        private void radioButton_RLedOff_Checked(object sender, RoutedEventArgs e)
        {
            CommandParser.RightLed = _rightLed;
            SendCommand(GoPiGoCommand.SwitchRightled);
            if (UiRLed != null) this.UiRLed.Fill = new SolidColorBrush(Colors.Gray);
        }

        private void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SendCommand(GoPiGoCommand.SetServoAngle, (int)slider_ServoControl.Value);
        }
    }
}
