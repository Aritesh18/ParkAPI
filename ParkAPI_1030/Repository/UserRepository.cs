using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkAPI_1030.Data;
using ParkAPI_1030.Models;
using ParkAPI_1030.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkAPI_1030.Repository
{
    public class UserRepository : iUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        public UserRepository(ApplicationDbContext context,IOptions<AppSettings>appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public User Authenticate(string userName, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName==userName && u.Password == password);
            if (userInDb == null) return null;
            //JWT Authentication
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInDb.Id.ToString()),
                    new Claim(ClaimTypes.Role, userInDb.Role)
                }),
                Expires=DateTime.UtcNow.AddDays(7),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
   };
            var token = tokenHandler.CreateToken(tokenDescritor);
            userInDb.Token = tokenHandler.WriteToken(token);
              userInDb.Password = "";
            return userInDb;

        }

        public bool IsUniqueUser(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
                return true;
            else
                return false;
        }

        public User Register(string userName, string password)
        {
            User user = new User()
            {
                UserName = userName,
                Password = password,
                Role = "Admin"

            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
