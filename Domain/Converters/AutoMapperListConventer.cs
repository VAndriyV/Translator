using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
namespace Domain.Converters
{
    public class AutoMapperListConventer
    {
        public static List<TDestination> MapList<TSource, TDestination>(List<TSource> source)
        {
            return source.Select(x => Mapper.Map<TDestination>(x)).ToList();
        }
    }
}
