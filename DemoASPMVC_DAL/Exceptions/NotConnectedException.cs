using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoASPMVC_DAL.Exceptions {
    public class NotConnectedException : Exception{

        public NotConnectedException() { }

        public NotConnectedException(string message) : base(message) { }
    }
}
