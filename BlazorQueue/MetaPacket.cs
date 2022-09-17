using System;
using System.Text.Json;
using System.Xml.Linq;

namespace BlazorQueue
{
    public record MetaPacket
    {
        //todo: Some caching of types to speed this up
        public Type? GetRequestType()
        {
            if (Type == null) return null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type? responseType = assembly.GetType(Type);
                if (responseType != null) return responseType;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Type? GetResponseType()
        {
            if (ResponseType == null) return null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type? responseType = assembly.GetType(ResponseType);
                if (responseType != null) return responseType;
            }
            return null;
        }
    
        /// <summary>
        /// 
        /// </summary>
        public string? ResponseType { get; init; }
        /// <summary>
        /// 
        /// </summary>
        public string? Type { get; init; }
        /// <summary>
        /// 
        /// </summary>
        public JsonElement Data { get; init; }
    }
}