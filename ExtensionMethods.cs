using System;
using System.Security.Claims;

namespace MvcCore 
{
    public static class ExtensionMethods
    {
		public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

		public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static User Info(this ClaimsPrincipal user, Db db = null)
        {
			var id = 0;
            if (db != null && int.TryParse(user?.Identity?.Name, out id))
                return db.Users.Get(id);
            return new User { Id = 0, Name = "Anonymous" };
        }

        public static string Encrypt(this string s)
        {
            var validationKey = "518A9D0E650ACE4CB22A35DA4563315098A96D0BB8E357531C7065D032099214A11D1CA074B6D66FF0836B35CEAAD0E7EEEFAED772754832E0A5F94EF8522222";
            //var decryptionKey = "DB5660C109E9EC70F044BA1FED99DE0C5922321C5125E84C23A1B5CA0E426909";
            var validationKeyBytes = new byte[validationKey.Length / 2];
            for (int i = 0; i < validationKeyBytes.Length; i++)
                validationKeyBytes[i] = Convert.ToByte(validationKey.Substring(i * 2, 2), 16);

            var hash = new System.Security.Cryptography.HMACSHA1(validationKeyBytes);
            var encodedPassword = Convert.ToBase64String(hash.ComputeHash(System.Text.Encoding.Unicode.GetBytes(s)));

            return encodedPassword;
        }
    }
}