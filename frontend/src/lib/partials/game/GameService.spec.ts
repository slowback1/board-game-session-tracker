import { beforeEach, describe, type Mock } from 'vitest';
import { testCreateGameRequest, testGameResponse } from '$lib/testHelpers/testData/testGameData';
import GameService from '$lib/partials/game/GameService';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import { SiteMap } from '$lib/utils/siteMap';
import { testErrorApiResponse } from '$lib/testHelpers/testData/testApiErrorResponses';

describe('GameService', () => {
	let service: GameService;
	let mockNavigate: Mock;
	beforeEach(() => {
		mockNavigate = vi.fn();
		service = new GameService(mockNavigate);
	});

	describe('creating a game', () => {
		let mockFetch: Mock;

		beforeEach(() => {
			mockFetch = getFetchMock({ response: testGameResponse });
		});

		it('calls the API to create the game', async () => {
			await service.CreateGame(testCreateGameRequest);

			expect(mockFetch).toHaveBeenCalled();
		});

		it('redirects the user back to the home page after the game has been created', async () => {
			await service.CreateGame(testCreateGameRequest);

			expect(mockNavigate).toHaveBeenCalledWith(SiteMap.app.home());
		});

		it('sets the errors if the api sends an error response', async () => {
			mockFetch = getFetchMock(testErrorApiResponse);

			await service.CreateGame(testCreateGameRequest);

			expect(service.errors).toEqual(testErrorApiResponse.errors);
		});

		it('does not navigate if there are errors', async () => {
			mockFetch = getFetchMock(testErrorApiResponse);

			await service.CreateGame(testCreateGameRequest);

			expect(mockNavigate).not.toHaveBeenCalled();
		});
	});
});
