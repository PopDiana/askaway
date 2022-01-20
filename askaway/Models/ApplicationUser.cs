using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askaway.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        public DateTime RegistrationDate { get; set; }
        public DateTime Birthday { get; set; }
        public int QuestionsAsked { get; set; }
        public int GivenAnswers { get; set; }
        public int StarsGiven { get; set; }
        public int StarsReceived { get; set; }
        

    }
}