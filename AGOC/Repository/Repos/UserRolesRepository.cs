﻿using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Repository.Repos;

namespace AGOC.Repository
{
    public class UserRolesRepository : RepositoryBase<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}