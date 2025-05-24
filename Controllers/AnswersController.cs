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
    public class AnswersController : ControllerBase
    {
        private readonly AnswerRepository _answerRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public AnswersController(
            AnswerRepository answerRepository,
            QuestionRepository questionRepository,
            IMapper mapper)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        // soru id sine göre cevapları listeleme

        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> GetAnswersByQuestion(int questionId)
        {
            var answers = await _answerRepository.GetAnswersByQuestionAsync(questionId);
            var answerDTOs = _mapper.Map<IEnumerable<AnswerDTO>>(answers);
            return Ok(answerDTOs);
        }

        // id ye göre cevabı listeleme

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswer(int id)
        {
            var answer = await _answerRepository.GetAnswerWithDetailsAsync(id);
            if (answer == null)
                return NotFound();

            var answerDTO = _mapper.Map<AnswerDTO>(answer);
            return Ok(answerDTO);
        }

        // cevap oluşturma metodu

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerCreateDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var question = await _questionRepository.GetByIdAsync(model.QuestionId);
            if (question == null)
                return NotFound("Question not found");

            var answer = _mapper.Map<Answer>(model);
            answer.UserId = userId;
            answer.CreatedDate = DateTime.UtcNow;
            answer.IsActive = true;
            answer.IsAccepted = false;

            var createdAnswer = await _answerRepository.AddAsync(answer);
            var answerDTO = _mapper.Map<AnswerDTO>(createdAnswer);
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswer.Id }, answerDTO);
        }

        // id ye göre cevap güncelleme metodu

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAnswer(int id, [FromBody] AnswerUpdateDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var answer = await _answerRepository.GetByIdAsync(id);

            if (answer == null)
                return NotFound();

            if (answer.UserId != userId)
                return Forbid();

            _mapper.Map(model, answer);
            answer.UpdatedDate = DateTime.UtcNow;

            await _answerRepository.UpdateAsync(answer);
            return NoContent();
        }

        // id ye göre cevap silme metodu

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var answer = await _answerRepository.GetByIdAsync(id);

            if (answer == null)
                return NotFound();

            var isAdmin = User.IsInRole("Admin");
            if (answer.UserId != userId && !isAdmin)
                return Forbid();

            answer.IsActive = false;
            await _answerRepository.UpdateAsync(answer);
            return NoContent();
        }

        // cevabı onaylama metodu

        [HttpPost("{id}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptAnswer(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var answer = await _answerRepository.GetAnswerWithDetailsAsync(id);

            if (answer == null)
                return NotFound();

            var question = await _questionRepository.GetByIdAsync(answer.QuestionId);
            if (question.UserId != userId)
                return Forbid();

            await _answerRepository.AcceptAnswerAsync(id);
            return NoContent();
        }
    }
}