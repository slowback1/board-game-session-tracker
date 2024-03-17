import { fireEvent, render, type RenderResult, waitFor } from '@testing-library/svelte';
import PlayerInventory from './PlayerInventory.svelte';
import { beforeEach, type Mock } from 'vitest';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import {
	testPlayerInventory,
	testPlayerInventoryItem,
	testPlayerInventoryItemGroup
} from '$lib/testHelpers/testData/testPlayerInventoryResponses';

const testResponse = [
	{
		...testPlayerInventory,
		inventory: [
			testPlayerInventoryItemGroup,
			{
				items: [testPlayerInventoryItem],
				inventoryTypeId: '9876',
				inventoryTypeName: 'something else'
			}
		],
		playerName: 'Rathalos'
	},
	{
		...testPlayerInventory,
		inventory: [
			testPlayerInventoryItemGroup,
			{
				items: [testPlayerInventoryItem],
				inventoryTypeId: '9876',
				inventoryTypeName: 'something else'
			}
		],
		playerName: 'Rathian'
	}
];
describe('PlayerInventory', () => {
	let result: RenderResult<PlayerInventory>;
	let mockFetch: Mock;

	beforeEach(() => {
		if (result) result.unmount();

		mockFetch = getFetchMock({ response: testResponse });
		result = render(PlayerInventory, { gameId: 'game' });
	});

	it('initially renders a loading spinner', () => {
		let loadingIndicator = result.getByTestId('loading-indicator');

		expect(loadingIndicator).toBeInTheDocument();
	});

	it('calls the API to get player inventory on mount', async () => {
		await waitFor(() => {
			expect(mockFetch).toHaveBeenCalled();
		});
	});

	it('eventually does not render the loading spinner', async () => {
		await waitFor(() => {
			let loadingIndicator = result.queryByTestId('loading-indicator');

			expect(loadingIndicator).not.toBeInTheDocument();
		});
	});

	describe('When the data is loaded', () => {
		beforeEach(async () => {
			await waitFor(() => {
				let loadingIndicator = result.queryByTestId('loading-indicator');

				expect(loadingIndicator).not.toBeInTheDocument();
			});
		});

		it('renders a select box to change the selected inventory type', () => {
			let select = result.getByTestId('player-inventory__inventory-type-select');

			expect(select).toBeInTheDocument();
			expect(select.tagName.toLowerCase()).toEqual('select');
		});

		it('has select options for each inventory type found', () => {
			let expectedLength = testPlayerInventory.inventory.length;

			let select = result.getByTestId('player-inventory__inventory-type-select');
			let options = select.querySelectorAll('option');

			expect(options.length).toEqual(expectedLength);
		});

		it.each([
			[0, testResponse[0].inventory[0].inventoryTypeId],
			[1, testResponse[0].inventory[1].inventoryTypeId]
		])('The select option has an option at index %s with value %s', (index, expectedValue) => {
			let select = result.getByTestId('player-inventory__inventory-type-select');
			let options = select.querySelectorAll('option');

			expect(options[index].value).toEqual(expectedValue);
		});

		it.each([
			[0, testResponse[0].inventory[0].inventoryTypeName],
			[1, testResponse[0].inventory[1].inventoryTypeName]
		])('The select option has an option at index %s with label %s', (index, expectedValue) => {
			let select = result.getByTestId('player-inventory__inventory-type-select');
			let options = select.querySelectorAll('option');

			expect(options[index]).toHaveTextContent(expectedValue);
		});

		it("has an 'all players' section for managing inventory group", () => {
			let allPlayersToggle = result.getAllByTestId('player-inventory-group__player-name')[0];

			expect(allPlayersToggle).toBeInTheDocument();
			expect(allPlayersToggle).toHaveTextContent('All Players');
		});

		function assertThatThereAreXNumberOfPlayerInventoryItemsInSection(amount: number) {
			let allPlayersSection = result.getAllByTestId('player-inventory-group__player-name')[0]
				.parentNode;

			let inventoryNodes = allPlayersSection.querySelectorAll(
				"[data-testid='player-inventory-item__name']"
			);

			expect(inventoryNodes.length).toEqual(amount);
		}

		it('there are player inventory items in the all players section', () => {
			assertThatThereAreXNumberOfPlayerInventoryItemsInSection(
				testResponse[0].inventory[0].items.length
			);
		});

		it('selecting the other option changes the displayed items in the all players section', async () => {
			let select = result.getByTestId('player-inventory__inventory-type-select');
			let option = select.children[1] as HTMLOptionElement;

			fireEvent.change(select, { target: { value: option.value } });

			await waitFor(() => {
				assertThatThereAreXNumberOfPlayerInventoryItemsInSection(
					testResponse[0].inventory[1].items.length
				);
			});
		});

		it.each([
			[1, 'Rathalos'],
			[2, 'Rathian']
		])(
			'contains sections for each of the players (%s, %s)',
			(index: number, expectedName: string) => {
				let playerToggle = result.getAllByTestId('player-inventory-group__player-name')[index];

				expect(playerToggle).toBeInTheDocument();
				expect(playerToggle).toHaveTextContent(expectedName);
			}
		);
	});
});
