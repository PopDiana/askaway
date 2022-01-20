using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace askaway.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        [Required]
        public string Text { get; set; }
        public bool? Starred { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public bool? Anonymous { get; set; }
        public byte[] Picture { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }

        public Question Question { get; set; }
    }
}
