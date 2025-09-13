 
using Microsoft.Data.SqlClient;


namespace CT.Auth
{
    public class AuthDL
    {

        DLUtil util = new DLUtil();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password_hash"></param>
        /// <returns></returns>
        public List<auth_accounts> GetLoginInfoByUsernameHash(string username, string password_hash)
        {


            var sql = @"
select *
from [auth].auth_accounts a
where a.Username=@username 
and a.password_hash = @password_hash

";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password_hash", password_hash);

            return util.SelectList<auth_accounts>(cmd);



        }


         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param> 
        /// <returns></returns>
        public auth_accounts GetLoginInfoByUsername(string username)
        {


            var sql = @"
select top 1 *
from [auth].auth_accounts a
where a.Username=@username  

";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@username", username); 

            return util.Select<auth_accounts>(cmd);



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="encryptedPassword"></param>
        /// <returns></returns>
        public void Signin(auth_accounts loginInfo)
        {


            var sql = @"
INSERT INTO [auth].[auth_accounts]
           ( [Username]
           ,[password_hash]  
)
     VALUES
           (
		    @Username 
           ,@password_hash  
		   )
";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@username", loginInfo.username);
            cmd.Parameters.AddWithValue("@password_hash", loginInfo.password_hash); 

            util.ExcuteSqlCommand(cmd, sql);

        } 

    }
}
