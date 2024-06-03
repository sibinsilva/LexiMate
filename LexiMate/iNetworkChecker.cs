using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiMate
{
    public interface iNetworkChecker
    {
        bool IsConnected { get; }
        void CheckInternetConnection();
    }
}
