using AutoMapper;
using KanbanRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KanbanRestAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var rules = new EnableCorsAttribute("*", "*", "*");

            config.EnableCors(rules);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Set API to return JSON instead of XML
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            CreateMaps();
        }

        private static void CreateMaps()
        {
            Mapper.CreateMap<Card, CardsModel>();
            Mapper.CreateMap<List, ListsModel>();
        }
    }
}
