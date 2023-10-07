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

namespace DemoASPMVC_DAL.Services
{
    public class GenreService : BaseRepository<Genre>, IGenreService {

        protected override Genre Mapper(Genre genre) {

            return new Genre {
                
            };
        }

        // Envois token pour AdminPolicy
        public void Add(string genre, string token) {

            string jsonToSend = JsonConvert.SerializeObject( new { label = genre } );
            HttpContent content = new StringContent( jsonToSend, Encoding.UTF8, "application/json" );
            _client.DefaultRequestHeaders.Add( "Authorization", "Bearer " + token );

            using (HttpResponseMessage response = _client.PostAsync("Genre", content ).Result ) {

                if(response.IsSuccessStatusCode)
                    Console.WriteLine($"Genre {genre} créé avec succès");
                else
                    Console.WriteLine("Erreur lors de la création du genre.");
            }
        }
    }
}
