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
            return Type == null ? null : AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(Type)).FirstOrDefault(responseType => responseType != null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Type? GetResponseType()
        {
            return ResponseType == null ? null : AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(ResponseType)).FirstOrDefault(responseType => responseType != null);
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