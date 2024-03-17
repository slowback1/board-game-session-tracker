import { render, type RenderResult } from '@testing-library/svelte';
import LoadingIndicator from '$lib/ui/indicators/LoadingIndicator/LoadingIndicator.svelte';
import { beforeEach } from 'vitest';

describe('LoadingIndicator', () => {
	let result: RenderResult<LoadingIndicator>;

	beforeEach(() => {
		if (result) result.unmount();

		result = render(LoadingIndicator);
	});

	it('renders a loading indicator', () => {
		let indicator = result.getByTestId('loading-indicator');

		expect(indicator).toBeInTheDocument();
	});
});
