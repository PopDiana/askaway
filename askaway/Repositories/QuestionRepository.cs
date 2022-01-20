using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askaway.Models;
using Microsoft.EntityFrameworkCore;

namespace askaway.Repositories
{
    public class QuestionRepository: IQuestionRepository, IDisposable
    {
        private askawayDbContext context;

        public QuestionRepository(askawayDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Question> GetQuestions()
        {
            
            return context.Questions.Include(q => q.Category).ToList();
        }

        public Question GetQuestionByID(int id)
        {          
            return context.Questions.Include(q => q.Category).FirstOrDefault(q => q.QuestionId == id);
                
        }
        public IQueryable<Question> GetQuestionsQuery()
        {
            return context.Questions.Include(q => q.Category);
        }

        public void InsertQuestion(Question question)
        {
            context.Questions.Add(question);
        }

        public void DeleteQuestion(int questionID)
        {
            Question question = context.Questions.Find(questionID);
            var answers = from a in context.Answers where a.QuestionId == questionID select a;
            foreach (var answer in answers)
            {
                context.Answers.Remove(answer);
            }
            
            context.Questions.Remove(question);
        }

        public void UpdateQuestion(Question question)
        {
            context.Entry(question).State = EntityState.Modified;
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
    }
}

