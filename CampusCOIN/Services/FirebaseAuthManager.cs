using Firebase.Auth;
using Firebase.Auth.Providers;
using CampusCOIN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusCOIN.Services
{
    public static class FirebaseAuthManager
    {
        private static readonly FirebaseAuthClient authClient;

        public static UserModel currentUser { get; set; }

        static FirebaseAuthManager()
        {
            //Intialize the authetication client
            authClient = new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyA1YUJkDEWxSlk2Cjm91dqmpn8ZN7bAfDY",
                AuthDomain = "campuscoin-fb1f5.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            });
        }

        //Register account with firebase
        public async static Task<bool> RegisterAccount(UserModel user, string password)
        {
            try
            {
                var credentials = await authClient.CreateUserWithEmailAndPasswordAsync(user.Email, password);
                var id = credentials.User.Uid;
                user.UserId = id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Try to login with Firebase
        public async static Task<bool> Login(string email, string password)
        {
            try
            {
                var credentials = await authClient.SignInWithEmailAndPasswordAsync(email, password);
                var id = credentials.User.Uid;

                currentUser = new UserModel()
                {
                    UserId = id,
                    Email = email,
                };
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Try to reset password
        public async static Task<bool> ResetPassword(string email)
        {
            try
            {
                await authClient.ResetEmailPasswordAsync(email);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
