using Database.Common.DTOs;

namespace Database.Supabase;

public partial class SupabaseStorer
{
    public Task<List<PlayerInventory>?> GetInventoryForGame(string gameId)
    {
        throw new NotImplementedException();
    }

    public Task<PlayerInventoryItem> UpdateInventoryForPlayer(string gameId, UpdatePlayerInventoryRequest request)
    {
        throw new NotImplementedException();
    }
}