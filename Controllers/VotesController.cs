using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SoruCevapPortal.DTOs;
using SoruCevapPortal.Models;
using SoruCevapPortal.Repositories;
using System.Security.Claims;

namespace SoruCevapPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly VoteRepository _voteRepository;
        private readonly IMapper _mapper;

        public VotesController(VoteRepository voteRepository, IMapper mapper)
        {
            _voteRepository = voteRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] VoteDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            // kullanıcı oy vermiş mi kontrol et
            var existingVote = await _voteRepository.GetUserVoteAsync(userId, model.QuestionId, model.AnswerId);

            if (existingVote != null)
            {
                // oylamayı güncelle
                existingVote.IsUpvote = model.IsUpvote;
                await _voteRepository.UpdateAsync(existingVote);
            }
            else
            {
                // yeni oylama oluştur
                var vote = _mapper.Map<Vote>(model);
                vote.UserId = userId;
                vote.VoteDate = DateTime.UtcNow;
                await _voteRepository.AddAsync(vote);
            }

            // güncellenmiş oy sayıtısını döndür
            var voteCount = await _voteRepository.GetVoteCountAsync(model.QuestionId, model.AnswerId);
            return Ok(new { voteCount });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveVote([FromQuery] int? questionId, [FromQuery] int? answerId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var vote = await _voteRepository.GetUserVoteAsync(userId, questionId, answerId);
            if (vote == null)
                return NotFound();

            await _voteRepository.DeleteAsync(vote);

            // güncellenmiş oy sayıtısını döndür
            var voteCount = await _voteRepository.GetVoteCountAsync(questionId, answerId);
            return Ok(new { voteCount });
        }
    }
}