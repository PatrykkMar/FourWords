using Microsoft.EntityFrameworkCore;
using FourWords.Server.Context;
using FourWords.Server.Helpers;
using FourWords.Server.Models;

namespace FourWords.Server.Services
{
    public interface IGameService
    {
        public Task StartGameAsync(string roomCode);
    }

    public class GameService(FourWordsDBContext context, ICardManager cardManager) : IGameService
    {
        private readonly FourWordsDBContext _context = context;
        private readonly ICardManager _cardManager = cardManager;

        public async Task StartGameAsync(string playerConnectionId)
        {
            var room = await _context.GameRooms
                .Include(r => r.Players)
                .FirstOrDefaultAsync(r => r.Players.Any(p => p.ConnectionId == playerConnectionId));

            if (room == null) throw new Exception("Room does not exist");
            if (room.Status != GameStatus.Lobby) throw new Exception("Game already started");
            if (room.Players.Count < 2) throw new Exception("Not enough players to start");
            room.Status = GameStatus.Writing;


            await _cardManager.AssignCardsToPlayersAsync([.. room.Players.Select(x => x.Id)], room.Id);
            await _context.SaveChangesAsync();
        }
    }
}
