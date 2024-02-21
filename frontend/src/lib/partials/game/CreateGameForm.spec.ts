import { act, fireEvent, render, type RenderResult, waitFor } from '@testing-library/svelte';
import CreateGameForm from '$lib/partials/game/CreateGameForm.svelte';
import { beforeEach } from 'vitest';

describe('CreateGameForm', () => {
	let result: RenderResult<CreateGameForm>;

	function renderComponent(props: Partial<{ onSubmit: () => {}; errors: string[] }> = {}) {
		if (result) result.unmount();

		result = render(CreateGameForm, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	it('contains a form', () => {
		let form = result.container.querySelector('form');

		expect(form).toBeInTheDocument();
	});

	it('contains a field for setting the game name', () => {
		let gameNameLabel = result.getByTestId('create-game-form__game-name-label');
		let gameNameField = result.getByTestId('create-game-form__game-name');

		expect(gameNameField).toBeInTheDocument();
		expect(gameNameLabel).toHaveTextContent('Name');
	});

	it('contains a submit  button', () => {
		let submit = result.getByTestId('create-game-form__submit');

		expect(submit).toBeInTheDocument();
	});

	async function fillOutForm() {
		let gameNameField = result.getByTestId('create-game-form__game-name');

		await act(() => {
			fireEvent.input(gameNameField, { target: { value: 'test game' } });
		});

		expect(gameNameField).toHaveValue('test game');
	}

	it('filling out the form and clicking submit submits the form with the right values', async () => {
		let mockOnSubmit = vi.fn();
		renderComponent({ onSubmit: mockOnSubmit });

		await fillOutForm();

		let submit = result.getByTestId('create-game-form__submit');

		await fireEvent.click(submit);

		expect(mockOnSubmit).toHaveBeenCalledWith({ gameName: 'test game' });
	});

	it('displays an error summary when there are errors', () => {
		renderComponent({ errors: ['test error'] });

		let errorSummary = result.getByTestId('error-summary');

		expect(errorSummary).toBeInTheDocument();
	});

	it('contains a page title with the correct title', () => {
		let pageTitle = result.getByTestId('page-title');

		expect(pageTitle).toHaveTextContent('Create Game');
	});
});
