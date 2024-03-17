import BaseApi, { type ApiResponse } from '$lib/api/baseApi';

export type PlayerInventory = {
	playerName: string;
	playerId: string;
	inventory: PlayerInventoryItemGroup[];
};

export type PlayerInventoryItemGroup = {
	inventoryTypeName: string;
	inventoryTypeId: string;
	items: PlayerInventoryItem[];
};

export type PlayerInventoryItem = {
	inventoryTypeOptionId: string;
	name: string;
	amount: number;
};

export type UpdatePlayerInventoryRequest = {
	playerId: string;
	inventoryTypeOptionId: string;
	amount: number;
};

export type UpdateAllPlayerInventoriesRequest = {
	inventoryTypeOptionId: string;
	amountChanged: number;
};

export default class PlayerInventoryApi extends BaseApi {
	getPlayerInventoryForGame(gameId: string): Promise<ApiResponse<PlayerInventory[]>> {
		return this.Get(`PlayerInventory/${gameId}`);
	}

	updateSinglePlayerInventory(
		gameId: string,
		request: UpdatePlayerInventoryRequest
	): Promise<ApiResponse<PlayerInventoryItem>> {
		return this.Put(`PlayerInventory/${gameId}/SinglePlayer`, request);
	}

	updateAllPlayerInventories(
		gameId: string,
		request: UpdateAllPlayerInventoriesRequest
	): Promise<ApiResponse<PlayerInventoryItem[]>> {
		return this.Put(`PlayerInventory/${gameId}/AllPlayers`, request);
	}
}
