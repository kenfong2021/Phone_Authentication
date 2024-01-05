using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAuth.Services
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateMobile(string mobile);
        Task<(bool verified, string userID)> ValidateOTP(string code);
    }
}
