using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers {
    [Route("api/v{version:apiVersion}/cities/{cityId}/[controller]")]
    [Authorize(Policy = "MustBeFromMannheim")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
            ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId) {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"stadt mit der id {cityId} konnte nicht gefunden werden");

                return NotFound();
            }

            var pointsOfInterest = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId) {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"stadt mit der id {cityId} konnte nicht gefunden werden");

                return NotFound();
            }

            var result = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (result == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(result));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest) {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"stadt mit der id {cityId} konnte nicht gefunden werden");

                return NotFound();
            }

            var newEntry = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestAsync(cityId, newEntry);
            await _cityInfoRepository.SaveChangesAsync();

            var persistedNewEntry = _mapper.Map<PointOfInterestDto>(newEntry);

            return CreatedAtRoute("GetPointOfInterest", new {
                cityId = cityId,
                pointOfInterestId = persistedNewEntry.Id
            }, persistedNewEntry);
        }

        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest) {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"stadt mit der id {cityId} konnte nicht gefunden werden");

                return NotFound();
            }

            var entry = _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (entry == null) {
                return NotFound();
            }

            await _mapper.Map(pointOfInterest, entry);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patch) {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"stadt mit der id {cityId} konnte nicht gefunden werden");

                return NotFound();
            }

            var entry = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (entry == null) {
                return NotFound();
            }

            var entryToPatch = _mapper.Map<PointOfInterestForUpdateDto>(entry);

            patch.ApplyTo(entryToPatch, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(entryToPatch)) {
                return BadRequest(ModelState);
            }

            _mapper.Map(entryToPatch, entry);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId) {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"stadt mit der id {cityId} konnte nicht gefunden werden");

                return NotFound();
            }

            var entry = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (entry == null) {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(entry);
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("poi wurde gelöscht", $"point-of-interest {entry.Name} mit der id {pointOfInterestId} wurde gelöscht");

            return NoContent();
        }
    }
}
