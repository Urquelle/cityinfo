namespace CityInfo.Api.Models {
    /// <summary>
    /// daten einer stadt inklusive der poi
    /// </summary>
    public class CityDto {
    /// <summary>
    /// id der stadt
    /// </summary>
        public int Id {
            get; set;
        }

        /// <summary>
        /// name der stadt
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// beschreibung der stadt
        /// </summary>
        public string? Description {
            get; set;
        }

        /// <summary>
        /// anzahl der points of interest
        /// </summary>
        public int NumberOfPointsOfInterest {
            get {
                return PointsOfInterest.Count;
            }
        }

        /// <summary>
        /// points of interest der stadt
        /// </summary>
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
    }
}
