namespace CSlab13
{
    public class Part
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int AssemblyId { get; set; }
        public Assembly Assembly { get; set; }
        public int DetailId { get; set; }
        public Detail Detail { get; set; }
        
    }
}