namespace CustomerAccount.API.Services
{
	public static class Hashing
	{
		public static bool VerifyPasswordHash(string password, string storedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, storedPassword);
		}

		public static string HashPassword(string input)
		{
			return BCrypt.Net.BCrypt.HashPassword(input);
		}
	}
}