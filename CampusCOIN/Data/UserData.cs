using CampusCOIN.Models;
using SQLite;
using System.Collections.Generic;


namespace CampusCOIN.Data
{

    public class UserData
    {
        private readonly SQLiteAsyncConnection connection;
        List<UserModel> users;

        public UserData()
        {
            if (connection == null)
            {
                connection = new SQLiteAsyncConnection(Constants.DatabasePath);
            }
            //create new usermodel table
            connection.CreateTableAsync<UserModel>();
            users = new List<UserModel>();
        }

        //add new user
        public async Task<string> SaveUser(UserModel user)
        {
            try
            {

                await connection.InsertAsync(user);
                return user.UserId;


            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                await Shell.Current.DisplayAlert("User Registration Failed", ex.Message, "Ok");
            }
            return user.UserId;
        }

        //delete user
        public async Task DeleteUser(UserModel user)
        {
            //await Init();
            try
            {
                await connection.DeleteAsync(user);
                await Shell.Current.DisplayAlert("Deletion Successful!", "Your data was successfully deleted.", "Ok");

            }

            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Deletion Unsuccessful!", ex.Message, "Ok");

            }

        }

        //get user by email
        public async Task<List<UserModel>> GetUserAsync(string email)
        {
            //await Init();
            try
            {
                return await connection.Table<UserModel>()
                .Where(u => u.Email == email).ToListAsync();
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("No User with the given Email", ex.Message, "Ok");

            }

            return new List<UserModel>();

        }

        //get user by id
        public async Task<List<UserModel>> GetUserById(string id)
        {
            List<UserModel> user = new List<UserModel>();
            try
            {
                user = await connection.Table<UserModel>().Where(u => u.UserId == id).ToListAsync();
                
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("No User with the given Id", ex.Message, "Ok");

            }

            return user;

        }

        //get user by email
        public async Task<string> GetUserByEmail(string email)
        {
            UserModel user = new UserModel();
            try
            {
                users = await connection.Table<UserModel>().Where(u => u.Email == email).ToListAsync();
                if (users.Count > 0)
                {
                    user = users.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("No User with the given Email", ex.Message, "Ok");

            }

            return user.UserId;

        }

        //update user
        public async Task UpdateUser(UserModel user)
        {
            try
            {
                await connection.UpdateAsync(user);
                await Shell.Current.DisplayAlert("Update Successful!", "Your data was successfully updated.", "Ok");

            }

            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Update Unsuccessful!", ex.Message, "Ok");

            }
        }
    }


}

