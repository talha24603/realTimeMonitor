namespace realTimeMonitor.Model
{
    public class SensorData
    {
        public string RadarId { get; set; } = "";
        public int TargetId { get; set; }

        public double Distance { get; set; }
        public double Azimuth { get; set; }
        public double Elevation { get; set; }
        public double Velocity { get; set; }

        public DateTime Timestamp { get; set; }
    }
}