using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Instructors;
using System.Diagnostics.CodeAnalysis;

namespace GraphQLDemo.API.Schema.Queries;

public class CourseType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }

    [GraphQLIgnore]
    public Guid InstructorId { get; set; }

    [GraphQLNonNullType]
    public async Task<InstructorType> Instructor([Service] InstructorRepository instructorRepository)
    {
        InstructorDTO instructorDTO =  await instructorRepository.GetById(InstructorId);
        
        return new InstructorType()
        {
            Id = instructorDTO.Id,
            FirstName = instructorDTO.FirstName,
            LastName = instructorDTO.LastName,
            Salary = instructorDTO.Salary,
        }
    }
    public IEnumerable<StudentType> Students { get; set; }
}
