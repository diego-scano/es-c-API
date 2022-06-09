using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EsercizioAC
{
    public static class Worker
    {
        public static void work()
        {
            string baseUrl = "https://jsonplaceholder.typicode.com/";

            HttpWebRequest webPostsReq = (HttpWebRequest)WebRequest.Create(baseUrl + "posts");
            HttpWebRequest webCommentsReq = (HttpWebRequest)WebRequest.Create(baseUrl + "comments");
            HttpWebRequest webTodosReq = (HttpWebRequest)WebRequest.Create(baseUrl + "todos");
            HttpWebRequest webUsersReq = (HttpWebRequest)WebRequest.Create(baseUrl + "users");

            webPostsReq.Method = "GET";
            webCommentsReq.Method = "GET";
            webTodosReq.Method = "GET";
            webUsersReq.Method = "GET";

            HttpWebResponse webPostsResp = (HttpWebResponse)webPostsReq.GetResponse();
            HttpWebResponse webCommentsResp = (HttpWebResponse)webCommentsReq.GetResponse();
            HttpWebResponse webTodosResp = (HttpWebResponse)webTodosReq.GetResponse();
            HttpWebResponse webUsersResp = (HttpWebResponse)webUsersReq.GetResponse();

            if (webPostsResp.StatusCode == HttpStatusCode.OK &&
                webCommentsResp.StatusCode == HttpStatusCode.OK &&
                webTodosResp.StatusCode == HttpStatusCode.OK &&
                webUsersResp.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("Request processed successfully\n");

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

                User userRandom = users[rand_num - 1];
                Console.WriteLine($"Id: {userRandom.id}\n" +
                    $"Name: {userRandom.name}\n" +
                    $"Username: {userRandom.username}\n" +
                    $"Email: {userRandom.email}");

                Console.WriteLine(new String('-', 50));

                Console.WriteLine($"\nTodos of {userRandom.name}:");
                var randomUserTodos = todos.Where(t => t.userId == rand_num);
                foreach (var t in randomUserTodos)
                {
                    Console.WriteLine($"{t.id}) - " + t.title + " - Completed: " + t.completed);
                }

                Console.WriteLine($"\nPosts of {userRandom.name}:");
                var randomUserPosts = posts.Where(p => p.userId == rand_num);
                foreach (var p in randomUserPosts)
                {
                    Console.WriteLine($"{p.id}) - " + p.title);
                }

                Console.WriteLine("\nInsert number of post for show its comments");
                int num = int.Parse(Console.ReadLine());
                var commentsList = comments.Where(c => c.postId == num);
                Console.WriteLine($"Comments of post number {num}:");
                foreach (var c in commentsList)
                {
                    Console.WriteLine($"{c.id}) - " + c.name);
                }
            }
            else
            {
                Console.WriteLine(HttpStatusCode.NotFound);
            }
        }
    }
}
