import { fireEvent, render, type RenderResult, waitFor } from '@testing-library/svelte';
import GameShareLink from '$lib/partials/game/GameShareLink.svelte';
import { afterEach, beforeEach } from 'vitest';
import { userEvent } from '@storybook/test';
import { SiteMap } from '$lib/utils/siteMap';

describe('GameShareLink', () => {
	let result: RenderResult<GameShareLink>;

	function renderComponent(gameId: string = '1234') {
		if (result) result.unmount();

		result = render(GameShareLink, { props: { gameId } });
	}

	beforeEach(() => {
		userEvent.setup({ writeToClipboard: true });
		vi.useFakeTimers();

		renderComponent();
	});

	afterEach(() => {
		vi.useRealTimers();
	});

	it('renders a game share link', () => {
		let shareLink = result.getByTestId('game-share-link');

		expect(shareLink).toBeInTheDocument();
		expect(shareLink).toHaveTextContent('Copy Share Link');
	});

	function mockClipboard() {
		let mock = vi.fn();
		navigator.clipboard.writeText = mock;

		return mock;
	}

	it('clicking the share link copies the share link to the clipboard', () => {
		let shareLink = result.getByTestId('game-share-link');

		let mock = mockClipboard();

		fireEvent.click(shareLink);

		expect(mock).toHaveBeenCalledWith('localhost:3000' + SiteMap.game.join('1234'));
	});

	it("clicking the share link changes the text to 'Link Copied!'", async () => {
		let shareLink = result.getByTestId('game-share-link');

		fireEvent.click(shareLink);

		await waitFor(() => {
			let shareLink = result.getByTestId('game-share-link');

			expect(shareLink).toHaveTextContent('Link Copied!');
		});
	});

	it('clicking the share link and waiting 5 seconds moves the link text back to the original text', async () => {
		let shareLink = result.getByTestId('game-share-link');

		fireEvent.click(shareLink);

		await waitFor(() => {
			let shareLink = result.getByTestId('game-share-link');

			expect(shareLink).toHaveTextContent('Link Copied!');
		});

		vi.advanceTimersByTime(5000);

		await waitFor(() => {
			let shareLink = result.getByTestId('game-share-link');

			expect(shareLink).toHaveTextContent('Copy Share Link');
		});
	});
});
