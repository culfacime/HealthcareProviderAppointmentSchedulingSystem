using Healthcare.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Service.Services
{
    public class SmsService : ISmsService
    {
        public bool Send(string message)
        {
            //will push the message to a sms queue (like rabbitmq)

            return true;
        }
    }
}
