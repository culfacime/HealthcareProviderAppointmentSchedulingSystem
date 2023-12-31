﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public int LocationId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool RemindPatientViaSms { get; set; }
        public bool RemindPatientViaEmail { get; set; }
        public bool Reminded { get; set; }
        public DateTime RemindingDate { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Location Location { get; set; }

    }   
}
