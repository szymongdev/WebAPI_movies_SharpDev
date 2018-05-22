﻿using AutoMapper;
using MoviesAPI.DbModels;
using MoviesAPI.Models;

namespace MoviesAPI.Mapping
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<ReviewRequest, Review>();
            CreateMap<Review, ReviewResponse>();
        }
    }
}
