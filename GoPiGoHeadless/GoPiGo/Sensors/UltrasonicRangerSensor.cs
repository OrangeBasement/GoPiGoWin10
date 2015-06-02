using System;
using System.Threading.Tasks;

namespace GoPiGo.Sensors
{
    public interface IUltrasonicRangerSensor
    {
        int MeasureInCentimeters();
    }

    internal class UltrasonicRangerSensor : IUltrasonicRangerSensor
    {
        private const byte CommandAddress = 117;
        private readonly GoPiGo _device;
        private readonly Pin _pin;

        internal UltrasonicRangerSensor(GoPiGo device, Pin pin)
        {
            _device = device;
            _pin = pin;
        }

        public int MeasureInCentimeters()
        {
            var buffer = new[] { CommandAddress, (byte)_pin, Constants.Unused, Constants.Unused };
            _device.DirectAccess.Write(buffer);
            WaitForMilliseconds(800);
            _device.DirectAccess.Read(buffer);
            var value1 = buffer[0];
            _device.DirectAccess.Read(buffer);
            var value2 = buffer[0];
            return value1*256 + value2;
        }

        private static async void WaitForMilliseconds(int milliseconds)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
        }
    }
}
