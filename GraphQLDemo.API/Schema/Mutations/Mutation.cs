﻿using Bogus.DataSets;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services.Courses;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;

namespace GraphQLDemo.API.Schema.Mutations;

public class Mutation
{
    private readonly CoursesRepository _coursesRepository;
    public Mutation(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    [Authorize]
    public async Task<CourseResult> CreateCourse(CourseTypeInput courseInput, [Service] ITopicEventSender topicEventSender,
        ClaimsPrincipal claimsPrincipal)
    {
        string userId =  claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
        string email =  claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL);
        string username =  claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME);
        string verified =  claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED);

        CourseDTO courseDTO = new CourseDTO()
        {
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId,
        };
        courseDTO  = await _coursesRepository.Create(courseDTO);

        CourseResult course = new CourseResult()
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
            InstructorId = courseDTO.InstructorId
        };
        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated),course);
        return course;
    }
    [Authorize]
    public async Task<CourseResult> UpdateCourse(Guid id, CourseTypeInput courseInput, [Service] ITopicEventSender topicEventSender)
    {
        CourseDTO courseDTO = new CourseDTO()
        {
            Id = id,
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId,
        };
        courseDTO = await _coursesRepository.Update(courseDTO);

        CourseResult course = new CourseResult()
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
            InstructorId = courseDTO.InstructorId
        };

        string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdate)}";
        await topicEventSender.SendAsync(updateCourseTopic, course);
        return course;
    }
    

    [Authorize(Policy = "IsAdmin")]
    public async Task<bool> DeleteCourse(Guid id)
    {
        try
        {
            return await _coursesRepository.Delete(id);

        }
        catch (Exception)
        {
            return false;
        }
    }



}
