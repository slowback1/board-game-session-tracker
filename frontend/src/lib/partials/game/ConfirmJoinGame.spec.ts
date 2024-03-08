import type { GameConfirmationInfo } from '$lib/partials/game/GameService';
import { fireEvent, render, type RenderResult } from '@testing-library/svelte';
import ConfirmJoinGame from '$lib/partials/game/ConfirmJoinGame.svelte';
import { beforeEach } from 'vitest';
import { SiteMap } from '$lib/utils/siteMap';

describe('ConfirmJoinGame', () => {
	let result: RenderResult<ConfirmJoinGame>;

	function renderComponent(overrides: object = {}) {
		if (result) result.unmount();

		let props = {
			confirmationInfo: {
				hostPlayerName: 'slowback',
				gameName: 'Monster Hunter',
				gameId: 'game'
			} as GameConfirmationInfo,
			onConfirm: vi.fn(),
			...overrides
		};

		result = render(ConfirmJoinGame, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	it('renders a confirmation button', () => {
		let confirm = result.getByTestId('confirm-join__confirm');

		expect(confirm).toBeInTheDocument();
		expect(confirm).toHaveTextContent('Confirm');
	});

	it('clicking the confirmation button calls onConfirm with the given id', () => {
		let onConfirm = vi.fn();
		renderComponent({ onConfirm });

		let confirm = result.getByTestId('confirm-join__confirm');

		fireEvent.click(confirm);

		expect(onConfirm).toHaveBeenCalledWith('game');
	});

	it('contains some text indicating the name of the game that the user is joining', () => {
		let text = result.getByTestId('confirm-join__summary');

		expect(text).toHaveTextContent('Monster Hunter');
	});

	it("contains some text indicating the host user's username", () => {
		let text = result.getByTestId('confirm-join__summary');

		expect(text).toHaveTextContent('slowback');
	});

	it("contains a cancel button that links back to the player's home", () => {
		let cancel = result.getByTestId('confirm-join__cancel');

		expect(cancel).toBeInTheDocument();
		expect(cancel).toHaveTextContent('Cancel');
		expect(cancel).toHaveAttribute('href', SiteMap.app.home());
	});
});
