using KLGproject.Repositories.AppObjects;
using KLGproject.Repositories.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KLGproject.Resources.Libraries.DB.Lib
{
    public class SQLite
    {

        private string _connectionString;

        /// <summary>
        /// DataBase Class constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public SQLite(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes general non querys
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public void ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }

        }

        /// <summary>
        /// Executes general querys
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns><see cref="List{Int32}"/></returns>
        public List<Int32> ExecuteQuery(string sql, Dictionary<string, object> parameters = null)
        {
            var gamesList = new List<Int32>();

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        gamesList.Add(Convert.ToInt32(reader[0]));
                    }
                }

                connection.Close();
            }

            return gamesList;
        }

        /// <summary>
        /// Get a game from the DataBase if exists
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns><see cref="Game"/></returns>
        /// <exception cref="GameNotFoundException"></exception>
        public Game GetGameIfExists(int gameId)
        {
            Game game = null;

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand())
            {
                connection.Close();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM GAMESRATING WHERE GAMEID = ?";
                command.Parameters.Add(new SQLiteParameter("GAMEID", DbType.Int32) { Value = gameId });

                using (var reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {

                        game = new Game()
                        {
                            id = Int32.Parse(reader[0].ToString()),
                            Likes = Int32.Parse(reader[1].ToString()),
                            DisLikes = Int32.Parse(reader[2].ToString()),
                        };

                    }

                }

                connection.Close();
            }

            if (game == null)
            {
                throw new GameNotFoundException("Game not found in database");
            }

            return game;
        }

        /// <summary>
        /// GetThe user by its name
        /// </summary>
        /// <param name="username"></param>
        /// <returns><see cref="User"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public User GetUserByName(string username)
        {
            User user = null;

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand())
            {
                connection.Close();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM USER WHERE NAME = ?";
                command.Parameters.Add(new SQLiteParameter("USERNAME", DbType.String) { Value = username });

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User()
                        {
                            Id = Int32.Parse(reader[0].ToString()),
                            Name = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            Status = Int32.Parse(reader[3].ToString()),
                            Email = reader[4].ToString()
                        };

                    }
                }

                connection.Close();
            }

            if (user == null)
            {
                throw new UserNotFoundException("User not found in database");
            }

            user.Library = GetUserLibrary(user.Id);

            return user;
        }

        /// <summary>
        /// Sets the User Game
        /// </summary>
        /// <param name="_currentUserId"></param>
        /// <param name="_id"></param>
        /// <param name="_liked"></param>
        /// <param name="_disliked"></param>
        public void SetUserGame(int _currentUserId, int _id, bool _liked, bool _disliked)
        {

            if (CheckIfGameExistsForUser(_currentUserId, _id, "USERGAMES"))
            {

                UpdateUserGame(_currentUserId, _id, _liked, _disliked);

            }
            else
            {

                using (var connection = new SQLiteConnection(_connectionString))
                using (var command = new SQLiteCommand("INSERT INTO USERGAMES (USERID, GAMEID, LIKED, DISLIKED) VALUES (@UserId, @GameId, @Liked, @Disliked)", connection))
                {
                    command.Parameters.AddWithValue("@UserId", _currentUserId);
                    command.Parameters.AddWithValue("@GameId", _id);
                    command.Parameters.AddWithValue("@Liked", _liked);
                    command.Parameters.AddWithValue("@Disliked", _disliked);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                }

                //throw new UserGameNotFoundException("Game not found for this user in database");

            }

        }

        /// <summary>
        /// Checks if the game exits for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <param name="tableName"></param>
        /// <returns><see cref="List{Game}"/></returns>
        public bool CheckIfGameExistsForUser(int userId, int gameId, string tableName)
        {
            bool gameExists = false;

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand($"SELECT GAMEID FROM {tableName} WHERE USERID = @UserId AND GAMEID = @GameId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GameId", gameId);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        gameExists = true;
                    }
                }

                connection.Close();
            }

            return gameExists;
        }

        /// <summary>
        /// Updates the user game status
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <param name="liked"></param>
        /// <param name="disliked"></param>
        public void UpdateUserGame(int userId, int gameId, bool liked, bool disliked)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand("UPDATE USERGAMES SET LIKED = @Liked, DISLIKED = @Disliked WHERE USERID = @UserId AND GAMEID = @GameId", connection))
            {
                command.Parameters.AddWithValue("@Liked", liked ? 1 : 0);
                command.Parameters.AddWithValue("@Disliked", disliked ? 1 : 0);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GameId", gameId);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// Gets the user game status
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns><see cref="(bool,bool)"/></returns>
        public (bool, bool) GetUserGameStatus(int userId, int gameId)
        {
            bool liked = false;
            bool disliked = false;

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand("SELECT LIKED, DISLIKED FROM USERGAMES WHERE USERID = @UserId AND GAMEID = @GameId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GameId", gameId);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        liked = reader.GetBoolean(0);
                        disliked = reader.GetBoolean(1);
                    }
                }

                connection.Close();
            }

            return (liked, disliked);
        }

        /// <summary>
        /// Gets the ids of the games that are in the user Library
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="List{int}"/></returns>
        public List<int> GetUserLibrary(int userId)
        {
            List<int> library = new List<int>();

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand("SELECT GAMEID FROM USERLIBRARY WHERE USERID = @UserId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        library.Add(reader.GetInt32(0));
                    }
                }

                connection.Close();
            }

            return library;
        }

        /// <summary>
        /// Adds to the user library a new game<br/>
        /// <see cref="CheckIfGameExistsForUser(int, int, string)"/>
        /// </summary>
        /// <param name="_currentUserId"></param>
        /// <param name="_id"></param>
        public void AddToUserLibrary(int _currentUserId, int _id)
        {

            if (!CheckIfGameExistsForUser(_currentUserId, _id, "USERLIBRARY"))
            {

                using (var connection = new SQLiteConnection(_connectionString))
                using (var command = new SQLiteCommand("INSERT INTO USERLIBRARY (USERID, GAMEID) VALUES (@UserId, @GameId)", connection))
                {
                    command.Parameters.AddWithValue("@UserId", _currentUserId);
                    command.Parameters.AddWithValue("@GameId", _id);
                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                }

            }

        }

        /// <summary>
        /// Deletes the user game from the Library
        /// <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="id"></param>
        private void UpdateUserGame(int currentUserId, int id)
        {
            //para eliminar de la librería un juego ya añadido
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the more liked Games List, and get the 3 whith more likes
        /// </summary>
        /// <returns><see cref="List{Game}"/></returns>
        public List<int> GetMoreLikedGames()
        {

            List<int> moreLikedGames = new List<int>();

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand("SELECT GAMEID FROM GAMESRATING GROUP BY GAMEID ORDER BY LIKES DESC", connection))
            {

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        moreLikedGames.Add(reader.GetInt32(0));
                    }
                }

                connection.Close();
            }

            return moreLikedGames.Take(3).ToList();
        }

        /// <summary>
        /// Updates the user fields in the data base
        /// </summary>
        /// <param name="_currentUserId"></param>
        /// <param name="newUserName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <exception cref="UserGameNotFoundException"></exception>
        public void UpdateUser(int _currentUserId, string newUserName, string email, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                
                using (SQLiteCommand command = new SQLiteCommand("UPDATE USER SET NAME = @UserName, EMAIL = @Email, PWD = @Password WHERE ID = @UserId;", connection))
                {
                    connection.Close();
                    connection.Open();

                    command.Parameters.AddWithValue("@UserName", newUserName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@UserId", _currentUserId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // User was updated successfully
                    }
                    else
                    {
                        throw new UserGameNotFoundException("The user was not found.");
                    }
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Checks if the user already exits in the app<br/>
        /// </summary>
        /// <param name="username"></param>
        /// <returns><see cref="bool"/></returns>
        public bool UserExists(string username)
        {
            bool n = false;

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT COUNT(*) FROM USER WHERE NAME = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    n = ((long)command.ExecuteScalar()) > 0;
                }
                connection.Close();
            }

            return n;
        }
        
        /// <summary>
        /// Delete a user from the database
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUserFromDataBase(User user)
        {
            string sql = "DELETE FROM User WHERE Id = @Id;";

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", user.Id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// GetsTheUsers List from the data base
        /// </summary>
        /// <returns><see cref="List{User}"/></returns>
        public List<User> GetAllUsers()
        {
            List<User> usuarios = new List<User>();

            string sql = "SELECT * FROM User;";
            User user = null;

            using (var connection = new SQLiteConnection(_connectionString))
            using (var command = new SQLiteCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        user = new User()
                        {
                            Id = Int32.Parse(reader[0].ToString()),
                            Name = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            Email = reader[4].ToString()
                        };
                        usuarios.Add(user);
                    }
                }
            }

            return usuarios;
        }
    }
}