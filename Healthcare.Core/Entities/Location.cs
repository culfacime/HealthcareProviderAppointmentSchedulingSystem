namespace Healthcare.Core.Entities
{
    public class Location
    {
        public int LocationId { get; set; }

        public string LocationName { get; set; } = null!;

        public virtual ICollection<Appointment> Appointment { get; set; }
    }
}
