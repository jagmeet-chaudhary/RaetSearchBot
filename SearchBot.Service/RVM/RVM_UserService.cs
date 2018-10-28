using SearchBot.Connectors;
using SearchBot.Connectors.RVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Service.RVM
{
    public class RVM_UserService : IRVM_UserService
    {

        private IRVMConnector _iRVMConnector;
        public RVM_UserService(IRVMConnector iRVMConnector)
        {
            _iRVMConnector = iRVMConnector;
        }

        public bool Resetpassword(string name, string newpassword)
        {
            return _iRVMConnector.ResetPassword(name, newpassword);
        }


    }

    public interface IRVM_UserService
    {
        bool Resetpassword(string name, string newpassword);

       
    }
}
