﻿using AutoMapper;

namespace YouTrack.REST.API.Mappings
{
    public static class MapperExtensions
    {
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}
