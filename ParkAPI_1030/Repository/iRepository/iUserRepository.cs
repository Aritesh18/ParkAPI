using ParkAPI_1030.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI_1030.Repository.iRepository
{
  public  interface iUserRepository
    {
        bool IsUniqueUser(string userName);
        User Authenticate(string userName, string password);
        User Register(string userName, string password);
    }
}
