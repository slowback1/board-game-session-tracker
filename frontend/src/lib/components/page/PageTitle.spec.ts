import { render, type RenderResult } from '@testing-library/svelte';
import PageTitle from '$lib/components/page/PageTitle.svelte';
import { beforeEach } from 'vitest';

describe('PageTitle', () => {
	let result: RenderResult<PageTitle>;

	function renderComponent(props: Partial<{ title: string }>) {
		if (result) result.unmount();

		result = render(PageTitle, { props });
	}

	beforeEach(() => {
		renderComponent({ title: 'test title' });
	});

	it('renders a page title', () => {
		let title = result.getByTestId('page-title');

		expect(title).toBeInTheDocument();
		expect(title).toHaveTextContent('test title');
	});
});
