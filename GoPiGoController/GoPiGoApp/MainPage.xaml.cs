using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using WindowsIoTSocketClient;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoPiGoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool commandAllowed;
        public static Stopwatch Stopwatch;
        private DispatcherTimer _timer;

        public MainPage()
        {
            this.InitializeComponent();
            txtBoxStatus.Text = "Idle";
            SocketConnection.ConnectStatusChanged += SetStatus;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        private void SetStatus(object sender, ConnectionStatusChangedEventArgs e)
        {
            txtBoxStatus.Text = e.Status.ToString();
        }

        private void button_Forward_Click(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("1|0");
        }

        private void button_TurnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("3|0");
        }

        private void button_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("0|0");
        }

        private void button_TurnRight_Click(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("4|0");
        }


        private void button_Backwards_Click(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("2|0");
        }

        private void button_LeftRotate_Click(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("5|0");
        }

        private void button_RightRotate_Click(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("6|0");
        }

        private void SetRightMotorSpeed(object sender, object e)
        {
            if (commandAllowed)
            {
                SocketConnection.SendMessage("8|" + slider_RightMotorSpeed.Value);
                if (checkBox_SyncLeftRightSpeed.IsChecked.Value)
                    SocketConnection.SendMessage("7|" + slider_LeftMotorSpeed.Value);
                _timer.Stop();
                _timer.Tick -= SetRightMotorSpeed;
            }
        }
        private void SetLeftMotorSpeed(object sender, object e)
        {
            if (commandAllowed)
            {
                SocketConnection.SendMessage("7|" + slider_LeftMotorSpeed.Value);
                if (this.checkBox_SyncLeftRightSpeed.IsChecked.Value)
                    SocketConnection.SendMessage("8|" + slider_RightMotorSpeed.Value);
                _timer.Stop();
                _timer.Tick -= SetLeftMotorSpeed;
            }
        }

        private void slider_RightMotorSpeed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (commandAllowed)
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
        }

        private void slider_LeftMotorSpeed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (commandAllowed)
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
        }

        private void radioButton_LLedOn_Checked(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
            {
                SocketConnection.SendMessage("9|1");
                this.UiLLed.Fill = new SolidColorBrush(Colors.Red);
            }
        }

        private void radioButton_LLedOff_Checked(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
            {
                SocketConnection.SendMessage("9|0");
                if (UiLLed != null) this.UiLLed.Fill = new SolidColorBrush(Colors.Gray);
            }
        }

        private void radioButton_RLedOn_Checked(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
            {
                SocketConnection.SendMessage("10|1");
                this.UiRLed.Fill = new SolidColorBrush(Colors.Red);
            }
        }

        private void radioButton_RLedOff_Checked(object sender, RoutedEventArgs e)
        {
            if (commandAllowed)
            {
                SocketConnection.SendMessage("10|0");
                if (UiRLed != null) this.UiRLed.Fill = new SolidColorBrush(Colors.Gray);
            }
        }

        private void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (commandAllowed)
                SocketConnection.SendMessage("11|" + slider_ServoControl.Value);
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            var server = txtBoxServer.Text;
            var port = txtBoxPort.Text;
            SocketConnection.Connect(server, port);
            commandAllowed = true;
        }
    }
}
