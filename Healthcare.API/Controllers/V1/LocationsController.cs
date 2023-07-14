using AutoMapper;
using Healthcare.Core.DTOs;
using Healthcare.Core.DTOs.EntityDtos;
using Healthcare.Core.Entities;
using Healthcare.Core.Services;
using Healthcare.Core.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Healthcare.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/V{version:apiversion}/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : BaseController
    {
        private readonly IGenericService<Location> _locationService;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public LocationsController(IGenericService<Location> locationService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _locationService = locationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var list = await _locationService.GetAllAsync();
            return CreateActionResult(CustomResponseDto<List<Location>>.Success(200, list.ToList()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location = await _locationService.FirstOrDefaultAsync(x => x.LocationId == id);
            return CreateActionResult(CustomResponseDto<Location>.Success(200, location));
        }

        /// <summary>
        /// Allows users creating new location
        /// </summary>
        /// <param name="locationModel"></param>
        /// <returns>Created location</returns>
        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] LocationDto locationModel)
        {
            var location = _mapper.Map<Location>(locationModel);

            var newLocation = await _locationService.AddAsync(location);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<Location>.Success(200, newLocation));
        }

       /// <summary>
       /// Updating locations
       /// </summary>
       /// <param name="locationModel"></param>
       /// <returns>Updated location</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationDto locationModel)
        {
            var location = await _locationService.FirstOrDefaultAsync(x => x.LocationId == locationModel.LocationId);
            
            location.LocationName=locationModel.LocationName;
            
            await _locationService.UpdateAsync(location);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<Location>.Success(200, location));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _locationService.FirstOrDefaultAsync(x => x.LocationId == id);

            if (location is null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(404, new List<string> { "Not found" }));
            }


            await _locationService.RemoveAsync(location);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(200,new List<string> { "Location has been successfully deleted." }));
        }
    }
}
