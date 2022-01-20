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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace askaway.Controllers
{
    public class QuestionController : Controller
    {
        private IQuestionRepository questionRepository;
        private ICategoryRepository categoryRepository;
        private IAnswerRepository answerRepository;
        private UserManager<ApplicationUser> _userManager;

        public QuestionController(UserManager<ApplicationUser> userManager)
        {
            this.questionRepository = new QuestionRepository(new askawayDbContext());
            this.categoryRepository = new CategoryRepository(new askawayDbContext());
            this.answerRepository = new AnswerRepository(new askawayDbContext());
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Search")]
        public ViewResult Search(string searchString)
        {

            var answers = from a in answerRepository.GetAnswers().Where(e => e.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                          select a;

            var questions = from q in questionRepository.GetQuestions().Where(e => e.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase) || e.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                            select q;

            var noAnswers = answers.Count(a => a.AnswerId != 0);
            var noQuestions = questions.Count(q=> q.QuestionId != 0);

            var categories = from c in categoryRepository.GetCategories()
                             select c;

            ViewBag.categories = categories;

            var users = _userManager.Users.ToList();
            ViewBag.users = users;

            ViewBag.resultedQuestions = questions;
            ViewBag.resultedAnswers = answers;
            ViewBag.noAnswers = noAnswers;
            ViewBag.noQuestions = noQuestions;
            ViewBag.searchString = searchString;
            return View();
        }

        [Route("")]
        [Route("Questions")]
        public async Task<ViewResult> Index(string byCategory, bool popular, bool withoutAnswer, bool unstarred, int? pageNumber)
        {

            var users = _userManager.Users.ToList();
            ViewBag.users = users;
            ViewBag.noUsers = users.Count();

            var questions = from q in questionRepository.GetQuestions()
                            select q;


            int noQuestions = 0;
            int noQuestionsAnswered = 0;

            foreach(var question in questions)
            {
                noQuestions++;
                if(question.AnswersNumber > 0)
                {
                    noQuestionsAnswered++;
                }
            }         

            float percentage = 0;

            if (noQuestions != 0)
            {
                percentage = (noQuestionsAnswered * 100) / noQuestions;
            }

            string answered = percentage.ToString("0.00");

            ViewBag.answered = answered;

            var answers = from a in answerRepository.GetAnswers()
                          select a;

            int noAnswers = 0;

            foreach(var answer in answers)
            {
                noAnswers++;
            }

            ViewBag.noAnswers = noAnswers;

            var categories = from c in categoryRepository.GetCategories()
                             select c;

            ViewBag.categories = categories;

            questions = questions.OrderByDescending(q => q.Date);

            if(byCategory != null && byCategory != "All Categories")
            {
                questions = questions.Where(q => q.Category.CategoryName == byCategory);
                ViewBag.category = byCategory;
            }

            if(popular == true)
            {
                questions = questions.OrderByDescending(q => q.AnswersNumber);
            }

            if(withoutAnswer == true)
            { 
                questions = questions.Where(q => q.AnswersNumber == 0);
            }


            if(unstarred == true)
            {       
                questions = questions.Where(q => q.HasStarredAnswer == false);
            }


            noQuestions = questions.Count(q => q.QuestionId != 0);
            ViewBag.noQuestions = noQuestions;

            int pageSize = 8;
            return View(await PaginatedList<Question>.CreateAsync(questions, pageNumber ?? 1, pageSize));

        }

        [Route("Question/{id}/Details")]
        public ViewResult Details(int id)
        {

            Question question = questionRepository.GetQuestionByID(id);
            IEnumerable<Answer> answers = answerRepository.GetAnswersByQuestionID(id);
            ViewBag.questionAnswers = answers;

            var users = _userManager.Users.ToList();
            ViewBag.users = users;

            Utilities.Instance.setQuestion(question);

            var categories = from c in categoryRepository.GetCategories()
                             select c;

            ViewBag.categories = categories;
            bool starredAnswer = false;

            foreach(var answer in answers)
            {
                if(answer.Starred == true)
                {
                    starredAnswer = true;
                }
            }

            ViewBag.starredAnswer = starredAnswer;

            return View(question);
        }

        [Authorize]
        [Route("Question/Create")]
       
        public ActionResult Create()
        {

            ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryName");
            return View();
        }


        [Authorize]
        [Route("Question/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("QuestionId,Title,Text,Date,AnswersNumber,Picture,Anonymous,CategoryId,UserId")] Question question)
        {

            if (ModelState.IsValid)
            {
               
                question.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userManager.Users.FirstOrDefault(u => u.Id == question.UserId);
                user.QuestionsAsked++;
                await _userManager.UpdateAsync(user);
                question.AnswersNumber = 0;
                question.HasStarredAnswer = false;
                question.Date = DateTime.Now;

                questionRepository.InsertQuestion(question);
                questionRepository.Save();
               
                return RedirectToAction("Index");
            }
            return View(question);
        }


        [Authorize]
        [Route("Question/{id}/Edit")]
        public ActionResult Edit(int id)
        {

            ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryName");
            Question question = questionRepository.GetQuestionByID(id);
            Utilities.Instance.setUserId(question.UserId);
            Utilities.Instance.setNumberOfAnswers((int)question.AnswersNumber);
            if(User.FindFirstValue(ClaimTypes.NameIdentifier) == question.UserId || User.IsInRole("Admin"))
            {
                return View(question);
            }
            else
            {
                Question q = Utilities.Instance.getQuestion();
                return Redirect("~/Question/" + q.QuestionId.ToString() + "/Details");
                
            }
        }



        [Authorize]
        [Route("Question/{id}/Edit")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("QuestionId,Title,Text,Date,AnswersNumber,Picture,Anonymous,CategoryId,UserId")] Question question)
        {

            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    question.UserId = Utilities.Instance.getUserId();
                    question.AnswersNumber = Utilities.Instance.getNumberOfAnswers();
                    question.Date = DateTime.Now;
                    questionRepository.UpdateQuestion(question);
                    questionRepository.Save();
                }
                catch (DataException)
                {
                    
                    ModelState.AddModelError(string.Empty, "Unable to save changes. Try again");
                }

                Question q = Utilities.Instance.getQuestion();
                return Redirect("~/Question/" + q.QuestionId.ToString() + "/Details");
            }
          
            return View(question);
        }


        [Authorize]
        [Route("Question/{id}/Delete")]
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Question question = questionRepository.GetQuestionByID(id);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == question.UserId || User.IsInRole("Admin"))
            {
                return View(question);
            }
            else
            {
                
                return Redirect("~/Questions");
            }
        }

        [Route("Question/{id}/Delete")]
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Question q = Utilities.Instance.getQuestion();

            try
            {
                Question question = questionRepository.GetQuestionByID(id);
                var user = _userManager.Users.FirstOrDefault(u => u.Id == question.UserId);
                user.QuestionsAsked--;
                await _userManager.UpdateAsync(user);
                questionRepository.DeleteQuestion(id);
                questionRepository.Save();
            }
            catch (DataException)
            {
                
                return Redirect("~/Question/" + q.QuestionId.ToString() + "/Details");
            }
           
            return Redirect("~/Question/" + q.QuestionId.ToString() + "/Details");
        }

        protected override void Dispose(bool disposing)
        {
            questionRepository.Dispose();
            base.Dispose(disposing);
        }
       
    }
}
