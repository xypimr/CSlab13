namespace CSlab13
{
    public class AuditoriumGroup
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public int AuditoriumId { get; set; }
        public Auditorium Auditorium { get; set; }
        
    }
}