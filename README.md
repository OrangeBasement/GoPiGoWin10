//Build our device factory
var DeviceFactory = GoPiGo.DeviceFactory.Build;
//Build motor controller
var motorController = GoPiGo.MotorController();

//GoPiGoCommand.Stop:
motorController.Stop();

//GoPiGoCommand.Backward:
motorController.MoveBackward();

//GoPiGoCommand.Forward:
motorController.MoveForward();

//GoPiGoCommand.Left:
motorController.MoveLeft();

//GoPiGoCommand.Right:
motorController.MoveRight();

//GoPiGoCommand.RotateLeft:
motorController.RotateLeft();

//GoPiGoCommand.RotateRight:
motorController.RotateRight();

//GoPiGoCommand.SetLeftMotorSpeed:
motorController.SetLeftMotorSpeed(value);

//GoPiGoCommand.SetRightMotorSpeed:
motorController.SetRightMotorSpeed(value);

//GoPiGoCommand.SwitchLeftLed:
LeftLed.ChangeState((SensorStatus)value);

//GoPiGoCommand.SwitchRightled:
RightLed.ChangeState((SensorStatus)value);

//GoPiGoCommand.SetServoAngle:
motorController.RotateServo(value);

//Build GoPiGo Leds
var leftLed = DeviceFactory.BuildLed(Pin.LedLeft);
var rightLed = DeviceFactory.BuildLed(Pin.LedRight);

//Turn LED On/Off (value = 0 | 1)
leftLed.ChangeState((SensorStatus)value);


# GoPiGoWin10
