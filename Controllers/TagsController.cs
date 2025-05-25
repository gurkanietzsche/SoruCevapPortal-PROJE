using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortal.DTOs;
using SoruCevapPortal.Models;
using SoruCevapPortal.Repositories;


namespace SoruCevapPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly TagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagsController(TagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        // etiketleri listeleme metodu
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _tagRepository.GetTagsWithQuestionCountAsync();
            var tagDTOs = _mapper.Map<IEnumerable<TagDTO>>(tags);
            return Ok(tagDTOs);
        }

        // id ye göre etiketleri listeleme metodu
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                return NotFound();

            var tagDTO = _mapper.Map<TagDTO>(tag);
            return Ok(tagDTO);
        }

        // etiket oluşturma metodu

        [HttpPost]
        [Authorize(Roles = "Admin,Questioner")]
        public async Task<IActionResult> CreateTag([FromBody] TagCreateDTO model)
        {
            // Model doğrulama kontrolü
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tag = _mapper.Map<Tag>(model);

            // Eğer Description null ise varsayılan bir değer atayalım
            if (string.IsNullOrEmpty(tag.Description))
            {
                tag.Description = $"{model.Name} etiketi için açıklama"; // Varsayılan değer
            }

            var createdTag = await _tagRepository.AddAsync(tag);
            var tagDTO = _mapper.Map<TagDTO>(createdTag);
            return CreatedAtAction(nameof(GetTag), new { id = createdTag.Id }, tagDTO);
        }

        // etiket güncelleme metodu

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Questioner")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] TagCreateDTO model)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                return NotFound();

            _mapper.Map(model, tag);
            await _tagRepository.UpdateAsync(tag);
            return NoContent();
        }

        // etiket silme metodu

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Questioner")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                return NotFound();

            await _tagRepository.DeleteAsync(tag);
            return NoContent();
        }
    }
}