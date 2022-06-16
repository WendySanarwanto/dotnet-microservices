using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController: ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly ILogger<PlatformController> _logger;

        public PlatformController(IPlatformRepo platformRepo, IMapper mapper, 
            ICommandDataClient commandDataClient, ILogger<PlatformController> logger)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms() {
            _logger.LogInformation("--> Getting Platforms ...");
            var allPlatforms = _platformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(allPlatforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        [ProducesResponseType(typeof(PlatformReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PlatformReadDto> GetPlatformById(int id) {
            var platformItem = _platformRepo.GetPlatformById(id);
            if (platformItem != null){
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        } 

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto newPlatform){
            _logger.LogInformation("--> Creating a new Platform: "+newPlatform.Name+", "+newPlatform.Publisher);
            if (newPlatform != null) {
                var platformModel = _mapper.Map<Platform>(newPlatform);
                _platformRepo.CreatePlatform(platformModel);
                _platformRepo.SaveChanges();

                var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

                try {
                    await _commandDataClient.SendPlatformToCommand(platformReadDto);
                } catch(Exception ex){
                    _logger.LogError($"---> Could not send synchronously: {ex.Message}");
                }

                return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto );
            }

            return BadRequest();
        }
    }
}