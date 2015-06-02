using System;
using Windows.UI.Xaml.Controls;
using WindowsIoTSocketServer;
using GoPiGo;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoPiGoNetworked
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly IBuildGoPiGoDevices DeviceFactory = GoPiGo.DeviceFactory.Build;
        private readonly ICommandParser _commandParser;

        public MainPage()
        {
            var goPiGo = DeviceFactory.BuildGoPiGo();
            goPiGo.MotorController().EnableServo();
            var leftLed = DeviceFactory.BuildLed(Pin.LedLeft);
            var rightLed = DeviceFactory.BuildLed(Pin.LedRight);

            _commandParser = new CommandParser(goPiGo,leftLed,rightLed);

            SocketConnection.StartListener();
            SocketConnection.NewMessageReady += SendCommand;

            InitializeComponent();
        }

        private void SendCommand(object sender, MessageSentEventArgs e)
        {
            _commandParser.ParseCommand(e.Message);
        }
    }
}
