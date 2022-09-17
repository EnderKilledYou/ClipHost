
using BlazorQueue;
using System.Text.Json;

public static class MetaObjectHelper
{

    public static MetaPacket ToMetaPacket<T,R>(this object any)
    {
        return new MetaPacket() { ResponseType = typeof(T).FullName, Type = typeof(R).FullName, Data = JsonSerializer.SerializeToElement(any) };
    }
}