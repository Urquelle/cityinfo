namespace CityInfo.Api.Models {
    /// <summary>
    /// daten für die stadt, ohne poi
    /// </summary>
    public class CityWithoutPointsOfInterestDto {
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
    }
}
