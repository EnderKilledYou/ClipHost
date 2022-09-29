using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClipHost.ServiceModel.Types
{
    public interface ITableUp
    {
        public void TableUp(Action<Type[]> createTableIfNotExists);
    }
    public abstract class TablesUp : ITableUp
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public void TableUp(Action<Type[]> createTableIfNotExists)
        {
            createTableIfNotExists(new Type[] { GetType() });
        }
    }

    [TableUp(1)]
    public class CommandCenter : TablesUp
    {

        public string Name { get; set; } = "";
        public int StreamerCount { get; set; } = 0;
        public int MaxStreamers { get; set; } = 0;



    }
}
