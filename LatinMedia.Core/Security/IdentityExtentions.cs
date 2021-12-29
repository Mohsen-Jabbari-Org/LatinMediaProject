using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace LatinMedia.Core.Security
{
   public static class IdentityExtentions
    {
        public static string GetID(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value ?? string.Empty;
        }

        public static string GetZero(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Locality);
            return claim?.Value ?? string.Empty;
        }
        public static string GetEmail(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Email);
            return claim?.Value ?? string.Empty;
        }

        public static string GetMobile(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.MobilePhone);
            return claim?.Value ?? string.Empty;
        }

        public static string GetAvatar(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
            return claim?.Value ?? string.Empty;
        }

        public static string GetGender(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Gender);
            return claim?.Value ?? string.Empty;
        }

        public static string GetAcademyId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Actor);
            return claim?.Value ?? string.Empty;
        }
    }
}
