using realTimeMonitor.Model;
using realTimeMonitor.Services;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace realTimeMonitor
{
    public partial class MainWindow : Window
    {
        private readonly RadarSensor _radarSensor;

        private readonly ConcurrentQueue<SensorData> _sensorQueue = new();

        private readonly SemaphoreSlim _semaphore = new(0);

        private readonly CancellationTokenSource _cts = new();

        public MainWindow()
        {
            InitializeComponent();

            _radarSensor = new RadarSensor("RADAR-01");

            Task.Run(Producer);
            Task.Run(Consumer);
        }

        private async Task Producer()
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    SensorData data = _radarSensor.Read();

                    _sensorQueue.Enqueue(data);

                    _semaphore.Release();

                    await Task.Delay(100, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // Producer stopped
            }
        }

        private async Task Consumer()
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    await _semaphore.WaitAsync(_cts.Token);

                    if (_sensorQueue.TryDequeue(out SensorData? data))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            RadarIdText.Text =
                                $"Radar: {data.RadarId}";

                            TargetIdText.Text =
                                $"Target ID: {data.TargetId}";

                            DistanceText.Text =
                                $"Distance: {data.Distance / 1000:F2} km";

                            AzimuthText.Text =
                                $"Azimuth: {data.Azimuth:F2}°";

                            VelocityText.Text =
                                $"Velocity: {data.Velocity:F2} m/s";

                            TimestampText.Text =
                                $"Last updated: {data.Timestamp:HH:mm:ss.fff}";
                        });
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Consumer stopped
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _cts.Cancel();

            base.OnClosed(e);
        }
    }
}