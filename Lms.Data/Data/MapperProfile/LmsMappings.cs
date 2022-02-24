﻿using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data.MapperProfile
{
    public class LmsMappings : Profile
    {
        public LmsMappings()
        {
            CreateMap<Course, CourseDto>()
                .ForMember(
                dest => dest.EndDate,
                opt => opt.MapFrom(src => src.StartDate.AddMonths(3)))
                
                .ForMember(
                dest=>dest.Modules,
                opt => opt.MapFrom(src =>src.Modules));
            CreateMap<Module, ModuleDto>()
                .ForMember(
                dest => dest.EndDate,
                opt => opt.MapFrom(src => src.StartDate.AddMonths(1)));

        }
    }
}