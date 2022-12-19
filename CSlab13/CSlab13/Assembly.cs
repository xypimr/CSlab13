#nullable enable
using System.Collections.Generic;

namespace CSlab13
{
    public class Assembly
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Part> Parts { get; set; } = new();
    }
}