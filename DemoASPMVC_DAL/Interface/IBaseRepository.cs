namespace DemoASPMVC_DAL.Interface
{
    public interface IBaseRepository<TModel>
    {
        void Delete(string route, int id, string token );
        IEnumerable<TModel> GetAll(string route);
        TModel GetById(string route, int id);
    }
}