import InventoryTypeService from '$lib/partials/inventoryTypes/InventoryTypeService';
import { beforeEach } from 'vitest';
import {
	testCreateInventoryTypeRequest,
	testEditInventoryTypeRequest,
	testInventoryTypeResponse
} from '$lib/testHelpers/testData/testInventoryTypeData';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import { SiteMap } from '$lib/utils/siteMap';
import { testErrorApiResponse } from '$lib/testHelpers/testData/testApiErrorResponses';

describe('InventoryTypeService', () => {
	const testGameId = '1234-5678-9101';
	const navigate = vi.fn();
	const service = new InventoryTypeService(testGameId, navigate);

	beforeEach(() => {
		navigate.mockClear();
		service.errors = [];
	});

	it('calls the API when creating a game', async () => {
		let mockFetch = getFetchMock({ response: testInventoryTypeResponse });
		let result = await service.CreateInventoryType(testCreateInventoryTypeRequest);

		expect(mockFetch).toHaveBeenCalled();
	});

	it('calls navigate on a successful response', async () => {
		let mockFetch = getFetchMock({ response: testInventoryTypeResponse });
		let result = await service.CreateInventoryType(testCreateInventoryTypeRequest);

		expect(navigate).toHaveBeenCalledWith(SiteMap.game.home(testGameId));
	});

	it('does not call navigate on an error response', async () => {
		let mockFetch = getFetchMock(testErrorApiResponse);
		let result = await service.CreateInventoryType(testCreateInventoryTypeRequest);

		expect(navigate).not.toHaveBeenCalled();
	});

	it('stores the errors on the class when creating with an error response', async () => {
		let mockFetch = getFetchMock(testErrorApiResponse);
		let result = await service.CreateInventoryType(testCreateInventoryTypeRequest);

		expect(service.errors).toEqual(testErrorApiResponse.errors);
	});

	it('calls the API when editing an inventory type', async () => {
		let mockFetch = getFetchMock({ response: testInventoryTypeResponse });
		let result = await service.EditInventoryType(testEditInventoryTypeRequest);

		expect(mockFetch).toHaveBeenCalled();
	});

	it('calls navigate when editing an inventory type with a successful response', async () => {
		let mockFetch = getFetchMock({ response: testInventoryTypeResponse });
		let result = await service.EditInventoryType(testEditInventoryTypeRequest);

		expect(navigate).toHaveBeenCalledWith(SiteMap.game.home(testGameId));
	});

	it('does not call navigate when editing an inventory type with a failed response', async () => {
		let mockFetch = getFetchMock(testErrorApiResponse);
		let result = await service.EditInventoryType(testEditInventoryTypeRequest);

		expect(navigate).not.toHaveBeenCalled();
	});

	it('sets the errors when editing with an error response', async () => {
		let mockFetch = getFetchMock(testErrorApiResponse);
		let result = await service.EditInventoryType(testEditInventoryTypeRequest);

		expect(service.errors).toEqual(testErrorApiResponse.errors);
	});

	it('calls the api when getting an inventory type by id', async () => {
		let mockFetch = getFetchMock({ response: testInventoryTypeResponse });
		let result = await service.GetInventoryType('1234');

		expect(mockFetch).toHaveBeenCalled();
	});

	it('returns the result when getting an inventory type by id', async () => {
		let mockFetch = getFetchMock({ response: testInventoryTypeResponse });
		let result = await service.GetInventoryType('1234');

		expect(result).toEqual(testInventoryTypeResponse);
	});

	it('returns null when given an error  response', async () => {
		let mockFetch = getFetchMock(testErrorApiResponse);
		let result = await service.GetInventoryType('1234');

		expect(result).toEqual(null);
	});

	it('sets the errors on the service when the get by id method returns an error response', async () => {
		let mockFetch = getFetchMock(testErrorApiResponse);
		let result = await service.GetInventoryType('1234');

		expect(service.errors).toEqual(testErrorApiResponse.errors);
	});
});
