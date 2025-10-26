using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class CreateMeterReadingDto
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string DeviceEui { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue, ErrorMessage = "讀值不可為負")]
        public decimal ReadingValue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Consumption { get; set; }

        [Range(0, double.MaxValue)]
        public decimal FlowRate { get; set; }

        [Range(0, 100)]
        public double BatteryLevel { get; set; }

        public int Rssi { get; set; }

        public double Snr { get; set; }

        public bool IsEstimated { get; set; }

        [StringLength(50)]
        public string Quality { get; set; } = "Good";

        [StringLength(512)]
        public string PayloadHex { get; set; } = string.Empty;
    }

    public class MeterReadingDto
    {
        public int Id { get; set; }
        public string DeviceEui { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public decimal ReadingValue { get; set; }
        public decimal Consumption { get; set; }
        public decimal FlowRate { get; set; }
        public double BatteryLevel { get; set; }
        public int Rssi { get; set; }
        public double Snr { get; set; }
        public bool IsEstimated { get; set; }
        public string Quality { get; set; } = string.Empty;
        public string PayloadHex { get; set; } = string.Empty;
    }

    public class MeterReadingSummaryDto
    {
        public DateTime Date { get; set; }
        public decimal TotalConsumption { get; set; }
        public decimal AverageFlowRate { get; set; }
        public decimal MaxReading { get; set; }
        public decimal MinReading { get; set; }
        public int Count { get; set; }
    }
}
