using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class CreateLoRaDeviceDto
    {
        [Required(ErrorMessage = "Device EUI 為必填欄位")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Device EUI 必須為 16 碼十六進位字串")]
        public string DeviceEui { get; set; } = string.Empty;

        [Required(ErrorMessage = "設備名稱為必填欄位")]
        [StringLength(200, ErrorMessage = "設備名稱最長 200 個字元")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "儀表種類為必填欄位")]
        [StringLength(100)]
        public string MeterType { get; set; } = string.Empty;

        [Required(ErrorMessage = "韌體版本為必填欄位")]
        [StringLength(50)]
        public string FirmwareVersion { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "電量必須介於 0-100 之間")]
        public double BatteryLevel { get; set; }

        [Required(ErrorMessage = "安裝位置為必填欄位")]
        [StringLength(500)]
        public string InstallLocation { get; set; } = string.Empty;

        [Required(ErrorMessage = "閘道器 EUI 為必填欄位")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "閘道器 EUI 必須為 16 碼十六進位字串")]
        public string GatewayEui { get; set; } = string.Empty;

        [Required(ErrorMessage = "Application Key 為必填欄位")]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "Application Key 必須為 32 碼十六進位字串")]
        public string ApplicationKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "頻段計畫為必填欄位")]
        [StringLength(50)]
        public string FrequencyPlan { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "告警門檻不可為負值")]
        public double AlertThreshold { get; set; }

        public bool SupportsClassC { get; set; }

        public DateTime InstallDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateLoRaDeviceDto : CreateLoRaDeviceDto
    {
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public double LastSignalStrength { get; set; }
    }

    public class UpdateLoRaDeviceStatusDto
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Range(0, 100)]
        public double? BatteryLevel { get; set; }

        [StringLength(16, MinimumLength = 16)]
        public string? GatewayEui { get; set; }

        public bool? IsActive { get; set; }

        public double? LastSignalStrength { get; set; }
    }

    public class LoRaDeviceDto
    {
        public int Id { get; set; }
        public string DeviceEui { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MeterType { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public double BatteryLevel { get; set; }
        public string InstallLocation { get; set; } = string.Empty;
        public DateTime InstallDate { get; set; }
        public DateTime LastCommunication { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
        public string GatewayEui { get; set; } = string.Empty;
        public string ApplicationKey { get; set; } = string.Empty;
        public string FrequencyPlan { get; set; } = string.Empty;
        public double AlertThreshold { get; set; }
        public bool SupportsClassC { get; set; }
        public double LastSignalStrength { get; set; }
        public decimal LastReadingValue { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
