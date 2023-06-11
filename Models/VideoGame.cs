using System.ComponentModel.DataAnnotations;

namespace MVC_VideoGames.Models
{
    public class VideoGame
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public bool IsMultiplayer { get; set; }
        public List<Review> Reviews { get; set; }
        public VideoGame()
        {
            Reviews = new List<Review>();
        }
    }
}
