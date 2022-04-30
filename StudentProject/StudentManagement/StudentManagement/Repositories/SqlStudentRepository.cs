using Microsoft.EntityFrameworkCore;
using StudentManagement.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext context;
        public SqlStudentRepository(StudentAdminContext context)
        {
            this.context = context;
        }

       

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }
        public async Task<Student> GetAllStudentAsync(Guid studentId)
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Address)).FirstOrDefaultAsync(x => x.Id == studentId);
        }

        public async Task<List<Gender>> GetAllGendersAsync()
        {
            return await context.Gender.ToListAsync();
        }

        public async Task<bool> Exists(Guid studentId)
        {
          return await context.Student.AnyAsync(x => x.Id == studentId);
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existsStudent = await GetAllStudentAsync(studentId);
            if (existsStudent!=null)
            {
                existsStudent.FirstName = request.FirstName;
                existsStudent.LastName = request.LastName;
                existsStudent.DateOfBirth = request.DateOfBirth;
                existsStudent.Email = request.Email;
                existsStudent.Mobile = request.Mobile;
                existsStudent.GenderId = request.GenderId;
                existsStudent.Address.PhysicalAdress = request.Address.PhysicalAdress;
                existsStudent.Address.PostalAdress = request.Address.PostalAdress;

               await context.SaveChangesAsync();
                return existsStudent;
            }
            return null;
        }

        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var existsstudent = await GetAllStudentAsync(studentId);

            if (existsstudent!=null)
            {
                context.Student.Remove(existsstudent);
                await context.SaveChangesAsync();
                return existsstudent;
            }
            return null;
        }

        public async Task<Student> AddStudent(Student request)
        {
         var student=  await context.Student.AddAsync(request);
            await context.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            var student = await GetAllStudentAsync(studentId);
            if (student!=null)
            {
                student.ProfileImageUrl = profileImageUrl;
               await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
