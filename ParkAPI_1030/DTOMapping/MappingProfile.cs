using AutoMapper;
using ParkAPI_1030.Models;
using ParkAPI_1030.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI_1030.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalParkDto, NationalPark>().ReverseMap();
            CreateMap<TrailDto, Trail>().ReverseMap();
        }
    }
    
}


//steps******

//db-model-repository-DTO-Controller         (for find and display) yeah controller ko user ke ps access krega
//controller-dto-rep-model-db                 (for save and delete)


//db----------model-dto
//dto---------model--db
