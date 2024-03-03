import { act, fireEvent, render, type RenderResult, waitFor } from '@testing-library/svelte';
import InventoryTypeForm from '$lib/partials/inventoryTypes/InventoryTypeForm.svelte';
import { beforeEach } from 'vitest';
import { testInventoryTypeResponse } from '$lib/testHelpers/testData/testInventoryTypeData';

describe('InventoryTypeForm', () => {
	let result: RenderResult<InventoryTypeForm>;

	function renderComponent(props: Partial<{}> = {}) {
		if (result) result.unmount();

		result = render(InventoryTypeForm, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	async function addOptionRow() {
		let button = result.getByTestId('inventory-type-form__add-option');

		await act(() => {
			fireEvent.click(button);
		});

		await waitFor(() => {
			let field = result.getByTestId(`inventory-type-form__option-1-label`);

			expect(field).toBeInTheDocument();
		});
	}

	async function inputNameField() {
		let name = result.getByTestId('inventory-type-form__name');
		await fireEvent.input(name, { target: { value: 'name' } });
	}

	async function inputOptionLabelAndValue(i: number) {
		let label = result.getByTestId(`inventory-type-form__option-${i}-label`);
		let value = result.getByTestId(`inventory-type-form__option-${i}-value`);

		await fireEvent.input(label, { target: { value: `label ${i}` } });
		await fireEvent.input(value, { target: { value: `value ${i}` } });
	}

	it('renders a form', () => {
		expect(result.container.querySelector('form')).toBeInTheDocument();
	});

	it('will render an error summary if given errors', () => {
		renderComponent({ errors: ['test error'] });

		expect(result.getByTestId('error-summary')).toBeInTheDocument();
	});

	it('contains a field for name', () => {
		let field = result.getByTestId('inventory-type-form__name');

		expect(field).toBeInTheDocument();

		let label = result.getByTestId('inventory-type-form__name-label');

		expect(label).toBeInTheDocument();
		expect(label).toHaveTextContent('Name');
	});

	it.each([
		['label', 'Label'],
		['value', 'Value']
	])('contains a %s field for options with %s for the label', (testIdPart, expectedLabel) => {
		let field = result.getByTestId(`inventory-type-form__option-0-${testIdPart}`);
		let label = result.getByTestId(`inventory-type-form__option-0-${testIdPart}-label`);

		expect(field).toBeInTheDocument();
		expect(label).toBeInTheDocument();
		expect(label).toHaveTextContent(expectedLabel);
	});

	it('contains an add option button', () => {
		let button = result.getByTestId('inventory-type-form__add-option');

		expect(button).toBeInTheDocument();
	});

	it.each(['label', 'value'])(
		'clicking the add option button adds another %s field',
		async (testId) => {
			let button = result.getByTestId('inventory-type-form__add-option');

			await act(() => {
				fireEvent.click(button);
			});

			await waitFor(() => {
				let field = result.getByTestId(`inventory-type-form__option-1-${testId}`);

				expect(field).toBeInTheDocument();
			});
		}
	);

	it('contains a remove option button', () => {
		let button = result.getByTestId('inventory-type-form__remove-option-0');

		expect(button).toBeInTheDocument();
	});

	it('clicking the remove option button removes the option row', async () => {
		await addOptionRow();

		let removeButton = result.getByTestId('inventory-type-form__remove-option-1');

		await fireEvent.click(removeButton);

		await waitFor(() => {
			let field = result.queryByTestId(`inventory-type-form__option-1-label`);

			expect(field).not.toBeInTheDocument();
		});
	});

	it("the remove option button is disabled if there's only one option row", async () => {
		let removeButton = result.getByTestId('inventory-type-form__remove-option-0');

		expect(removeButton).toBeDisabled();
	});

	it("the remove option button is not disabled if there's more than one row", async () => {
		await addOptionRow();

		let removeButton = result.getByTestId('inventory-type-form__remove-option-1');

		expect(removeButton).toBeEnabled();
	});

	it('contains a submit button', () => {
		let submit = result.getByTestId('inventory-type-form__submit');

		expect(submit).toBeInTheDocument();
	});

	it('filling out the form and clicking the submit button calls onSubmit with the filled in form values', async () => {
		let onSubmit = vi.fn();
		renderComponent({ onSubmit });
		await inputNameField();

		await addOptionRow();

		for (let i = 0; i < 2; i++) {
			await inputOptionLabelAndValue(i);
		}

		await waitFor(() => {
			let value = result.getByTestId(`inventory-type-form__option-1-value`);

			expect(value).toHaveValue('value 1');
		});

		let submit = result.getByTestId('inventory-type-form__submit');

		await fireEvent.click(submit);

		expect(onSubmit).toHaveBeenCalledWith({
			name: 'name',
			options: [
				{ label: 'label 0', value: 'value 0' },
				{ label: 'label 1', value: 'value 1' }
			]
		});
	});

	it('can take in an inventory type and fill out the form with the given values', async () => {
		renderComponent({ inventoryType: testInventoryTypeResponse });

		await waitFor(() => {
			let name = result.getByTestId('inventory-type-form__name');
			expect(name).toHaveValue(testInventoryTypeResponse.name);

			for (let i = 0; i < 2; i++) {
				let label = result.getByTestId(`inventory-type-form__option-${i}-label`);
				let value = result.getByTestId(`inventory-type-form__option-${i}-value`);

				expect(label).toHaveValue(testInventoryTypeResponse.options[i].label);
				expect(value).toHaveValue(testInventoryTypeResponse.options[i].value);
			}
		});
	});
});
