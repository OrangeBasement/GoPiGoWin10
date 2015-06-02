using GoPiGo;
using GoPiGo.Sensors;

namespace GoPiGoApp.Controllers
{
    public static class CommandParser
    {
        public static IGoPiGo GoPiGo;
        public static ILed LeftLed;
        public static ILed RightLed;
        public static void ParseCommand(GoPiGoCommand command, int value)
        {
            var motorController = GoPiGo.MotorController();
            switch (command)
            {
                case GoPiGoCommand.Stop:
                    motorController.Stop();
                    break;
                case GoPiGoCommand.Backward:
                    motorController.MoveBackward();
                    break;
                case GoPiGoCommand.Forward:
                    motorController.MoveForward();
                    break;
                case GoPiGoCommand.Left:
                    motorController.MoveLeft();
                    break;
                case GoPiGoCommand.Right:
                    motorController.MoveRight();
                    break;
                case GoPiGoCommand.RotateLeft:
                    motorController.RotateLeft();
                    break;
                case GoPiGoCommand.RotateRight:
                    motorController.RotateRight();
                    break;
                case GoPiGoCommand.SetLeftMotorSpeed:
                    motorController.SetLeftMotorSpeed(value);
                    break;
                case GoPiGoCommand.SetRightMotorSpeed:
                    motorController.SetRightMotorSpeed(value);
                    break;
                case GoPiGoCommand.SwitchLeftLed:
                    LeftLed.ChangeState((SensorStatus)value);
                    break;
                case GoPiGoCommand.SwitchRightled:
                    RightLed.ChangeState((SensorStatus)value);
                    break;
                case GoPiGoCommand.SetServoAngle:
                    motorController.RotateServo(value);
                    break;
            }
        }
    }

    public enum GoPiGoCommand
    {
        Stop,
        Forward,
        Backward,
        Left,
        Right,
        RotateLeft,
        RotateRight,
        SetLeftMotorSpeed,
        SetRightMotorSpeed,
        SwitchLeftLed,
        SwitchRightled,
        SetServoAngle
    }
}