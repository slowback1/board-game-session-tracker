import { fireEvent, render, type RenderResult } from '@testing-library/svelte';
import FormWrapper from '$lib/ui/containers/formWrapper/FormWrapper.svelte';
import { beforeEach } from 'vitest';

describe('FormWrapper', () => {
	let result: RenderResult<FormWrapper>;

	function renderComponent(props: Partial<{ onSubmit: () => void; errors: string[] }> = {}) {
		if (result) result.unmount();

		result = render(FormWrapper, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	it('renders the form wrapper', () => {
		let wrapper = result.getByTestId('form-wrapper');

		expect(wrapper).toBeInTheDocument();
	});

	it('renders a form', () => {
		let form = result.container.querySelector('form');

		expect(form).toBeInTheDocument();
	});

	it('submitting the form calls the onSubmit prop', () => {
		let mock = vi.fn();

		renderComponent({ onSubmit: mock });

		let form = result.container.querySelector('form');

		fireEvent.submit(form);

		expect(mock).toHaveBeenCalled();
	});

	it('renders an error summary when there are errors', () => {
		renderComponent({ errors: ['test error'] });

		let errorSummary = result.getByTestId('error-summary');

		expect(errorSummary).toBeInTheDocument();
	});
});
