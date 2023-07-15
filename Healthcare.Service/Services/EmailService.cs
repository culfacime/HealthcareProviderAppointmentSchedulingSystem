using Healthcare.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Service.Services
{
    public class EmailService : IEmailService
    {
        public bool Send(string email)
        {
            //will push the email a email queue (like rabbitmq)

            return true;
        }
    }
}
