using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StudentManagement.DomainModels;

namespace StudentManagement.Profiles.AfterMaps
{
    public class AddStudentRequestAfterMap : IMappingAction<AddStudentRequest, DataModels.Student>
    {
        public void Process(AddStudentRequest source, DataModels.Student destination, ResolutionContext context)
        {
            destination.Id = Guid.NewGuid();
            destination.Address = new DataModels.Address()
            {
                Id = Guid.NewGuid(),
                PhysicalAdress = source.PhysicalAddress,
                PostalAdress = source.PostalAddress
            };
        }
    }
}
