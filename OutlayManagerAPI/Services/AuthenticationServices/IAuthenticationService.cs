﻿using OutlayManagerAPI.Model.UserAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<AuthenticationCredentials> Authenticate(UserCredential userCredentials);
    }
}