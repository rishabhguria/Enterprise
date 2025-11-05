using AutoMapper;
using Newtonsoft.Json.Linq;
using Prana.APIAdapter.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prana.APIAdapter.Utilities
{
    class MappingProfile : Profile
    {

        /// <summary>
        /// Mapping Profile with from/to pairs
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public MappingProfile(string from, string to)
        {
            var map = CreateMap<JObject, AuthSession>()
           .ForMember(to, m => { m.MapFrom(s => s[from]); });

        }

        /// <summary>
        /// Mapping Profile with properties mapping dict
        /// </summary>
        /// <param name="columMapping"></param>
        public MappingProfile(Dictionary<string, string> columMapping)
        {
            var map = CreateMap<JObject, AuthSession>();

            foreach (KeyValuePair<string, string> item in columMapping)
            {
                List<string> nestedItems = item.Key.Split('^').ToList();
                if (nestedItems.Count == 1)
                    map.ForMember(item.Value, m => { m.MapFrom(s => s[item.Key]); });

                else if (nestedItems.Count == 2)
                    map.ForMember(item.Value, m => { m.MapFrom(s => s[nestedItems[0]][nestedItems[1]]); });
            }


        }

        /// <summary>
        /// Mapping Profile with properties mapping dict
        /// </summary>
        /// <param name="columMapping"></param>
        /// <param name="map"></param>
        public MappingProfile(Dictionary<string, string> columMapping, IMappingExpression<JObject, AuthSession> map)
        {
            foreach (KeyValuePair<string, string> item in columMapping)
            {
                List<string> nestedItems = item.Key.Split('^').ToList();
                if (nestedItems.Count == 1)
                    map.ForMember(item.Value, m => { m.MapFrom(s => s[item.Key]); });

                else if (nestedItems.Count == 2)
                    map.ForMember(item.Value, m => { m.MapFrom(s => s[nestedItems[0]][nestedItems[1]]); });
            }
        }

        //public void MappingProfile1 (Dictionary<string, string> columMapping, IMappingExpression<T, AuthSession> map)
        //{
        //    foreach (KeyValuePair<string, string> item in columMapping)
        //    {
        //        List<string> nestedItems = item.Key.Split('^').ToList();
        //        if (nestedItems.Count == 1)
        //            map.ForMember(item.Value, m => { m.MapFrom(s => s[item.Key]); });

        //        else if (nestedItems.Count == 2)
        //            map.ForMember(item.Value, m => { m.MapFrom(s => s[nestedItems[0]][nestedItems[1]]); });
        //    }
        //}
    }
}
