using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortal.DTOs;
using SoruCevapPortal.Models;
using SoruCevapPortal.Repositories;
using System.Security.Claims;

namespace SoruCevapPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;
        private readonly TagRepository _tagRepository;
        private readonly IMapper _mapper;

        public QuestionsController(
            QuestionRepository questionRepository,
            TagRepository tagRepository,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        // soruları listeleme metodu

        [HttpGet]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _questionRepository.GetQuestionsWithDetailsAsync();
            var questionDTOs = _mapper.Map<IEnumerable<QuestionDTO>>(questions);
            return Ok(questionDTOs);
        }

        // soruları id ye göre listeleme metodu

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(int id)
        {
            var question = await _questionRepository.GetQuestionWithDetailsAsync(id);
            if (question == null)
                return NotFound();

            await _questionRepository.IncrementViewCountAsync(id);
            var questionDTO = _mapper.Map<QuestionDTO>(question);
            return Ok(questionDTO);
        }

        // soru oluşturma metodu
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var question = new Question
            {
                Title = model.Title,
                Content = model.Content,
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                ViewCount = 0,
                Tags = new List<Tag>()
            };

            // tag'leri manuel olarak işleme ve olşturma
            if (model.Tags != null && model.Tags.Any())
            {
                foreach (var tagName in model.Tags)
                {
                    var tag = await _tagRepository.GetByNameAsync(tagName);
                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        await _tagRepository.AddAsync(tag);
                    }
                    question.Tags.Add(tag);
                }
            }

            var createdQuestion = await _questionRepository.AddAsync(question);
            var questionDTO = _mapper.Map<QuestionDTO>(createdQuestion);
            return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, questionDTO);
        }

        // id ye göre soruyu güncelleme metodu
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionUpdateDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                return NotFound();

            if (question.UserId != userId)
                return Forbid();

            _mapper.Map(model, question);
            question.UpdatedDate = DateTime.UtcNow;

            if (model.Tags != null)
            {
                question.Tags?.Clear();
                question.Tags = new List<Tag>();
                foreach (var tagName in model.Tags)
                {
                    var tag = await _tagRepository.GetByNameAsync(tagName);
                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        await _tagRepository.AddAsync(tag);
                    }
                    question.Tags.Add(tag);
                }
            }

            await _questionRepository.UpdateAsync(question);
            return NoContent();
        }

        // idye göre soru silme metodu

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                return NotFound();

            var isAdmin = User.IsInRole("Admin");
            if (question.UserId != userId && !isAdmin)
                return Forbid();

            question.IsActive = false;
            await _questionRepository.UpdateAsync(question);
            return NoContent();
        }

        // kullanıcı id sine göre cevapları listeleme metodu

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetQuestionsByUser(string userId)
        {
            var questions = await _questionRepository.GetQuestionsByUserAsync(userId);
            var questionDTOs = _mapper.Map<IEnumerable<QuestionDTO>>(questions);
            return Ok(questionDTOs);
        }
    }
}