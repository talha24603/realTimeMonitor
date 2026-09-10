using realTimeMonitor.Model;

namespace realTimeMonitor.Services
{
    public class RadarSensor
    {
        private readonly Random _random = new();
        private readonly string _radarId;

        public RadarSensor(string radarId)
        {
            _radarId = radarId;
        }

        public SensorData Read()
        {
            return new SensorData
            {
                RadarId = _radarId,

                TargetId = _random.Next(1, 10),

                Distance = _random.NextDouble() * 10000,

                Azimuth = -60 + _random.NextDouble() * 120,

                Elevation = -10 + _random.NextDouble() * 20,

                Velocity = -100 + _random.NextDouble() * 200,

                Timestamp = DateTime.Now
            };
        }
    }
}