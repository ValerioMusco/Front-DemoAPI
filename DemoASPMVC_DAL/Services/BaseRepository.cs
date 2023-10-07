using DemoASPMVC_DAL.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DemoASPMVC_DAL.Exceptions;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DemoASPMVC_DAL.Services {

    public abstract class BaseRepository<TModel> : IBaseRepository<TModel> {

        private readonly string _url = "https://localhost:7278/api/";
        protected readonly HttpClient _client = new();
        protected List<TModel> modelList = new();

        public BaseRepository() {
            _client.BaseAddress = new Uri( _url );
        }
        protected abstract TModel Mapper( TModel model );

        public virtual IEnumerable<TModel> GetAll(string route) {

            List<TModel> list = new();

            using(HttpResponseMessage response = _client.GetAsync( route ).Result ) {
                
                if( response.IsSuccessStatusCode ) {

                    string json = response.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<TModel>>( json );
                }
                else
                    Console.WriteLine(response.StatusCode);
            }

            return list;
        }

        public virtual TModel GetById( string route, int id ) {

            TModel model;
            using(HttpResponseMessage response = _client.GetAsync( route + "/Details/" + id ).Result ) {

                if( response.IsSuccessStatusCode ) {

                    string json = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<TModel>( json );
                }
                else {

                    Console.WriteLine(response.StatusCode);
                    throw new ModelNotFoundException($"Le modèle avec l'ID {id} n'a pas été trouvé.");
                }
            }
            return model;
        }

        public virtual void Delete( string route, int id, string token ) {

            _client.DefaultRequestHeaders.Add( "Authorization", "Bearer " + token );
            using( HttpResponseMessage response = _client.DeleteAsync( route + "?id=" + id ).Result ) {

                if( response.IsSuccessStatusCode )
                    Console.WriteLine("Jeu supprimé");
                else
                    Console.WriteLine(response.StatusCode);
            }
        }
    }
}
