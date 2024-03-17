import PlayerInventoryApi, { type PlayerInventory } from '$lib/api/playerInventoryApi';
import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';

export default class PlayerInventoryService {
	errors: string[] = [];

	constructor(private gameId: string) {
		this.GetPlayerInventory();
	}

	async UpdatePlayerInventory(playerId: string, inventoryTypeOptionId: string, amount: number) {
		let api = new PlayerInventoryApi();

		let result = await api.updateSinglePlayerInventory(this.gameId, {
			playerId,
			amount,
			inventoryTypeOptionId: inventoryTypeOptionId
		});

		if (result.errors) {
			this.errors = result.errors;
		} else await this.GetPlayerInventory();
	}

	async UpdateAllPlayerInventory(inventoryTypeOptionId: string, amountToChange: number) {
		let api = new PlayerInventoryApi();

		let result = await api.updateAllPlayerInventories(this.gameId, {
			inventoryTypeOptionId,
			amountChanged: amountToChange
		});

		if (result.errors) {
			this.errors = result.errors;
		} else await this.GetPlayerInventory();
	}

	public async GetPlayerInventory() {
		let api = new PlayerInventoryApi();

		let result = await api.getPlayerInventoryForGame(this.gameId);

		if (result.errors) {
			this.errors = result.errors;
			this.SendPlayerInventoryMessage([]);
		} else this.SendPlayerInventoryMessage(result.response);
	}

	private SendPlayerInventoryMessage(inventory: PlayerInventory[]) {
		MessageBus.sendMessage(Messages.PlayerInventory, inventory);
	}
}
