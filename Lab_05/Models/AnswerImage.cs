using System.ComponentModel.DataAnnotations;

namespace Lab_05.Models
{
    public enum Question
    {
        Earth, Computer
    }

    public class AnswerImage
    {
        [Required]
        public int AnswerImageId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        [Required]
        public Question Question { get; set; }


    }
}
