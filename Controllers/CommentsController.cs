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
    public class CommentsController : ControllerBase
    {
        private readonly CommentRepository _commentRepository;
        private readonly AnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public CommentsController(CommentRepository commentRepository, AnswerRepository answerRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        // cevap idsine göre yorum listeleme metodu

        [HttpGet("answer/{answerId}")]
        public async Task<IActionResult> GetCommentsByAnswer(int answerId)
        {
            var comments = await _commentRepository.GetCommentsByAnswerAsync(answerId);
            var commentDTOs = _mapper.Map<IEnumerable<CommentDTO>>(comments);
            return Ok(commentDTOs);
        }

        // yorum oluşturma metodu
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var answer = await _answerRepository.GetByIdAsync(model.AnswerId);
            if (answer == null)
                return NotFound("Answer not found");

            var comment = _mapper.Map<Comment>(model);
            comment.UserId = userId;
            comment.CreatedDate = DateTime.UtcNow;
            comment.IsActive = true;

            var createdComment = await _commentRepository.AddAsync(comment);
            var commentDTO = _mapper.Map<CommentDTO>(createdComment);
            return Ok(commentDTO);
        }

        // yorum silme metodu

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            var isAdmin = User.IsInRole("Admin");
            if (comment.UserId != userId && !isAdmin)
                return Forbid();

            comment.IsActive = false;
            await _commentRepository.UpdateAsync(comment);
            return NoContent();
        }
    }
}