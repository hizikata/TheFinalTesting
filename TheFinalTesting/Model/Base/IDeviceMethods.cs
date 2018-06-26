using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFinalTesting.Model
{
    interface IDeviceCommonMethods
    {
        void Reset();
        string GetIdn();
        void Initialize();
        string ReadCommand();
        bool WriteCommand(string command);
       
    }
}
