using System.Collections.Generic;

namespace EsercizioAC
{
    public class Post
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public List<Comment> comments { get; set; }
    }
}
