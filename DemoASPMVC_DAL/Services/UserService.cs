using DemoASPMVC_DAL.Exceptions;
using DemoASPMVC_DAL.Interface;
using DemoASPMVC_DAL.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoASPMVC_DAL.Services {
    public class UserService : BaseRepository<User>, IUserService {

        protected override User Mapper( User user ) {

            return new User {

            };
        }

        public UserLogin Login( string email, string pwd ) {

            string jsonToSend = JsonConvert.SerializeObject( new {email, password = pwd});
            HttpContent content = new StringContent( jsonToSend, Encoding.UTF8, "application/json" );

            using(HttpResponseMessage response = _client.PostAsync( "User/login", content).Result ) {

                if(response.IsSuccessStatusCode) {

                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    UserLogin jsonObject = JsonConvert.DeserializeObject<UserLogin>(jsonResponse );

                    return jsonObject;
                }
                else {

                    throw new UserNotFoundException( "Erreur lors de la connection." );
                }
            }
        }

        public bool Register( string email, string pwd, string nickname ) {

            string jsonToSend = JsonConvert.SerializeObject( new { username = nickname, email, password = pwd, confirmPassword = pwd });
            HttpContent content = new StringContent (jsonToSend, Encoding.UTF8, "application/json" );

            using(HttpResponseMessage response = _client.PostAsync( "User/Register", content).Result ) {

                if(response.IsSuccessStatusCode )
                    return true;
                else
                    return false;
            }
        }
    }
}
