using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos {
    public class PlatformCreateDto
    {
        [Required]
        public string? Name { set; get; }

        [Required]
        public string? Publisher { set; get; }

        [Required]
        public string? Cost { set; get; }        
    }
}