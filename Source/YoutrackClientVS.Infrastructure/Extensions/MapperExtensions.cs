using AutoMapper;

namespace YouTrackClientVS.Infrastructure.Extensions
{
    public static class MapperExtensions
    {
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}
