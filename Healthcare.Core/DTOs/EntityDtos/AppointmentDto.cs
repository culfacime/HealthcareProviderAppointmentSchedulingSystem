using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.DTOs.EntityDtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public int LocationId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool RemindPatientViaSms { get; set; }
        public bool RemindPatientViaEmail { get; set; }
        public bool Reminded { get; set; }
        public DateTime RemindingDate { get; set; }

        public PatientDto Patient { get; set; }
        public LocationDto Location { get; set; }

    }   
}
