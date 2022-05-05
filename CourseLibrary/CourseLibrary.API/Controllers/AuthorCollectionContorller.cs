using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Model;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionContorller: ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibrarayRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionContorller(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            _courseLibrarayRepository = courseLibraryRepository ??
                    throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetAuthorsCollection")]
        public IActionResult GetAuthorCollection(
            [FromRoute]
            [ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }

            var authorEntities = _courseLibrarayRepository.GetAuthors(ids);

            if(ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return Ok(authorsToReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<Author>> CreateAuthorCollection(
            IEnumerable<AuthorForCreationDto> authorColleciton)
        {
            var authorEntities = _mapper.Map<IEnumerable<Entities.Author>>(authorColleciton);
            foreach(var author in authorEntities)
            {
                _courseLibrarayRepository.AddAuthor(author);
            }
            _courseLibrarayRepository.Save();

            var authorsCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var idsAsString = string.Join(",", authorsCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorsCollection",
                new { ids = idsAsString },
                authorsCollectionToReturn);
        }
    }
}
