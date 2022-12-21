#nullable enable
using System.Collections.Generic;

namespace CSlab13
{
    public class Building
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<AuditoriumGroup> AuditoriumGroups { get; set; } = new();
    }
}