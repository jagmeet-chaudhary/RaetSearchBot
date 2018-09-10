using Microsoft.Bot.Connector;
using SearchBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot
{
    public interface IQueryManagerConversationInterface
    {
        List<Attachment> GetEmployeeSearchList(List<Employee> employees);
        string GetNoEmployeesMessage();
        string GetManagerMessage(Employee employee);
    }
}
