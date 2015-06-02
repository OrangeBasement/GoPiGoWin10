using System;
using GoPiGo;
using GoPiGo.Sensors;

namespace GoPiGoNetworked
{
    public interface ICommandParser
    {
        void ParseCommand(string message);
    }

    public class CommandParser : ICommandParser
    {
        private readonly IGoPiGo _goPiGo;
        private readonly ILed _leftLed;
        private readonly ILed _rightLed;

        public CommandParser(IGoPiGo goPiGo, ILed leftLed, ILed rightLed)
        {
            _goPiGo = goPiGo;
            _leftLed = leftLed;
            _rightLed = rightLed;
        }

        public void ParseCommand(string message)
        {
            try
            {
                var parsedCommand = message.Split('|');
                var commandInt = Convert.ToInt32(parsedCommand[0]);
                var command = (GoPiGoCommand)commandInt;
                var value = Convert.ToInt32(parsedCommand[1]);
                ParseCommand(command, value);
            }
            catch (Exception e)
            {
                //ToDo: error catching
            }
        }

        private void ParseCommand(GoPiGoCommand command, int value)
        {
            var motorController = _goPiGo.MotorController();
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
                    _leftLed.ChangeState((SensorStatus)value);
                    break;
                case GoPiGoCommand.SwitchRightled:
                    _rightLed.ChangeState((SensorStatus)value);
                    break;
                case GoPiGoCommand.SetServoAngle:
                    motorController.RotateServo(value);
                    break;
            }
        }
    }

    public enum GoPiGoCommand
    {
        Stop = 0,
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
