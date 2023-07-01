using System.ComponentModel.DataAnnotations;

namespace CrudMetrics.Api.Models
{
    public class User : IExternalEntity
    {
        public Guid? ExternalId { get; set; }
        [Required]
        public String? Name { get; set; }
        public Address? Address { get; set; }
        [Range(0, Int32.MaxValue)]
        public Int32? Age { get; set; }
        public String? HairColor { get; set; }
        public ICollection<String>? FavoriteThings { get; set; }
    }
}
