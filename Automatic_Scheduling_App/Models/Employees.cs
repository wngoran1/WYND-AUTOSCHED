namespace Automatic_Scheduling_App.Models
{
    public class Employee
    {
        public int EmployeeID { get; }
        public string Department { get; }

        public Employee(int employeeID, string department)
        {
            EmployeeID = employeeID;
            Department = department;
        }
    }
    public class EmployeeList
    {
        public List<Employee> employees;

        public EmployeeList()
        {
            employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            // Check if the employee already exists
            if (!employees.Exists(emp => emp.EmployeeID == employee.EmployeeID))
            {
                employees.Add(employee);
            }
        }

        public void RemoveEmployee(int employeeID)
        {
            // Remove the employee with the specified ID
            employees.RemoveAll(emp => emp.EmployeeID == employeeID);
        }

        public void PrintEmployees()
        {
            Console.WriteLine("Employees:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.EmployeeID}, Department: {employee.Department}");
            }
        }
    }
}