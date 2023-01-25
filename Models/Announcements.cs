using System.ComponentModel.DataAnnotations;

namespace EDUZilla.Models
{
    public class Announcements
    {
        [Key]
        public int Id { get; set; }
        public string Topic { get; set; }   
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public Teacher? Sender { get; set; }
        public Class? Receiver { get; set; }


    }
}
