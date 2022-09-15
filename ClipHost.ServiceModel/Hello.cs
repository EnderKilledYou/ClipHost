using ServiceStack;

namespace ClipHost.ServiceModel
{
    [Route("/hello")]
    [Route("/hello/{Name}")]
    public class Hello : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }
    [Route("/test")]
    [Route("/test/{Name}")]
    public class HelloTest : IReturn<HelloTestResponse>
    {
        public string Name { get; set; }
    }

    public class HelloTestResponse
    {
        public string Result { get; set; }
    }
}
