using System;
using System.Collections.Generic;
using System.Text;

namespace TripLog.Services
{
    public interface IAuthService
    {
        void SignInAsync(string clientId, Uri authUrl, Uri callbackUrl,
            Action<string> tokenCallback, Action<string> errorCallback);
    }
}
