using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrgStructure.Models;
using WebAPICore.Data;
using WebAPICore.Models;

namespace WebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserDetailsController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public UserDetailsController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: api/UserDetails
        [HttpGet]
        public IEnumerable<UserDetails> GetUserdetails()
        {
            return _context.Userdetails;
        }

        //[HttpGet]
        //public IEnumerable<UserDetails> GetUserdetails()
        //{
        //    return _context.Userdetails.FromSql<UserDetails>("spAddNewUser").ToList();
        //}

        // GET: api/UserDetails/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUserDetails([FromRoute] long id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var userDetails = await _context.Userdetails.FindAsync(id);

        //    if (userDetails == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(userDetails);
        //}

        //[HttpGet("{EmailID}")]
        //public UserDetails GetUserDetails([FromRoute] string EmailID)
        //{
        //    var userDetail = _context.Userdetails.FromSql<UserDetails>("spCheckUserDetailsById {0}", EmailID).ToList().FirstOrDefault();
        //    //if(userDetail.EmailID == EmailID)
        //    //{
        //    //    userDetail.EmailID = "true";
        //    //}
        //    return userDetail;
        //}

        [HttpGet("{EmailID}")]
        public CurrentUserDetails GetUserDetails([FromRoute] string EmailID)
        {
            var userDetail = _context.CurrentUsers.FromSql<CurrentUserDetails>("spCheckUserDetailsById {0}", EmailID).ToList().FirstOrDefault();
            //if(userDetail.EmailID == EmailID)
            //{
            //    userDetail.EmailID = "true";
            //}
            return userDetail;
        }

        // PUT: api/UserDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetails([FromRoute] long id, [FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDetails.UserID)
            {
                return BadRequest();
            }

            _context.Entry(userDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserDetails
        [HttpPost]
        public async  Task<IActionResult> PostUserDetails([FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Userdetails.Add(userDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDetails", new { id = userDetails.UserID }, userDetails);
        }

        // DELETE: api/UserDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetails([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDetails = await _context.Userdetails.FindAsync(id);
            if (userDetails == null)
            {
                return NotFound();
            }

            _context.Userdetails.Remove(userDetails);
            await _context.SaveChangesAsync();

            return Ok(userDetails);
        }

        private bool UserDetailsExists(long id)
        {
            return _context.Userdetails.Any(e => e.UserID == id);
        }

        [HttpGet("/EmployeeOrgn/{id}")]
        public async Task<IActionResult> GetEmployeeOrg([FromRoute] long id)
        {
            List<CurrentUserDetails> emps = new List<CurrentUserDetails>
            {
                          new CurrentUserDetails  {UserID= 12, EmailID= "Vaibhav113@gmail.com" }

            };
            return Ok(emps);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Get(string EmailID, string password)
        {

            UserDetailsWithSecureToken objSecureToken = new UserDetailsWithSecureToken();
            var userDetail = _context.CurrentUsers.FromSql<CurrentUserDetails>("spCheckUserDetailsById {0}", EmailID).ToList().FirstOrDefault();
            //if(userDetail.EmailID == EmailID)
            //{
            //    userDetail.EmailID = "true";
            //}
            //just hard code here.  
            string EncodePass = EncodePassword(password);
            string DecodedPass = DecodePassword(EncodePass);
            if (userDetail.EmailID == EmailID && userDetail.ProfilePassword == DecodedPass)
                //if (userDetail.EmailID == EmailID && userDetail.ProfilePassword == password)
            {
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, EmailID),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is the secret key to add some default jwt token, lets see how it works"));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = "EmpPortalIssuer",
                    ValidateAudience = true,
                    ValidAudience = "EmpPortalAudience",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };

                var jwt = new JwtSecurityToken(
                    issuer: "EmpPortalIssuer",
                    audience: "EmpPortalAudience",
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(20)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt
                    //expires_in = (int)TimeSpan.FromMinutes(8000).TotalSeconds
                };

                objSecureToken.EmailID = userDetail.EmailID;
                objSecureToken.EmployeeID = userDetail.EmployeeID;
                objSecureToken.EmployeeName = userDetail.EmployeeName;
                objSecureToken.UserID = userDetail.UserID;
                objSecureToken.UserRole = userDetail.UserRole;
                objSecureToken.ProfilePassword = userDetail.ProfilePassword;
                objSecureToken.SecureToken = encodedJwt.ToString();

                return Ok(objSecureToken);
            }
            else
            {
                return Ok("");
            }
        }

        [HttpPost]
        [Route("Encode")]
        public static string EncodePassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        [HttpPost]
        [Route("Decode")]
        public string DecodePassword(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

    }
}