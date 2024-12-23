﻿using AutoMapper;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Models.DTO;


namespace Learning_Web.API.Converters
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegionDTO, Region>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<WalkDTO, Walk>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<DifficultyDTO, Difficulty>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Region, RegionResponse>().ReverseMap();
            CreateMap<Walk, WalkResponse>().ReverseMap();
            CreateMap<Difficulty, DifficultyResponse>().ReverseMap();
            CreateMap<ImageDTO, Image>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName)))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.File.Length));

        }
    }
}
