#nullable enable
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CSlab13
{
    public class Auditorium
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int NumberOfSeats { get; set; }
        public string? Description { get; set; }
    }
}