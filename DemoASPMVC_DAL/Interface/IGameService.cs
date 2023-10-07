using DemoASPMVC_DAL.Models;

namespace DemoASPMVC_DAL.Interface
{
    public interface IGameService : IBaseRepository<Game>
    {
        void Create(Game game, string token);
        IEnumerable<Game> GetByUserId(int userId, string token );
        void AddFavorite(int idUser, int idGame, string token);
    }
}