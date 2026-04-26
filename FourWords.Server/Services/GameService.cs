using FourWords.Server.Context;

namespace FourWords.Server.Services
{
    public interface IGameService
    {

    }

    public class GameService(FourWordsDBContext context) : IGameService
    {
        private readonly FourWordsDBContext _context = context;
    }
}
