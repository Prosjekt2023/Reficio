using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace bacit_dotnet.MVC.Entities
{
    [Table("Employee")]
    public class EmployeeEntity
    {
        public int emp_id { get; set; }
        public int role_id { get; set; }
        public int authorization_role_id { get; set; }
        public string name { get; set; }
        public string authorizationRole { get; set; }
        
        [JsonIgnore]
        public string passwordhash { get; set; }
        public RoleEntity role { get; set; }
        [JsonIgnore]
        public byte[] salt { get; set; }

        public List<TeamEntity> teams { get; set; }
        public List<SuggestionEntity> suggestions { get; set; }
        public List<RoleEntity> roles { get; set; }
    }
}