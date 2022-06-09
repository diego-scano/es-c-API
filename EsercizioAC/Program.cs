using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace EsercizioAC
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = "https://jsonplaceholder.typicode.com/";
            string postsEP = "posts";
            string commentsEP = "comments";
            string todosEP = "todos";
            string usersEP = "users";

            HttpWebRequest webPostsReq = (HttpWebRequest)WebRequest.Create(baseUrl + postsEP);
            HttpWebRequest webCommentsReq = (HttpWebRequest)WebRequest.Create(baseUrl + commentsEP);
            HttpWebRequest webTodosReq = (HttpWebRequest)WebRequest.Create(baseUrl + todosEP);
            HttpWebRequest webUsersReq = (HttpWebRequest)WebRequest.Create(baseUrl + usersEP);

            webPostsReq.Method = "GET";
            webCommentsReq.Method = "GET";
            webTodosReq.Method = "GET";
            webUsersReq.Method = "GET";

            HttpWebResponse webPostsResp = (HttpWebResponse)webPostsReq.GetResponse();
            HttpWebResponse webCommentsResp = (HttpWebResponse)webCommentsReq.GetResponse();
            HttpWebResponse webTodosResp = (HttpWebResponse)webTodosReq.GetResponse();
            HttpWebResponse webUsersResp = (HttpWebResponse)webUsersReq.GetResponse();

            if(webPostsResp.StatusCode == HttpStatusCode.OK && 
                webCommentsResp.StatusCode == HttpStatusCode.OK && 
                webTodosResp.StatusCode == HttpStatusCode.OK && 
                webUsersResp.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("Richiesta elaborata correttamente\n");

                static string Streamer(HttpWebResponse resp)
                {
                    Stream stream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }

                List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(Streamer(webPostsResp));
                List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(Streamer(webCommentsResp));
                List<Todo> todos = JsonConvert.DeserializeObject<List<Todo>>(Streamer(webTodosResp));
                List<User> users = JsonConvert.DeserializeObject<List<User>>(Streamer(webUsersResp));

                Console.WriteLine("Posts: " + posts.Count);
                Console.WriteLine("Comments: " + comments.Count);
                Console.WriteLine("Todos: " + todos.Count);
                Console.WriteLine("Users: " + users.Count);

                Random rd = new Random();
                int rand_num = rd.Next(1, users.Count);

                Console.WriteLine(new String('-', 50));

                Console.WriteLine("Random user -> ");

                foreach (var user in users)
                {
                    if (user.id == rand_num)
                    {
                        Console.WriteLine($"ID: {user.id}\nName: {user.name}\nUsername: {user.username}\nEmail: {user.email}");
                    }
                }
            } else
            {
                Console.WriteLine(HttpStatusCode.NotFound);
            }
        }
    }
}
