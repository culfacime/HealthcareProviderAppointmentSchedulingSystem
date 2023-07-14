using AutoMapper;
using Healthcare.Core.DTOs;
using Healthcare.Core.DTOs.EntityDtos;
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
        private IMapper _mapper;
        public AppointmentsController(IGenericService<Appointment> appointmentService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var list=await _appointmentService.GetAllAsync();
            return CreateActionResult(CustomResponseDto<List<Appointment>>.Success(200, list.ToList()));
        }


     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _appointmentService.FirstOrDefaultAsync(x => x.AppointmentId == id);
            return CreateActionResult(CustomResponseDto<Appointment>.Success(200, appointment));
        }

        /// <summary>
        /// Allows users creating new appointment
        /// </summary>
        /// <param name="appointmentModel"></param>
        /// <returns>Created appointment</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDto appointmentModel)
        {
            var appointment = _mapper.Map<Appointment>(appointmentModel);

            var newAppointment = await _appointmentService.AddAsync(appointment);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<Appointment>.Success(200, newAppointment));
        }

        /// <summary>
        /// Updating appointments
        /// </summary>
        /// <param name="appointmentModel"></param>
        /// <returns>Updated appointment</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentDto appointmentModel)
        {
            var appointment = await _appointmentService.FirstOrDefaultAsync(x => x.AppointmentId == appointmentModel.AppointmentId);
           
            appointment=_mapper.Map<Appointment>(appointmentModel);

            await _appointmentService.UpdateAsync(appointment);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<Appointment>.Success(200, appointment));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentService.FirstOrDefaultAsync(x => x.AppointmentId == id);

            await _appointmentService.RemoveAsync(appointment);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(200, new List<string> { "Appointment has been successfully deleted." }));
        }

    }
}
