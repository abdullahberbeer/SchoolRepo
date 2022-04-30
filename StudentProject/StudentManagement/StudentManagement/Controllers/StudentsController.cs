using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.DomainModels;
using StudentManagement.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IImageRepository IImageRepository;
        private readonly IMapper mapper;
        public StudentsController(IStudentRepository studentRepository,IImageRepository IImageRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.IImageRepository = IImageRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
          var students= await studentRepository.GetAllStudentsAsync();
            return Ok(mapper.Map<List<Student>>(students));
        }
        [HttpGet]
        [Route("[controller]/{studentId:guid}"),ActionName("GetAllStudentAsync")]
        public async Task<IActionResult> GetAllStudentAsync([FromRoute] Guid studentId)
        {
            var student = await studentRepository.GetAllStudentAsync(studentId);
            if (student==null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Student>(student));
        }
        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> updateStudentAsync([FromRoute] Guid studentId,[FromBody] UpdateStudentRequest request)
        {
            if (await studentRepository.Exists(studentId))
            {
              var updatedStudent= await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));
                if (updatedStudent!=null)
                {
                    return Ok(mapper.Map<Student>(updatedStudent));
                }
            }
            return NotFound();
            
        }
        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await studentRepository.Exists(studentId))
            {
                var student = await studentRepository.DeleteStudent(studentId);
                if (student!=null)
                {
                    return Ok(mapper.Map<Student>(student));
                }
            }
            return NotFound();
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
          var student=  await studentRepository.AddStudent(mapper.Map<DataModels.Student>(request));

            return CreatedAtAction(nameof(GetAllStudentAsync), new { studentId = student.Id }, mapper.Map<Student>(student));
        }
        
        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId,IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
                ".jpeg",
                ".png",
                ".gif",
                ".jpg"
            };

            if (profileImage!=null && profileImage.Length>0)
            {
                var extensions = Path.GetExtension(profileImage.FileName);

                if (validExtensions.Contains(extensions))
                {
                    if(await studentRepository.Exists(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                        var fileImagePath = await IImageRepository.Upload(profileImage, fileName);
                        if (await studentRepository.UpdateProfileImage(studentId,fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error Uploading Image");
                    }
                }
                return BadRequest("Hatalı format");

            }

            return NotFound();
        }
    }
}
