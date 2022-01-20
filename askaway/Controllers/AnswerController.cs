using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using askaway.Models;
using askaway.Repositories;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace askaway.Controllers
{
    public class AnswerController : Controller
    {
        private IAnswerRepository answerRepository;
        private IQuestionRepository questionRepository;
        private UserManager<ApplicationUser> _userManager;
        

        public AnswerController(UserManager<ApplicationUser> userManager)
        {
            this.answerRepository = new AnswerRepository(new askawayDbContext());
            this.questionRepository = new QuestionRepository(new askawayDbContext());
            _userManager = userManager;
            
        }

        [Route("Answer/Create")]
        [Authorize]
        public ActionResult Create()
        {
            Question question = Utilities.Instance.getQuestion();
            return Redirect("~/Question/" + question.QuestionId.ToString() + "/Details");

        }

        [Authorize]
        [Route("Answer/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("AnswerId,Text,Starred,Date,Anonymous,Picture,QuestionId,UserId")] Answer answer)
        {
           
            if (ModelState.IsValid)
            {
                answer.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userManager.Users.FirstOrDefault(u => u.Id == answer.UserId);
                user.GivenAnswers++;
                await _userManager.UpdateAsync(user);
                answer.Starred = false;
                answer.Date = DateTime.Now;
                Question question = Utilities.Instance.getQuestion();
                answer.QuestionId = question.QuestionId;
                question.AnswersNumber++;
                questionRepository.UpdateQuestion(question);
                questionRepository.Save();
                answerRepository.InsertAnswer(answer);
                answerRepository.Save();
                return Redirect("~/Question/" + question.QuestionId.ToString() +"/Details");
            }
            return View(answer);
        }

        [Authorize]
        [Route("Answer/{id}/Edit")]
        public ActionResult Edit(int id)
        {
            
            Answer answer = answerRepository.GetAnswerByID(id);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == answer.UserId || User.IsInRole("Admin")){
                ViewData["QuestionId"] = new SelectList(questionRepository.GetQuestions(), "QuestionId", "Title");
                Utilities.Instance.setUserId(answer.UserId);
                return View(answer);
            }
            else
            {
                Question question = Utilities.Instance.getQuestion();
                return Redirect("~/Question/" + question.QuestionId.ToString() + "/Details");
            }
           
        }


        [Authorize]
        [Route("Answer/{id}/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("AnswerId,Text,Starred,Date,Anonymous,Picture,QuestionId,UserId")] Answer answer)
        {
            if (id != answer.AnswerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    answer.UserId = Utilities.Instance.getUserId();
                    answer.Date = DateTime.Now;
                    answer.Starred = false;
                    answer.QuestionId = Utilities.Instance.getQuestion().QuestionId;
                    answerRepository.UpdateAnswer(answer);
                    answerRepository.Save();



                }
                catch (DataException)
                {

                    ModelState.AddModelError(string.Empty, "Unable to save changes. Try again");
                }

                Question question = Utilities.Instance.getQuestion();
                return Redirect("~/Question/" + question.QuestionId.ToString() + "/Details");
            }

                return View(answer);
        }

      
        [Authorize]
        [HttpPost, ActionName("Star")]
        public async Task<ActionResult> Star(int id)
        {
            Answer answer = answerRepository.GetAnswerByID(id);
            Question question = answer.Question;
            bool alreadyStarred = false;
           
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == question.UserId)
            {
                IEnumerable<Answer> answers = answerRepository.GetAnswers();
                foreach(Answer item in answers)
                {                 
                        Answer a = item;
                        if(a.Starred == true)
                        {
                            alreadyStarred = true;
                            a.Starred = false;
                            var user = _userManager.Users.FirstOrDefault(u => u.Id == a.UserId);
                            user.StarsReceived--;
                            await _userManager.UpdateAsync(user);
                            answerRepository.UpdateAnswer(a);
                            answerRepository.Save();

                        }
                                     
                }
                
                answer.Starred = true;
                question.HasStarredAnswer = true;
                questionRepository.UpdateQuestion(question);
                questionRepository.Save();
                var starredAnswerUser = _userManager.Users.FirstOrDefault(u => u.Id == answer.UserId);
                starredAnswerUser.StarsReceived++;
                await _userManager.UpdateAsync(starredAnswerUser);
                answerRepository.UpdateAnswer(answer);
                answerRepository.Save();

                if (!alreadyStarred)
                {
                    var questionUser = _userManager.Users.FirstOrDefault(u => u.Id == answer.Question.UserId);
                    questionUser.StarsGiven++;
                    await _userManager.UpdateAsync(questionUser);
                }
               

            }
            Question q = Utilities.Instance.getQuestion();
            return Redirect("~/Question/" + q.QuestionId.ToString() + "/Details");
        }

        [Authorize]
        [Route("Answer/{id}/Delete")]
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Answer answer = answerRepository.GetAnswerByID(id);

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == answer.UserId || User.IsInRole("Admin"))
            {

                return View(answer);
            }
            else
            {
                Question question = Utilities.Instance.getQuestion();
                return Redirect("~/Question/" + question.QuestionId.ToString() + "/Details");
            }
        }

        [Authorize]
        [Route("Answer/{id}/Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Answer answer = answerRepository.GetAnswerByID(id);
                var user = _userManager.Users.FirstOrDefault(u => u.Id == answer.UserId);
                user.GivenAnswers--;
                await _userManager.UpdateAsync(user);
                Question question = Utilities.Instance.getQuestion();
                question.AnswersNumber--;
                questionRepository.UpdateQuestion(question);
                questionRepository.Save();
                answerRepository.DeleteAnswer(id);
                answerRepository.Save();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            Question q = Utilities.Instance.getQuestion();
            return Redirect("~/Question/" + q.QuestionId.ToString() + "/Details");
        }

        protected override void Dispose(bool disposing)
        {
            answerRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
