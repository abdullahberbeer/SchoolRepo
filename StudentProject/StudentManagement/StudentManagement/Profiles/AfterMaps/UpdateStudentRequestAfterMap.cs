using AutoMapper;
using StudentManagement.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels = StudentManagement.DataModels;

namespace StudentManagement.Profiles.AfterMaps
{
    public class UpdateStudentRequestAfterMap : IMappingAction<UpdateStudentRequest, DataModels.Student>
    {
        public void Process(UpdateStudentRequest source, DataModels.Student destination, ResolutionContext context)
        {
            destination.Address = new DataModels.Address()
            {
                PhysicalAdress = source.PhysicalAddress,
                PostalAdress = source.PostalAddress
            };
        }
    }
}
