import type { PlayerInventoryItem } from '$lib/api/playerInventoryApi';
import { testPlayerInventoryItem } from '$lib/testHelpers/testData/testPlayerInventoryResponses';
import { fireEvent, render, type RenderResult } from '@testing-library/svelte';
import PlayerInventoryItemField from '$lib/partials/playerInventory/PlayerInventoryItemField.svelte';
import { beforeEach } from 'vitest';

describe('PlayerInventoryItemField', () => {
	let result: RenderResult<PlayerInventoryItemField>;

	function renderComponent(
		overrides: Partial<{
			inventoryItem: PlayerInventoryItem;
			playerId: string;
			onUpdate: (playerId: string, inventoryTypeOptionId: string, amount: number) => void;
		}> = {}
	) {
		let props = {
			inventoryItem: testPlayerInventoryItem,
			playerId: 'player',
			onUpdate: vi.fn(),
			...overrides
		};

		if (result) result.unmount();

		result = render(PlayerInventoryItemField, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	it('renders the name of the inventory item', () => {
		let name = result.getByTestId('player-inventory-item__name');

		expect(name).toBeInTheDocument();
		expect(name).toHaveTextContent(testPlayerInventoryItem.name);
	});

	it('contains a button that increments the amount', () => {
		let button = result.getByTestId('player-inventory-item__increment');

		expect(button).toBeInTheDocument();
	});

	it('clicking the increment button calls onUpdate with the amount plus 1', () => {
		let onUpdateMock = vi.fn();
		renderComponent({ onUpdate: onUpdateMock });

		let button = result.getByTestId('player-inventory-item__increment');

		fireEvent.click(button);

		expect(onUpdateMock).toHaveBeenCalledWith(
			'player',
			testPlayerInventoryItem.inventoryTypeOptionId,
			testPlayerInventoryItem.amount + 1
		);
	});

	it('contains a button to decrement the amount', () => {
		let button = result.getByTestId('player-inventory-item__decrement');

		expect(button).toBeInTheDocument();
	});

	it('clicking the decrement button calls onUpdate will decrement the amount by 1', () => {
		let onUpdateMock = vi.fn();
		renderComponent({ onUpdate: onUpdateMock });

		let button = result.getByTestId('player-inventory-item__decrement');

		fireEvent.click(button);

		expect(onUpdateMock).toHaveBeenCalledWith(
			'player',
			testPlayerInventoryItem.inventoryTypeOptionId,
			testPlayerInventoryItem.amount - 1
		);
	});

	it('displays the current amount', () => {
		let amount = result.getByTestId('player-inventory-item__amount');

		expect(amount).toBeInTheDocument();
		expect(amount).toHaveTextContent(`${testPlayerInventoryItem.amount}`);
	});
});
