﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;

namespace AuthenticationService.Database
{
    /// <summary>
    /// DataRepository interface
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// To get user details
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UserModel> GetUserDetails(string userName);

        /// <summary>
        /// To save user data
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<string> SaveUserDetails(User user, string action);
    }
}
