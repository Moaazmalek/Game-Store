using System.ComponentModel.DataAnnotations;

namespace GameStore.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
    }
}