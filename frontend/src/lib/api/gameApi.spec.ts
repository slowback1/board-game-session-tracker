import { beforeEach, type Mock } from 'vitest';
import { mockApi } from '$lib/testHelpers/getFetchMock';
import { testCreateGameRequest, testGameResponse } from '$lib/testHelpers/testData/testGameData';
import GameApi from '$lib/api/gameApi';

describe('GameApi', () => {
	let mockFetch: Mock;

	beforeEach(() => {
		mockFetch = mockApi({
			Game: { response: testGameResponse },
			'Game/ForUser': { response: [testGameResponse] }
		});
	});

	let api = new GameApi();

	it('calls the correct url and method to create a game', async () => {
		let result = await api.CreateGame(testCreateGameRequest);

		expect(mockFetch).toHaveBeenCalled();

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain('Game');
		expect(options.method).toEqual('POST');
	});

	it('calls the correct url and method to get the list of games for the user', async () => {
		let result = await api.GetGamesForUser();

		expect(mockFetch).toHaveBeenCalled();

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain('Game/ForUser');
		expect(options.method).toEqual('GET');
	});
});
