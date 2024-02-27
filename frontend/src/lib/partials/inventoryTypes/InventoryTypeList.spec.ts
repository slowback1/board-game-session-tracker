import { beforeEach, type Mock } from 'vitest';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import { testInventoryTypeResponse } from '$lib/testHelpers/testData/testInventoryTypeData';
import { render, type RenderResult, waitFor } from '@testing-library/svelte';
import InventoryTypeList from '$lib/partials/inventoryTypes/InventoryTypeList.svelte';
import { SiteMap } from '$lib/utils/siteMap';

describe('InventoryTypeList', () => {
	let result: RenderResult<InventoryTypeList>;
	let mockFetch: Mock;

	beforeEach(() => {
		mockFetch = getFetchMock({ response: [testInventoryTypeResponse] });

		result = render(InventoryTypeList, { props: { gameId: '1234' } });
	});

	it('eventually calls fetch', async () => {
		await waitFor(() => {
			expect(mockFetch).toHaveBeenCalled();
		});
	});

	it('has an add button that links to the add inventory type page', () => {
		let addButton = result.getByTestId('inventory-type-list__add');

		expect(addButton).toBeInTheDocument();
		expect(addButton).toHaveAttribute('href', SiteMap.inventoryTypes.create('1234'));
	});

	it('eventually contains a list of inventory types', async () => {
		await waitFor(() => {
			let row = result.getByTestId('inventory-type-list__row');

			expect(row).toBeInTheDocument();
		});
	});

	it.each([
		['name', testInventoryTypeResponse.name],
		['options-count', `${testInventoryTypeResponse.options.length}`]
	])('eventually contains data for %s', async (suffix, expectedTextContent) => {
		await waitFor(() => {
			let row = result.getByTestId('inventory-type-list__row');

			let cell = row.querySelector(`[data-testid="inventory-type-list__row-${suffix}"]`);

			expect(cell).toBeInTheDocument();
			expect(cell).toHaveTextContent(expectedTextContent);
		});
	});

	it('eventually contains an edit button that goes to the edit page', async () => {
		await waitFor(() => {
			let row = result.getByTestId('inventory-type-list__row');

			let cell = row.querySelector(`[data-testid="inventory-type-list__row-edit"]`);

			expect(cell).toBeInTheDocument();
			expect(cell).toHaveAttribute(
				'href',
				SiteMap.inventoryTypes.edit(testInventoryTypeResponse.inventoryTypeId)
			);
		});
	});
});
