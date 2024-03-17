import type {
	PlayerInventory,
	PlayerInventoryItem,
	PlayerInventoryItemGroup,
	UpdateAllPlayerInventoriesRequest,
	UpdatePlayerInventoryRequest
} from '$lib/api/playerInventoryApi';

export const testPlayerInventoryItem: PlayerInventoryItem = {
	inventoryTypeOptionId: '1234',
	amount: 5,
	name: 'test'
};

export const testPlayerInventoryItemGroup: PlayerInventoryItemGroup = {
	items: [testPlayerInventoryItem, testPlayerInventoryItem, testPlayerInventoryItem],
	inventoryTypeId: 'test',
	inventoryTypeName: 'test'
};

export const testPlayerInventory: PlayerInventory = {
	inventory: [testPlayerInventoryItemGroup, testPlayerInventoryItemGroup],
	playerId: 'abcd',
	playerName: 'bob'
};

export const testUpdatePlayerInventoryRequest: UpdatePlayerInventoryRequest = {
	amount: 5,
	inventoryTypeOptionId: '1234',
	playerId: '5555'
};

export const testAllPlayerInventoriesRequest: UpdateAllPlayerInventoriesRequest = {
	inventoryTypeOptionId: '1234',
	amountChanged: 5
};
