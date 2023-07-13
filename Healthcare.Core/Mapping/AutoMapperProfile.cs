using AutoMapper;
using Healthcare.Core.DTOs.EntityDtos;
using Healthcare.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
      
            CreateMap<Patient, PatientDto>().ReverseMap();

            CreateMap<Location, LocationDto>().ReverseMap();
        }
    }
}
