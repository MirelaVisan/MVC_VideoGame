using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_VideoGames.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Date { get; set; }
        public Review()
        {
            Date = DateTime.Now;
        }
    }
}
