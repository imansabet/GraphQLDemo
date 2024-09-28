﻿using Bogus;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Courses;

namespace GraphQLDemo.API.Schema.Queries;

public class Query
{
    private readonly CoursesRepository _coursesRepository;

    //private readonly Faker<InstructorType> _instructorFaker;
    //private readonly Faker<StudentType> _studentFaker;
    //private readonly Faker<CourseType> _courseFaker;
    //public Query()
    //{
    //    _instructorFaker = new Faker<InstructorType>()
    //        .RuleFor(c => c.Id, f => Guid.NewGuid())
    //        .RuleFor(c => c.FirstName, f => f.Name.FirstName())
    //        .RuleFor(c => c.LastName, f => f.Name.LastName())
    //        .RuleFor(c => c.Salary, f => f.Random.Double(0, 100000));
    //    _studentFaker = new Faker<StudentType>()
    //        .RuleFor(c => c.Id, f => Guid.NewGuid())
    //        .RuleFor(c => c.FirstName, f => f.Name.FirstName())
    //        .RuleFor(c => c.LastName, f => f.Name.LastName())
    //        .RuleFor(c => c.GPA, f => f.Random.Double(1, 4));
    //    _courseFaker = new Faker<CourseType>()
    //        .RuleFor(c => c.Id, f => Guid.NewGuid())
    //        .RuleFor(c => c.Name, f => f.Name.JobTitle())
    //        .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
    //        .RuleFor(c => c.Instructor, f => _instructorFaker.Generate())
    //        .RuleFor(c => c.Students, f => _studentFaker.Generate(3));
    //}
    public Query(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    public async Task<IEnumerable<CourseType>> GetCourses()
    {
        IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
        return courseDTOs.Select(c => new CourseType()
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            Instructor = new InstructorType()
            {
                Id = c.Instructor.Id,
                FirstName = c.Instructor.FirstName,
                LastName = c.Instructor.LastName,
                Salary = c.Instructor.Salary,
            }
        });
    }
    public async Task<CourseType> GetCourseByIdAsync(Guid id)
    {
        CourseDTO courseDTO = await _coursesRepository.GetById(id);
        return new CourseType()
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
            Instructor = new InstructorType()
            {
                Id = courseDTO.Instructor.Id,
                FirstName = courseDTO.Instructor.FirstName,
                LastName = courseDTO.Instructor.LastName,
                Salary = courseDTO.Instructor.Salary,
            }
        };
    }


    [GraphQLDeprecated("This Query is deprecated.")]
    public string Instructions => "First Query Type.";
}
