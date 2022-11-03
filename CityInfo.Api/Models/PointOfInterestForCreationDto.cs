using System.ComponentModel.DataAnnotations;

namespace CityInfo.Api.Models {
    public class PointOfInterestForCreationDto {
        [Required(ErrorMessage = "Gib mir einen Namen")]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
