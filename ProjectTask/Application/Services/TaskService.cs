using Azure.Core;
using EmployeeService;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using ProjectTask.Infrastructure.Data;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectTask.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _db;

        public TaskService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProjectTasks>> GetTasks(TaskRequest request)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId && p.UserId == request.UserId)
                ?? throw new Exception("Project not found");

            return await _db.ProjectTasks.Where(t => t.ProjectId == request.ProjectId).ToListAsync();
        }

        public async Task<ProjectTasks> CreateTask(TaskRequest request)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId && p.UserId == request.UserId)
                ?? throw new Exception("Project not found");

            var task = new ProjectTasks
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                IsCompleted = request.IsCompleted,
                ProjectId = request.ProjectId
            };
            _db.ProjectTasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task<ProjectTasks> UpdateTask(TaskRequest request)
        {
            var task = await _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == request.TaskId && t.Project.UserId == request.UserId)
                ?? throw new Exception("Task not found");

            task.Title = request.Title;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.IsCompleted = request.IsCompleted;

            await _db.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTask(TaskRequest request)
        {
            var task = await _db.ProjectTasks
                .FirstOrDefaultAsync(t => t.Id == request.TaskId)
                ?? throw new Exception("Task not found");

            _db.ProjectTasks.Remove(task);
            await _db.SaveChangesAsync();

        }
        public async Task GetEmployeeInfo1()
        {
            var task = await _db.EmployeeId.ToListAsync();

            foreach (var item in task)
            {
                int EmpId = item.Id;
                var client = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService);
                var emp = await client.GetEmployeeInfoAsync(EmpId);
                var empl = new Employees
                {
                    EmpId = EmpId,
                    Name = emp.Name,
                    Age = emp.Age,
                    Department = emp.Department,
                    Country = emp.Country,

                };
                _db.Employees.Add(empl);
                await _db.SaveChangesAsync();
            }

        }
        public async Task<List<EmployeeModel>> GetEmployeeInfo()
        {
            string connectionString = "Server=ANUARYA\\SQLEXPRESS;Database=ProjectTask;User Id=anu;Password=sa1234;TrustServerCertificate=True;";

            List<int> employeeIds = new List<int>();
            // List<EmployeeModel> employeeList = new List<EmployeeModel>();
            string xmlResponse = string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT EmployeeId FROM Employee_Test";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        employeeIds.Add(reader.GetInt32(0));
                }
            }
            int batchSize = 50;
            var employeeList = new List<EmployeeModel>();

            var allBatches = employeeIds
                .Select((id, index) => new { id, index })
                .GroupBy(x => x.index / batchSize)
                .Select(g => g.Select(x => x.id).ToList())
                .ToList();

            foreach (var batch in allBatches)
            {
                var tasks = batch.Select(id => FetchEmployeeAsync(id)).ToList();
                var results = await Task.WhenAll(tasks);

                employeeList.AddRange(results.Where(x => x != null));
            }
           
            BulkInsertEmployees(employeeList, connectionString);
            return employeeList;

        }
        private async Task<EmployeeModel> FetchEmployeeAsync(int EmpId)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:13476/Service.svc");
                request.Headers.Add("SOAPAction", "\"http://tempuri.org/IService/GetEmployeeInfo\"");

                string soapBody = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <GetEmployeeInfo xmlns=""http://tempuri.org/"">
      <id>{EmpId}</id>
    </GetEmployeeInfo>
  </soap:Body>
</soap:Envelope>";

                request.Content = new StringContent(soapBody, Encoding.UTF8, "text/xml");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string xmlResponse = await response.Content.ReadAsStringAsync();

                return DeserializeSoapResponse(xmlResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employee {EmpId}: {ex.Message}");
                return null;
            }
        }

        public EmployeeModel DeserializeSoapResponse(string soapXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(soapXml);

            var nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            nsMgr.AddNamespace("ns", "http://tempuri.org/");

            var node = doc.SelectSingleNode("//soap:Body/ns:GetEmployeeInfoResponse/ns:GetEmployeeInfoResult", nsMgr);
            var innerXml = node.InnerXml;

            // If your WCF returns JSON inside XML, use:
            // return JsonConvert.DeserializeObject<List<EmployeeModel>>(innerXml);

            // If your WCF returns XML inside XML, use:
            innerXml = innerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Trim();

            var serializer = new XmlSerializer(typeof(EmployeeModel));
            using (var reader = new StringReader(innerXml))
            {
                var model = (EmployeeModel)serializer.Deserialize(reader);
                return model;
            }


        }

        private void BulkInsertEmployees(List<EmployeeModel> employees, string connectionString)
        {
            var dt = new DataTable();

            // DO NOT ADD EmployeeId if it's IDENTITY (auto-increment)
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("RoleId", typeof(int));
            dt.Columns.Add("DepartmentId", typeof(int));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("PhoneNo", typeof(string));
            dt.Columns.Add("DateOfBirth", typeof(DateTime));
            dt.Columns.Add("JoinDate", typeof(DateTime));
            dt.Columns.Add("Nationality", typeof(string));
            dt.Columns.Add("PassportNo", typeof(string));
            dt.Columns.Add("PassportExpiryDate", typeof(DateTime));
            dt.Columns.Add("PassportIssuingCountry", typeof(string));
            dt.Columns.Add("VisaNo", typeof(string));
            dt.Columns.Add("VisaExpiryDate", typeof(DateTime));
            dt.Columns.Add("IsActive", typeof(int));            
            dt.Columns.Add("CreatedAt", typeof(DateTime));
            dt.Columns.Add("CreatedBy", typeof(int));

            foreach (var emp in employees)
            {
                dt.Rows.Add(
                    emp.Name,
                    emp.RoleId,
                    emp.DepartmentId,
                    emp.Email,
                    emp.Gender,
                    emp.PhoneNo,
                    emp.DateOfBirth,
                    emp.JoinDate,
                    emp.Nationality,
                    emp.PassportNo,
                    emp.PassportExpiryDate,
                    emp.PassportIssuingCountry,
                    emp.VisaNo,
                    emp.VisaExpiryDate,
                    emp.IsActive ? 1 : 0,                   
                    DateTime.Now,
                    emp.CreatedBy
                );
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName = "dbo.Employee_Test1";

                    // Column mappings (required if column names differ or table has more columns)
                    bulk.ColumnMappings.Add("Name", "Name");
                    bulk.ColumnMappings.Add("RoleId", "RoleId");
                    bulk.ColumnMappings.Add("DepartmentId", "DepartmentId");
                    bulk.ColumnMappings.Add("Email", "Email");
                    bulk.ColumnMappings.Add("Gender", "Gender");
                    bulk.ColumnMappings.Add("PhoneNo", "PhoneNo");
                    bulk.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                    bulk.ColumnMappings.Add("JoinDate", "JoinDate");
                    bulk.ColumnMappings.Add("Nationality", "Nationality");
                    bulk.ColumnMappings.Add("PassportNo", "PassportNo");
                    bulk.ColumnMappings.Add("PassportExpiryDate", "PassportExpiryDate");
                    bulk.ColumnMappings.Add("PassportIssuingCountry", "PassportIssuingCountry");
                    bulk.ColumnMappings.Add("VisaNo", "VisaNo");
                    bulk.ColumnMappings.Add("VisaExpiryDate", "VisaExpiryDate");
                    bulk.ColumnMappings.Add("IsActive", "IsActive");                    
                    bulk.ColumnMappings.Add("CreatedAt", "CreatedAt");
                    bulk.ColumnMappings.Add("CreatedBy", "CreatedBy");

                    bulk.WriteToServer(dt);
                }
            }
        }


    }
}
