using IronDomeAPI.Models;

namespace IronDomeAPI.Services
{
    public interface IDbService<T>
    {
        public List<T> Attacks { get; set; }
    }

    public class DbService
    {
        public static List<Attack> AttacksList = new List<Attack>();
       
    
    }
}
