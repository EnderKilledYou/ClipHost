using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(1)]
    public class Streamer : TablesUp
    {


        [SearchField]
        [Required]
        public string Name { get; set; } = "";
        [SearchField]
        public bool Enabled { get; set; } = true;
    }
}
