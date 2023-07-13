using Healthcare.Core.DTOs;
using Healthcare.Core.Entities;
using Healthcare.Core.Services;
using Healthcare.Core.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Healthcare.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/V{version:apiversion}/[controller]")]
    [ApiController]
  //  [Authorize]
    public class AppointmentsController : BaseController
    {
        private readonly IGenericService<Appointment> _appointmentService;
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsController(IGenericService<Appointment> appointmentService, IUnitOfWork unitOfWork)
        {
            _appointmentService = appointmentService;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> AppointmentList()
        {
            var list=await _appointmentService.GetAllAsync();
            return CreateActionResult(CustomResponseDto<List<Appointment>>.Success(200, list.ToList()));
        }
    }
}
