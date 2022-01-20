using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askaway.Models;

namespace askaway.Repositories
{
    public interface IAnswerRepository: IDisposable
    {
        IEnumerable<Answer> GetAnswers();
        Answer GetAnswerByID(int answerId);
        void InsertAnswer(Answer answer);
        void DeleteAnswer(int answerID);
        void UpdateAnswer(Answer answer);
        void Save();
        IEnumerable<Answer> GetAnswersByQuestionID(int id);
    }
}
