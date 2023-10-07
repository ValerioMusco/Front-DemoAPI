using DemoASPMVC_DAL.Exceptions;
using DemoASPMVC_DAL.Interface;
using DemoASPMVC_DAL.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DemoASPMVC_DAL.Services {
    public class GameDBService : BaseRepository<Game>, IGameService {

        protected override Game Mapper( Game g ) {
            return new Game {

            };
        }

        // Envois token pour AdminPolicy
        public void Create( Game game, string token ) {

            _client.DefaultRequestHeaders.Add( "Authorization", "Bearer " + token );
            string jsonToSend = JsonConvert.SerializeObject( new { game.Title, game.Description, GenreId = game.IdGenre} );
            HttpContent content = new StringContent( jsonToSend, Encoding.UTF8, "application/json" );

            using(HttpResponseMessage response = _client.PostAsync( "Game", content ).Result ) {

                if( response.IsSuccessStatusCode )
                    Console.WriteLine("Jeu ajouté avec succès");
                else
                    Console.WriteLine("Erreur lors de l'ajout du jeu");
            }
            
        }

        // Envois token pour IsConnected
        // List mes favoris
        public IEnumerable<Game> GetByUserId( int userId, string token ) {

            List<Game> list = new ();
            _client.DefaultRequestHeaders.Add( "Authorization", "Bearer " + token );
            using( HttpResponseMessage response = _client.GetAsync( "User/favorite/" + userId ).Result ) {

                if( response.IsSuccessStatusCode ) {

                    string json = response.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<Game>>( json );
                }
                else
                    Console.WriteLine( "Erreur récupération favoris" );
            }

            return list;
        }

        // Envois token pour IsConnected
        // Ajout favoris
        public void AddFavorite( int idUser, int idGame , string token ) {

            string jsonToSend = JsonConvert.SerializeObject( new {idUser, idGame} );
            HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, "application/json" );
            _client.DefaultRequestHeaders.Add( "Authorization", "Bearer " + token );

            using( HttpResponseMessage response = _client.PostAsync( "User/AddFavorite", content ).Result ) {

                if( response.IsSuccessStatusCode )
                    Console.WriteLine("Jeu ajouté au favoris.");
                else
                    Console.WriteLine("Erreur lors de l'ajout au favoris.");
            }
        }
    }
}
