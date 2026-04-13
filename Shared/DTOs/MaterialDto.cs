using System;

namespace Shared.DTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
