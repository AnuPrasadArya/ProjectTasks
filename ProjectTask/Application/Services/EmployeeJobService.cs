using System.Text.Json.Serialization;

namespace ProjectTask.Application.Services
{
    public class EmployeeJobService
    {
        public void FetchAndProcessEmployeeData()
        {
            // Simulate work
            Console.WriteLine($"[Job] Running at {DateTime.Now}");
            // TODO: Call external API and save to DB
        }
        public class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            [JsonIgnore]
            public int? InsertedId { get; set; } // Optional: Return EmployeeId or null
        }
    }
}
