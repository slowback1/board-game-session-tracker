import { render, type RenderResult } from '@testing-library/svelte';
import ErrorSummary from '$lib/components/error/ErrorSummary.svelte';
import { beforeEach } from 'vitest';

describe('ErrorSummary', () => {
	let result: RenderResult<ErrorSummary>;

	function renderComponent(errors: string[]) {
		if (result) result.unmount();

		result = render(ErrorSummary, { props: { errors } });
	}

	beforeEach(() => {
		renderComponent(['test error']);
	});

	it('renders a wrapper div to key off of in tests', () => {
		let wrapper = result.getByTestId('error-summary');

		expect(wrapper).toBeInTheDocument();
	});

	it.each([[[]], [null], [undefined]])(
		'does not render a wrapper div if there are no errors (variant: %s)',
		(variant: any) => {
			renderComponent(variant);

			let wrapper = result.queryByTestId('error-summary');

			expect(wrapper).not.toBeInTheDocument();
		}
	);

	it('displays the error message as an error item', () => {
		let errorItem = result.getByTestId('error-item');

		expect(errorItem).toBeInTheDocument();
		expect(errorItem).toHaveTextContent('test error');
	});
});
