using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XuxzLib.Communication
{
    public interface IDeviceCommonMethods
    {
        void Reset();
        string GetIdn();
        void Initialize();
        string ReadCommand();
        bool WriteCommand(string command);
       
    }
}
