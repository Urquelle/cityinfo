using CityInfo.Api.Models;

namespace CityInfo.Api {
    public class CitiesDataStore {
        public List<CityDto> Cities {
            get; set;
        }

        public CitiesDataStore() {
            Cities = new List<CityDto>() {
                new CityDto() {
                    Id = 1,
                    Name = "Mannheim",
                    Description = "Quadratestadt wo isch wohne tu",
                    PointsOfInterest = new List<PointOfInterestDto>() {
                        new PointOfInterestDto() {
                            Id = 1,
                            Name = "Wasserturm",
                            Description = "Wasserturm am Ende der Planken mit einem Park"
                        },

                        new PointOfInterestDto() {
                            Id = 2,
                            Name = "Rosengarten",
                            Description = "Veranstaltungszentrum am Ring"
                        }
                    }
                },

                new CityDto() {
                    Id = 2,
                    Name = "Berlin",
                    Description = "Hauptstadt des Landes wo isch lebe tu",
                    PointsOfInterest = new List<PointOfInterestDto>() {
                        new PointOfInterestDto() {
                            Id = 3,
                            Name = "Reichstag",
                            Description = "Demokratie wird hier verbrochen"
                        },

                        new PointOfInterestDto() {
                            Id = 4,
                            Name = "Brandenburger Tor",
                            Description = "Großes Tor mit Checkpoint Charlie in der Nähe(?)"
                        }
                    }
                }
            };
        }
    }
}
