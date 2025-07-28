using System.Xml.Serialization;

namespace ProjectTask.Domain.Entities
{
    public class Employees
    {   
        public int Id { get; set; }
        public int EmpId { get; set; }
        
        public string Name { get; set; }

        public int Age { get; set; }

        public string Department { get; set; }

        public string Country { get; set; }

    }
    [XmlType(Namespace = "http://schemas.datacontract.org/2004/07/")]
    [XmlRoot("EmployeeModel", Namespace = "http://schemas.datacontract.org/2004/07/")]
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; }
        public string Nationality { get; set; }
        public string PassportNo { get; set; }
        public DateTime PassportExpiryDate { get; set; }
        public string PassportIssuingCountry { get; set; }
        public string VisaNo { get; set; }
        public DateTime VisaExpiryDate { get; set; }
        public bool IsActive { get; set; }
        [XmlElement(IsNullable = true)]
        public string PasswordHash { get; set; }

        [XmlElement(IsNullable = true)]
        public string PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
    }
}
