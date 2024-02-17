import Header from './Header.svelte';
import { act, type RenderResult, waitFor } from '@testing-library/svelte';
import { render } from '@testing-library/svelte';
import { beforeEach } from 'vitest';
import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';
import { testUserResponse } from '$lib/testHelpers/testData/testUserData';

describe('Header', () => {
	let result: RenderResult<Header>;

	function renderComponent() {
		if (result) result.unmount();

		result = render(Header);
	}

	beforeEach(() => {
		renderComponent();
	});

	it('renders the nav', () => {
		let nav = result.getByTestId('header');

		expect(nav).toBeInTheDocument();
	});

	it('contains a skip to content link', () => {
		expect(result.container.querySelector("[href='#content']")).toBeInTheDocument();
	});

	function LogUserIn() {
		MessageBus.sendMessage(Messages.UserData, testUserResponse);
		MessageBus.sendMessage(Messages.UserToken, 'token');
	}

	it('contains a link to the home page when the user is logged in', async () => {
		act(() => {
			LogUserIn();
		});
		await waitFor(() => {
			expect(result.container.querySelector("[href='/']")).toBeInTheDocument();
		});
	});
});
