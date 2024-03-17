import { beforeEach, type Mock } from 'vitest';
import PlayerInventoryService from '$lib/partials/playerInventory/PlayerInventoryService';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import { testPlayerInventory } from '$lib/testHelpers/testData/testPlayerInventoryResponses';
import { waitFor } from '@testing-library/svelte';
import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';
import type { PlayerInventory } from '$lib/api/playerInventoryApi';
import { testErrorApiResponse } from '$lib/testHelpers/testData/testApiErrorResponses';

describe('PlayerInventoryService', () => {
	let service: PlayerInventoryService;
	let mockFetch: Mock;

	describe('constructing with a good response', () => {
		beforeEach(() => {
			mockFetch = getFetchMock({ response: [testPlayerInventory] });

			service = new PlayerInventoryService('game');
		});

		it('eventually adds the player inventory to the message bus', async () => {
			await waitFor(() => {
				let playerInventory = MessageBus.getLastMessage<PlayerInventory[]>(
					Messages.PlayerInventory
				);

				expect(playerInventory).toEqual([testPlayerInventory]);
			});
		});

		it('eventually calls the api', async () => {
			await waitFor(() => {
				let urls = mockFetch.mock.calls.map((c) => c[0]);

				expect(urls).toContain(`PlayerInventory/game`);
			});
		});

		describe('updating player inventory', () => {
			beforeEach(async () => {
				await waitFor(() => {
					let playerInventory = MessageBus.getLastMessage<PlayerInventory[]>(
						Messages.PlayerInventory
					);

					expect(playerInventory).toEqual([testPlayerInventory]);
				});
			});

			it('UpdatePlayerInventory calls the API to update the player inventory', async () => {
				mockFetch.mockClear();
				await service.UpdatePlayerInventory('player', 'option', 5);

				let urls = mockFetch.mock.calls.map((c) => c[0]);

				expect(urls).toContain(`PlayerInventory/game/SinglePlayer`);
			});

			it('UpdatePlayerInventory re-calls the API with the updated player inventory', async () => {
				mockFetch.mockClear();
				await service.UpdatePlayerInventory('player', 'option', 5);

				let urls = mockFetch.mock.calls.map((c) => c[0]);

				expect(urls).toContain(`PlayerInventory/game`);
			});

			it('UpdatePlayerInventory does not re-calls the API with the updated player inventory when it errors', async () => {
				mockFetch = getFetchMock(testErrorApiResponse);
				await service.UpdatePlayerInventory('player', 'option', 5);

				let urls = mockFetch.mock.calls.map((c) => c[0]);

				expect(urls).not.toContain(`PlayerInventory/game`);
			});

			it('UpdatePlayerInventory shows the errors when given an error response', async () => {
				mockFetch = getFetchMock(testErrorApiResponse);

				await service.UpdatePlayerInventory('player', 'option', 5);

				expect(service.errors).toEqual(testErrorApiResponse.errors);
			});

			it('UpdateAllPlayerInventory calls the API', async () => {
				mockFetch.mockClear();
				await service.UpdateAllPlayerInventory('option', 5);

				let urls = mockFetch.mock.calls.map((c) => c[0]);

				expect(urls).toContain(`PlayerInventory/game/AllPlayers`);
			});

			it('UpdateAllPlayerInventory also re-gets the player inventory', async () => {
				mockFetch.mockClear();
				await service.UpdateAllPlayerInventory('option', 5);

				let urls = mockFetch.mock.calls.map((c) => c[0]);

				expect(urls).toContain(`PlayerInventory/game`);
			});

			it('UpdateAllPlayerInventory does not re-calls the API with the updated player inventory when it errors', async () => {
				mockFetch = getFetchMock(testErrorApiResponse);
				await service.UpdateAllPlayerInventory('option', 5);

				let urls = mockFetch.mock.calls.map((c) => c[0]);

				expect(urls).not.toContain(`PlayerInventory/game`);
			});

			it('UpdateAllPlayerInventory shows the errors when given an error response', async () => {
				mockFetch = getFetchMock(testErrorApiResponse);

				await service.UpdateAllPlayerInventory('option', 5);

				expect(service.errors).toEqual(testErrorApiResponse.errors);
			});
		});
	});

	describe('constructing with an error response', () => {
		beforeEach(() => {
			getFetchMock(testErrorApiResponse);

			service = new PlayerInventoryService('1234');
		});

		it('exposes the api errors', async () => {
			await waitFor(() => {
				expect(service.errors).toEqual(testErrorApiResponse.errors);
			});
		});

		it('sends an empty array to the message bus', async () => {
			await waitFor(() => {
				let playerInventory = MessageBus.getLastMessage<PlayerInventory[]>(
					Messages.PlayerInventory
				);

				expect(playerInventory).toEqual([]);
			});
		});
	});
});
