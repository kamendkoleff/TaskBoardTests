using RestSharp;
using System.Net;
using System.Text.Json;

namespace API_Tests_TaskBoard
{
    public class API_Tests_TaskBoard
    {
        private const string Url = "https://taskboard.nakov.repl.co/api/tasks";
        private RestClient client;
        private RestRequest request;
        [SetUp]

        public void Setup()
        {
            client = new RestClient();   
        }

        [Test]
        public void Test_ListTasks()
        {

            this.request = new RestRequest(Url);
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskResponse>>(response.Content);

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].board?.name, Is.EqualTo("Done"));
            Assert.That(tasks[0].title, Is.EqualTo("Project skeleton"));

        }

        [Test]
        public void Test_FindTask_ByValidInput()
        {
            this.request = new RestRequest(Url + "/search/home");
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskResponse>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].title, Is.EqualTo("Home page"));


        }
        [Test]
        public void Test_FindTask_ByInvalidInput()
        {
            this.request = new RestRequest(Url + "/search/missing");
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskResponse>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.EqualTo(0));
        }
        [Test]
        public void Test_CreateTask_InvalidInput()
        {
            this.request = new RestRequest(Url);
            var body = new
            {
                //title = "My New Task",
                description = "My New Task Rocks!!!",
                board = "Open"
            };
            request.AddJsonBody(body);
            RestResponse? response = this.client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));

        }
        [Test]
        public void Test_CreateTask_ValidInput()
        {
            this.request = new RestRequest(Url);
            var body = new
            {
                title = "QA Superstar",
                description = "Improve Your Code!!!",
                board = "Open"
            };
            request.AddJsonBody(body);
            RestResponse? response = this.client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
 
            RestResponse? alltasks = this.client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<TaskResponse>>(alltasks.Content);
            var lastTask = tasks[tasks.Count - 1];

            Assert.That(lastTask.title, Is.EqualTo("QA Superstar"));
            Assert.That(lastTask.description, Is.EqualTo("Improve Your Code!!!"));

        }

    }
}


