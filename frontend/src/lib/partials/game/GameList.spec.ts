import { beforeEach, type Mock } from 'vitest';
import { render, type RenderResult, waitFor } from '@testing-library/svelte';
import GameList from '$lib/partials/game/GameList.svelte';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import { testCreateGameRequest, testGameResponse } from '$lib/testHelpers/testData/testGameData';
import { logUserInWithTestData } from '$lib/testHelpers/messageBusSetupHelpers';
import { SiteMap } from '$lib/utils/siteMap';

describe('GameList', () => {
	let result: RenderResult<GameList>;
	let mockFetch: Mock;

	beforeEach(() => {
		logUserInWithTestData();

		mockFetch = getFetchMock({ response: [testGameResponse] });
		result = render(GameList);
	});

	it('eventually calls fetch', async () => {
		await waitFor(() => {
			expect(mockFetch).toHaveBeenCalled();
		});
	});

	it('contains a link to create a new game', () => {
		let link = result.getByTestId('game-list__add-game');

		expect(link).toBeInTheDocument();
		expect(link).toHaveAttribute('href', SiteMap.game.create());
	});

	it('eventually contains a list of games', async () => {
		await waitFor(() => {
			let gameItem = result.getAllByTestId('game-list__game');

			expect(gameItem[0]).toBeInTheDocument();
			expect(gameItem[0]).toHaveTextContent(testGameResponse.gameName);
		});
	});

	it("eventually contains a link to go to that game's dashboard", async () => {
		await waitFor(() => {
			let gameLinks = result.getAllByTestId('game-list__game-link');

			let firstItem = gameLinks[0];

			expect(firstItem).toBeInTheDocument();
			expect(firstItem).toHaveAttribute('href', SiteMap.game.home(testGameResponse.gameId));
		});
	});
});
