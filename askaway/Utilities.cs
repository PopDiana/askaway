using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askaway.Models;

namespace askaway
{
    public class Utilities
    {
        private Question currentQuestion = null;
        private int noAnswers = 0;
        private string userId;

        private static Utilities _instance = null;
        public static Utilities Instance
        {
            get
            {
                if (_instance == null) _instance = new Utilities();
                return _instance;
            }
        }

        public void setQuestion(Question question)
        {
            currentQuestion = question;
        }

        public void setNumberOfAnswers(int answers)
        {
            noAnswers = answers;
        }

        public int getNumberOfAnswers()
        {
            return noAnswers;
        }

        public void setUserId(string id)
        {
            userId = id;
        }

        public string getUserId()
        {
            return userId;
        }

        public Question getQuestion()
        {
            return currentQuestion;
        }
    }
}
