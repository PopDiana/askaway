using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace askaway.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Summary is required.")]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int? AnswersNumber { get; set; }
        public byte[] Picture { get; set; }
        [Required]
        public bool? Anonymous { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public bool HasStarredAnswer { get; set; }

        public Category Category { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
