import { beforeEach, type Mock } from 'vitest';
import { mockApi } from '$lib/testHelpers/getFetchMock';
import {
	testCreateInventoryTypeRequest,
	testEditInventoryTypeRequest,
	testInventoryTypeResponse
} from '$lib/testHelpers/testData/testInventoryTypeData';
import InventoryTypeApi from '$lib/api/inventoryTypeApi';

describe('InventoryTypeApi', () => {
	let mockFetch: Mock;

	beforeEach(() => {
		mockFetch = mockApi({
			'InventoryTypes/1234': { response: testInventoryTypeResponse },
			'InventoryTypes/Create/1234': { response: testInventoryTypeResponse },
			'InventoryTypes/Edit/1234': { response: testInventoryTypeResponse },
			'InventoryTypes/ForGame/1234': { response: [testInventoryTypeResponse] }
		});
	});

	let api = new InventoryTypeApi();

	it('calls the correct URL to get an inventory type by ID', async () => {
		let result = await api.GetInventoryTypeById('1234');

		expect(mockFetch).toHaveBeenCalled();
		let [url] = mockFetch.mock.lastCall;
		expect(url).toContain('InventoryTypes/1234');

		expect(result.response).toEqual(testInventoryTypeResponse);
	});

	it('calls the correct URL and method to create an inventory type', async () => {
		let result = await api.CreateInventoryType('1234', testCreateInventoryTypeRequest);

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain('InventoryTypes/Create/1234');
		expect(options.method).toEqual('POST');
	});

	it('calls the correct URL and method to edit an inventory type', async () => {
		let result = await api.EditInventoryType('1234', testEditInventoryTypeRequest);

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain('InventoryTypes/Edit/1234');
		expect(options.method).toEqual('PUT');
	});

	it('calls the correct URL for getting the inventory types for a game', async () => {
		let result = await api.GetInventoryTypesForGame('1234');

		expect(mockFetch).toHaveBeenCalled();
		let [url] = mockFetch.mock.lastCall;
		expect(url).toContain('InventoryTypes/ForGame/1234');
	});
});
