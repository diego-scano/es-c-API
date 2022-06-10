using System.Collections.Generic;

namespace EsercizioAC
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public List<Post> posts { get; set; }
        public List<Todo> todos { get; set; }
    }
}
