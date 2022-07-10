using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;

namespace AuthenticationService.Database
{
    /// <summary>
    /// DataRepository class
    /// </summary>
    public class DataRepository : IDataRepository
    {
        /// <summary>
        /// _databaseContext
        /// </summary>
        private DatabaseContext _databaseContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseContext"></param>
        public DataRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        ///<inheritdoc/>
        public async Task<UserModel> GetUserDetails(string userName)
        {
            var userDetails = await (from u in _databaseContext.Users.Where(x => x.UserName == userName)
                                     join ur in _databaseContext.UserRoles on u.UserId equals ur.UserId
                                     join r in _databaseContext.Roles on ur.RoleId equals r.RoleId
                                     //select u).FirstOrDefaultAsync();
                                     select new UserModel
                                     {
                                         UserId = u.UserId,
                                         UserName = u.UserName,
                                         PasswordHash = u.PasswordHash,
                                         PasswordSalt = u.PasswordSalt,
                                         Role = r.RoleName,
                                         TokenCreated = u.TokenCreated,
                                         TokenExpires = u.TokenExpires,
                                         RefreshToken = u.RefreshToken,
                                         CreatedBy = u.CreatedBy,
                                         CreatedDateTime = u.CreatedDateTime,
                                         LastChangedBy = u.LastChangedBy,
                                         LastChangedDateTime = u.LastChangedDateTime
                                     }).FirstOrDefaultAsync();

            return userDetails;
        }

        ///<inheritdoc/>
        public async Task<string> SaveUserDetails(User user, string action)
        {
            _databaseContext.Entry(user).State = (user.UserId == 0) ? EntityState.Added : EntityState.Modified;
            var noOfRowsChanged = await _databaseContext.SaveChangesAsync();
            if (noOfRowsChanged <= 0)
                return "Error occured while saving data in User table";

            if (action == "register")
            {
                var userRole = new UserRole()
                {
                    UserId = user.UserId,
                    RoleId = 2,
                    CreatedBy = user.UserName,
                    CreatedDateTime = DateTime.Now,
                    LastChangedBy = user.UserName,
                    LastChangedDateTime = DateTime.Now
                };
                await _databaseContext.UserRoles.AddAsync(userRole);
                var noOfRecordsChanged = await _databaseContext.SaveChangesAsync();
                if (noOfRecordsChanged <= 0)
                    return "Error occured while saving data in UserRole table";
            }

            return "Data saved successfully";
        }
    }
}
