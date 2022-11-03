using System.Text.Json;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers {
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper) {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities([FromQuery] string? name,
            [FromQuery] string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, maxCitiesPageSize);
            var (cities, pagination) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
        }

        /// <summary>
        /// Hole die Daten für die Stadt mit der angegebenen id
        /// </summary>
        /// <param name="id">die id der stadt, nach der gesucht werden soll</param>
        /// <param name="includePointsOfInterest">gibt an ob die poi ebenfalls angezeigt werden sollen</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Gibt die Daten der angefragten Stadt zurück</response>
        /// <response code="404">Stadt zu der angegebenen id konnte nicht ermittelt werden</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false) {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            if (city == null) {
                return NotFound();
            }

            if (includePointsOfInterest) {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
