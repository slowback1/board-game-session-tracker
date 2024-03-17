import { render, type RenderResult } from '@testing-library/svelte';
import PlayerInventoryGroup from '$lib/partials/playerInventory/PlayerInventoryGroup.svelte';
import type { ComponentProps } from 'svelte';
import { beforeEach, expect } from 'vitest';
import { testPlayerInventoryItemGroup } from '$lib/testHelpers/testData/testPlayerInventoryResponses';

describe('PlayerInventoryGroup', () => {
	let result: RenderResult<PlayerInventoryGroup>;

	function renderComponent(overrides: Partial<ComponentProps<PlayerInventoryGroup>> = {}) {
		let props = {
			inventoryGroup: testPlayerInventoryItemGroup,
			playerId: 'player',
			playerName: 'bob',
			onUpdate: vi.fn(),
			...overrides
		};

		if (result) result.unmount();

		result = render(PlayerInventoryGroup, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	it('renders a PlayerInventoryItemField for each item in the group', () => {
		let playerInventoryItemFields = result.getAllByTestId('player-inventory-item__name');

		expect(playerInventoryItemFields.length).toEqual(3);
	});

	it('renders the player name', () => {
		let playerNameField = result.getByTestId('player-inventory-group__player-name');

		expect(playerNameField).toBeInTheDocument();
		expect(playerNameField).toHaveTextContent('bob');
	});
});
