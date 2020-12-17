# DevAssessementWebsite

This is a project I built as way to assess my abilities in C# and Web apps using ASP.Net Core.

The login page looks like this:
![image](https://user-images.githubusercontent.com/20579513/102452744-8c8d7780-4043-11eb-9273-1857530eb699.png)

The User Details Page looks like this :
![image](https://user-images.githubusercontent.com/20579513/102452827-b3e44480-4043-11eb-9d96-b24e2fee726a.png)
Note that the user account being edited, won't save changes if the user logged in user isn't == user being edited.
Anyone can read the user information.


The prompt I was provided with was very simple, if I were to give recommendations on this prompt, I would suggest Making password hashing a requirement as based on the prompt I was given password is stored in plaintext, which isn't best practice in the industry.
Furthermore this could have been completed in under 3 hours using ASP.Net Core Identity with razor pages, but would include extra information in the Users Table, that I felt would be overkill for this assessment.

You can have a look at https://github.com/PieterVanZyl-Dev/ASPNETCore-WebAPI/blob/master/Controllers/UsersController.cs
and the below provided code for how you would implement that for a web api in asp.net core.

```
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {



                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

```
