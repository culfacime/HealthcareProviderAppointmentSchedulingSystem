namespace Healthcare.Core.DTOs.EntityDtos
{
    public class LocationDto
    {
        public int LocationId { get; set; }

        public string LocationName { get; set; } = null!;

        public List<AppointmentDto> Appointment { get; set; }
    }
}
