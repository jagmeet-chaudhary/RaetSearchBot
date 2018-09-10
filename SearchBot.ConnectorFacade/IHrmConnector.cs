
using SearchBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors
{
    public interface IHrmConnector
    {
        List<Employee> SearchEmployees(Employee employee);
        Employee GetManagerForEmployee(Employee employee);
    }
}
