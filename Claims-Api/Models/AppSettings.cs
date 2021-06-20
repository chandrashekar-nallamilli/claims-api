
namespace Claims_Api.Models
{
    public class AppSettings
    {
        public string schema_name { get; set; }
        public string queue_connectionstring { get; set; }
        public string queue_name { get; set; }
        public string db_connectionstring { get; set; }

    }
}
