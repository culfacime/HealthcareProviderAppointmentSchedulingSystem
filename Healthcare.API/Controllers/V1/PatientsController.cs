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
    public class PatientsController : BaseController
    {
        private readonly IGenericService<Patient> _patientService;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PatientsController(IGenericService<Patient> patientService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _patientService = patientService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var list=await _patientService.GetAllAsync();
            return CreateActionResult(CustomResponseDto<List<Patient>>.Success(200, list.ToList()));
        }


     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(Guid id)
        {
            var patient = await _patientService.FirstOrDefaultAsync(x => x.PatientId == id);
            return CreateActionResult(CustomResponseDto<Patient>.Success(200, patient));
        }

        /// <summary>
        /// Allows users creating new patient
        /// </summary>
        /// <param name="patientModel"></param>
        /// <returns>Created patient</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientDto patientModel)
        {
            var patient = _mapper.Map<Patient>(patientModel);

            var newPatient = await _patientService.AddAsync(patient);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<Patient>.Success(200, newPatient));
        }

        /// <summary>
        /// Updating patients
        /// </summary>
        /// <param name="patientModel"></param>
        /// <returns>Updated patient</returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePatient([FromBody] PatientDto patientModel)
        {
            var patient = await _patientService.FirstOrDefaultAsync(x => x.PatientId == patientModel.PatientId);
           
            patient=_mapper.Map<Patient>(patientModel);

            await _patientService.UpdateAsync(patient);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<Patient>.Success(200, patient));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _patientService.FirstOrDefaultAsync(x => x.PatientId == id);

            await _patientService.RemoveAsync(patient);
            _unitOfWork.Commit();

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(200, new List<string> { "Patient has been successfully deleted." }));
        }

    }
}
