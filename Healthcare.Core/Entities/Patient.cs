﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.Entities
{
    public class Patient
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? MedicalHistory { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
    } 
}
