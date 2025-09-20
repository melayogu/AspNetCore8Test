using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Models.LoRaModels;

namespace AspNetCore8Test.Services.LoRaServices
{
    public interface IMeterReadingService
    {
        Task<IEnumerable<MeterReadingDto>> GetAllReadingsAsync();
        Task<IEnumerable<MeterReadingDto>> GetReadingsByDeviceAsync(string deviceEui);
        Task<IEnumerable<MeterReadingDto>> GetReadingsByDeviceAsync(string deviceEui, DateTime? startDate, DateTime? endDate);
        Task<MeterReadingDto?> GetLatestReadingAsync(string deviceEui);
        Task<IEnumerable<MeterReadingSummaryDto>> GetDailySummaryAsync(string deviceEui, DateTime? startDate, DateTime? endDate);
        Task<MeterReadingDto> CreateReadingAsync(CreateMeterReadingDto createDto);
    }

    public class MeterReadingService : IMeterReadingService
    {
        private static readonly List<MeterReading> _readings = new()
        {
            new MeterReading
            {
                Id = 1,
                DeviceEui = "ABCDEF1234567890",
                Timestamp = DateTime.UtcNow.AddHours(-6),
                ReadingValue = 1268.3m,
                Consumption = 12.5m,
                FlowRate = 1.8m,
                BatteryLevel = 78,
                Rssi = -115,
                Snr = 5.2,
                IsEstimated = false,
                Quality = "Good",
                PayloadHex = "0102030405060708"
            },
            new MeterReading
            {
                Id = 2,
                DeviceEui = "ABCDEF1234567890",
                Timestamp = DateTime.UtcNow.AddHours(-3),
                ReadingValue = 1275.1m,
                Consumption = 6.8m,
                FlowRate = 1.6m,
                BatteryLevel = 77,
                Rssi = -113,
                Snr = 5.5,
                IsEstimated = false,
                Quality = "Good",
                PayloadHex = "090A0B0C0D0E0F10"
            },
            new MeterReading
            {
                Id = 3,
                DeviceEui = "1234567890ABCDEF",
                Timestamp = DateTime.UtcNow.AddHours(-1),
                ReadingValue = 8648.5m,
                Consumption = 15.4m,
                FlowRate = 2.4m,
                BatteryLevel = 95,
                Rssi = -108,
                Snr = 7.1,
                IsEstimated = false,
                Quality = "Excellent",
                PayloadHex = "1112131415161718"
            },
            new MeterReading
            {
                Id = 4,
                DeviceEui = "A1B2C3D4E5F60789",
                Timestamp = DateTime.UtcNow.AddHours(-12),
                ReadingValue = 438.2m,
                Consumption = 20.3m,
                FlowRate = 1.1m,
                BatteryLevel = 63,
                Rssi = -120,
                Snr = 2.5,
                IsEstimated = true,
                Quality = "Fair",
                PayloadHex = "1A1B1C1D1E1F2021"
            },
            new MeterReading
            {
                Id = 5,
                DeviceEui = "A1B2C3D4E5F60789",
                Timestamp = DateTime.UtcNow.AddHours(-2),
                ReadingValue = 452.7m,
                Consumption = 14.5m,
                FlowRate = 1.4m,
                BatteryLevel = 62,
                Rssi = -118,
                Snr = 3.1,
                IsEstimated = false,
                Quality = "Good",
                PayloadHex = "2122232425262728"
            }
        };

        private static int _nextId = 6;

        public async Task<IEnumerable<MeterReadingDto>> GetAllReadingsAsync()
        {
            await Task.Delay(1);
            return _readings
                .OrderByDescending(r => r.Timestamp)
                .Select(MapToDto);
        }

        public async Task<IEnumerable<MeterReadingDto>> GetReadingsByDeviceAsync(string deviceEui)
        {
            return await GetReadingsByDeviceAsync(deviceEui, null, null);
        }

        public async Task<IEnumerable<MeterReadingDto>> GetReadingsByDeviceAsync(string deviceEui, DateTime? startDate, DateTime? endDate)
        {
            await Task.Delay(1);
            var query = _readings.Where(r => r.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase));

            if (startDate.HasValue)
            {
                query = query.Where(r => r.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.Timestamp <= endDate.Value);
            }

            return query
                .OrderByDescending(r => r.Timestamp)
                .Select(MapToDto);
        }

        public async Task<MeterReadingDto?> GetLatestReadingAsync(string deviceEui)
        {
            await Task.Delay(1);
            var reading = _readings
                .Where(r => r.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefault();

            return reading != null ? MapToDto(reading) : null;
        }

        public async Task<IEnumerable<MeterReadingSummaryDto>> GetDailySummaryAsync(string deviceEui, DateTime? startDate, DateTime? endDate)
        {
            await Task.Delay(1);
            var query = _readings.Where(r => r.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase));

            if (startDate.HasValue)
            {
                query = query.Where(r => r.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.Timestamp <= endDate.Value);
            }

            return query
                .GroupBy(r => r.Timestamp.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new MeterReadingSummaryDto
                {
                    Date = g.Key,
                    TotalConsumption = g.Sum(r => r.Consumption),
                    AverageFlowRate = g.Average(r => r.FlowRate),
                    MaxReading = g.Max(r => r.ReadingValue),
                    MinReading = g.Min(r => r.ReadingValue),
                    Count = g.Count()
                });
        }

        public async Task<MeterReadingDto> CreateReadingAsync(CreateMeterReadingDto createDto)
        {
            await Task.Delay(1);

            var reading = new MeterReading
            {
                Id = _nextId++,
                DeviceEui = createDto.DeviceEui,
                Timestamp = createDto.Timestamp,
                ReadingValue = createDto.ReadingValue,
                Consumption = createDto.Consumption,
                FlowRate = createDto.FlowRate,
                BatteryLevel = createDto.BatteryLevel,
                Rssi = createDto.Rssi,
                Snr = createDto.Snr,
                IsEstimated = createDto.IsEstimated,
                Quality = createDto.Quality,
                PayloadHex = createDto.PayloadHex
            };

            _readings.Add(reading);
            return MapToDto(reading);
        }

        private static MeterReadingDto MapToDto(MeterReading reading)
        {
            return new MeterReadingDto
            {
                Id = reading.Id,
                DeviceEui = reading.DeviceEui,
                Timestamp = reading.Timestamp,
                ReadingValue = reading.ReadingValue,
                Consumption = reading.Consumption,
                FlowRate = reading.FlowRate,
                BatteryLevel = reading.BatteryLevel,
                Rssi = reading.Rssi,
                Snr = reading.Snr,
                IsEstimated = reading.IsEstimated,
                Quality = reading.Quality,
                PayloadHex = reading.PayloadHex
            };
        }
    }
}
