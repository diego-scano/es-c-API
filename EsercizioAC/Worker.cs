using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

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

                foreach (var user in users)
                {
                    user.posts = posts.Where(p => p.userId == user.id).ToList();
                    user.todos = todos.Where(t => t.userId == user.id).ToList();

                    Console.WriteLine($"Id: {user.id}\n" +
                        $"Name: {user.name}\n" +
                        $"Username: {user.username}\n" +
                        $"Email: {user.email}\n" +
                        $"Posts:");
                    foreach (var post in user.posts)
                    {
                        post.comments = comments.Where(c => c.postId == post.id).ToList();
                        Console.WriteLine($"\tId: {post.id}\n" +
                            $"\tTitle: {post.title}\n" +
                            $"\tComments:");
                        foreach (var comment in post.comments)
                        {
                            Console.WriteLine($"\t\tId: {comment.id}\n" +
                                $"\t\tName: {comment.name}");
                        }
                    }
                    Console.WriteLine("Todos:");
                    foreach (var todo in user.todos)
                    {
                        Console.WriteLine($"\tId: {todo.id}\n" +
                            $"\tTitle: {todo.title}");
                    }
                }
            }
            else
            {
                Console.WriteLine(HttpStatusCode.NotFound);
            }
        }
    }
}
