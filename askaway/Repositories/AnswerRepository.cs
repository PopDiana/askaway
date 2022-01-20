using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askaway.Models;
using Microsoft.EntityFrameworkCore;

namespace askaway.Repositories
{
    public class AnswerRepository: IAnswerRepository, IDisposable
    {
        private askawayDbContext context;

        public AnswerRepository(askawayDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Answer> GetAnswers()
        {

            return context.Answers.Include(a => a.Question).ToList();
        }

        public Answer GetAnswerByID(int id)
        {
            return context.Answers.Include(a => a.Question).FirstOrDefault(a => a.AnswerId == id);

        }

        public void InsertAnswer(Answer answer)
        {
            context.Answers.Add(answer);
        }

        public void DeleteAnswer(int answerID)
        {
            Answer answer = context.Answers.Find(answerID);
            context.Answers.Remove(answer);
        }

        public void UpdateAnswer(Answer answer)
        {
            context.Entry(answer).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Answer> GetAnswersByQuestionID(int id)
        {

            return context.Answers.Include(a => a.Question).Where(a => a.QuestionId == id).ToList();
        }
    }

}

