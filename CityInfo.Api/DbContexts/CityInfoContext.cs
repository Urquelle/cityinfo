using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.DbContexts {
    public class CityInfoContext : DbContext {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<City>().HasData(
                new City("Mannheim") {
                    Id = 1,
                    Description = "Quadratestadt wo isch wohne tu"
                },

                new City("Berlin") {
                    Id = 2,
                    Description = "Hauptstadt des Landes wo isch lebe tu",
                },

                new City("Paris") {
                    Id = 3,
                    Description = "Hauptstadt von Frankreich"
                }
            );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Wasserturm") {
                    Id = 1,
                    CityId = 1,
                    Description = "Wasserturm am Ende der Planken mit einem Park"
                },

                new PointOfInterest("Rosengarten") {
                    Id = 2,
                    CityId = 1,
                    Description = "Veranstaltungszentrum am Ring"
                },

                new PointOfInterest("Reichstag") {
                    Id = 3,
                    CityId = 2,
                    Description = "Demokratie wird hier gemacht"
                },

                new PointOfInterest("Brandenburger Tor") {
                    Id = 4,
                    CityId = 2,
                    Description = "Großes Tor mit Checkpoint Charlie in der Nähe(?)"
                },

                new PointOfInterest("Eiffelturm") {
                    Id = 5,
                    CityId = 3,
                    Description = "Großer Turm den man besichtigen kann"
                },

                new PointOfInterest("Notre Dame") {
                    Id = 6,
                    CityId = 3,
                    Description = "Römisch-katholische Kathedrale"
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
