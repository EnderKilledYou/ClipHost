using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListStreamerModels
{
    public class DeleteStreamerResponse
    {


        public string Message { get; set; } = "";



        public bool Success { get; set; } = true;



        public Streamer DeletedStreamer { get; set; }


    }
}
