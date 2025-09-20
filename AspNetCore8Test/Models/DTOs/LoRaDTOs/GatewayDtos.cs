using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class CreateLoRaGatewayDto
    {
        [Required(ErrorMessage = "Gateway EUI 為必填欄位")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Gateway EUI 必須為 16 碼十六進位字串")]
        public string GatewayEui { get; set; } = string.Empty;

        [Required(ErrorMessage = "閘道器名稱為必填欄位")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "安裝位置為必填欄位")]
        [StringLength(500)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "IP 位址為必填欄位")]
        [StringLength(50)]
        public string IpAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "韌體版本為必填欄位")]
        [StringLength(50)]
        public string FirmwareVersion { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "緯度需介於 -90 到 90 度")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "經度需介於 -180 到 180 度")]
        public double Longitude { get; set; }

        [Range(0, 100, ErrorMessage = "封包成功率需介於 0-100 之間")]
        public double PacketSuccessRate { get; set; }

        [Required]
        [StringLength(50)]
        public string BackhaulType { get; set; } = string.Empty;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateLoRaGatewayDto : CreateLoRaGatewayDto
    {
        public int Id { get; set; }
        public bool IsOnline { get; set; } = true;
        public string Status { get; set; } = "Normal";
        public int ConnectedDevices { get; set; }
    }

    public class UpdateLoRaGatewayStatusDto
    {
        public bool IsOnline { get; set; }

        [Range(0, 100)]
        public double? PacketSuccessRate { get; set; }

        public int? ConnectedDevices { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }
    }

    public class LoRaGatewayDto
    {
        public int Id { get; set; }
        public string GatewayEui { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime LastCommunication { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ConnectedDevices { get; set; }
        public double PacketSuccessRate { get; set; }
        public string BackhaulType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
