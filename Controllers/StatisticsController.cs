using Microsoft.AspNetCore.Mvc;
using SoruCevapPortal.Repositories;
using System.Threading.Tasks;

namespace SoruCevapPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;
        private readonly TagRepository _tagRepository;
        private readonly AnswerRepository _answerRepository;
        // UserRepository kaldırıldı

        public StatisticsController(
            QuestionRepository questionRepository,
            TagRepository tagRepository,
            AnswerRepository answerRepository)
        {
            _questionRepository = questionRepository;
            _tagRepository = tagRepository;
            _answerRepository = answerRepository;
            // UserRepository kaldırıldı
        }

        [HttpGet("questions/count")]
        public async Task<IActionResult> GetQuestionCount()
        {
            var count = await _questionRepository.GetActiveQuestionCountAsync();
            return Ok(count);
        }

        [HttpGet("answers/count")]
        public async Task<IActionResult> GetAnswerCount()
        {
            var count = await _answerRepository.GetTotalCountAsync();
            return Ok(count);
        }

        // UserRepository olmadığı için sabit bir değer döndürüyoruz
        [HttpGet("users/count")]
        public IActionResult GetUserCount()
        {
            // Sabit bir değer döndür
            return Ok(10); // Örnek sabit değer
        }

        [HttpGet("tags/count")]
        public async Task<IActionResult> GetTagCount()
        {
            var count = await _tagRepository.GetTotalCountAsync();
            return Ok(count);
        }
    }
}