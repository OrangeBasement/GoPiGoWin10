using Windows.ApplicationModel.Background;
using GoPiGo;
using Windows.System.Threading;
using System;
using System.Linq;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Driver
{
    public sealed class StartupTask : IBackgroundTask
    {
        private readonly IBuildGoPiGoDevices _deviceFactory = DeviceFactory.Build;
        IGoPiGo _goPiGo;
        private ThreadPoolTimer timer;
        Random random;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
   
            random = new Random();
            _goPiGo = _deviceFactory.BuildGoPiGo();
            _goPiGo.MotorController().EnableServo();
            var analogSensor = _deviceFactory.BuildUltraSonicSensor(Pin.Analog1);
            var test =analogSensor.MeasureInCentimeters();
 
            _goPiGo.MotorController().RotateServo(125);
            _goPiGo.MotorController().RotateServo(50);
            _goPiGo.MotorController().RotateServo(-90);

            //_goPiGo.EncoderController().EnableEncoders();
            //var test2 = _goPiGo.EncoderController().ReadEncoder(Motor.MotorOne);

            _goPiGo.MotorController().MoveForward();
        }

        private void Timer_Tick(ThreadPoolTimer timer)
        {
            RandomMovement();
        }

        private void RandomMovement()
        {
            var command =  (RandomCommands)random.Next(0, (int)GetLastCommand() + 1);
            RunCommand(command);
        }

        private void RunCommand(RandomCommands command)
        {
            switch (command)
            {
                case RandomCommands.Backwards:
                    _goPiGo.MotorController().MoveBackward();
                    break;
                case RandomCommands.Forward:
                    _goPiGo.MotorController().MoveBackward();
                    break;
                case RandomCommands.Left:
                    _goPiGo.MotorController().MoveLeft();
                    break;
                case RandomCommands.Right:
                    _goPiGo.MotorController().MoveRight();
                    break;

            }
        }

        private enum RandomCommands
        {
            Forward,
            Backwards,
            Left,
            Right
        }

        private RandomCommands GetLastCommand()
        {
            return Enum.GetValues(typeof(RandomCommands)).Cast<RandomCommands>().Last();
        }
    }
}
