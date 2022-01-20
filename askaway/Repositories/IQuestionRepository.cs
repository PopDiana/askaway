using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askaway.Models;

namespace askaway.Repositories
{
    public interface IQuestionRepository: IDisposable
    {
        IEnumerable<Question> GetQuestions();
        Question GetQuestionByID(int questionId);
        void InsertQuestion(Question question);
        void DeleteQuestion(int questionID);
        void UpdateQuestion(Question question);
        IQueryable<Question> GetQuestionsQuery();
        void Save();
    }
}
