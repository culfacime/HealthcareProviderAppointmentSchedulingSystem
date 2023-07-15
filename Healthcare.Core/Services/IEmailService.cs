using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.Services
{
    public interface IEmailService
    {
        bool Send(string email);
    }
}
