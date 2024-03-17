import PlayerInventoryApi from '$lib/api/playerInventoryApi';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import {
	testAllPlayerInventoriesRequest,
	testPlayerInventory,
	testPlayerInventoryItem,
	testUpdatePlayerInventoryRequest
} from '$lib/testHelpers/testData/testPlayerInventoryResponses';

describe('PlayerInventoryApi', () => {
	let api = new PlayerInventoryApi();
	it('calls the correct URL when getting player inventory for the game', async () => {
		let mockFetch = getFetchMock({ response: [testPlayerInventory] });

		let result = await api.getPlayerInventoryForGame('1234');

		expect(result.response[0]).toEqual(testPlayerInventory);

		expect(mockFetch).toHaveBeenCalled();
		let [url] = mockFetch.mock.lastCall;

		expect(url).toContain('PlayerInventory/1234');
	});

	it("calls the correct URL and method when updating a single player's inventory", async () => {
		let mockFetch = getFetchMock({ response: testPlayerInventoryItem });

		let result = await api.updateSinglePlayerInventory('1234', testUpdatePlayerInventoryRequest);

		expect(mockFetch).toHaveBeenCalled();

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain(`PlayerInventory/1234/SinglePlayer`);
		expect(options.method).toEqual('PUT');
	});

	it('calls the correct URL and method when updating all player inventories', async () => {
		let mockFetch = getFetchMock({ response: [testPlayerInventoryItem] });

		let result = await api.updateAllPlayerInventories('1234', testAllPlayerInventoriesRequest);

		expect(mockFetch).toHaveBeenCalled();

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain(`PlayerInventory/1234/AllPlayers`);
		expect(options.method).toEqual('PUT');
	});
});
