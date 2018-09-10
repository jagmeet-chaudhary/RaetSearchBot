using SearchBot.Common;
using SearchBot.Connectors;
using SearchBot.Connectors.HRM;
using SearchBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            HrmApiConnector h = new HrmApiConnector(new RequestHelper(),new TokenProvider());
            var result = h.SearchEmployees(new Employee() { FirstName = "Michelle" });

        }
    }
}
