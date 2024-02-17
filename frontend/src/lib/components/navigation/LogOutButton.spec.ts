import { fireEvent, render, type RenderResult, waitFor } from '@testing-library/svelte';
import LogOutButton from '$lib/components/navigation/LogOutButton.svelte';
import { beforeEach, type Mock } from 'vitest';
import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';
import { testUserResponse } from '$lib/testHelpers/testData/testUserData';

describe('LogOutButton', () => {
	let result: RenderResult<LogOutButton>;
	let navigateMock: Mock;

	beforeEach(() => {
		MessageBus.sendMessage(Messages.UserToken, 'token');
		MessageBus.sendMessage(Messages.UserData, testUserResponse);

		navigateMock = vi.fn();
		if (result) result.unmount();
		result = render(LogOutButton, { navigate: navigateMock });
	});

	it('has a log out button', () => {
		let logOutButton = result.getByTestId('log-out');

		expect(logOutButton).toBeInTheDocument();
	});

	it('clicking the log out button logs the user out', async () => {
		let logOutButton = result.getByTestId('log-out');

		await fireEvent.click(logOutButton);

		await waitFor(() => {
			let token = MessageBus.getLastMessage(Messages.UserToken);

			expect(token).toBeNull();
		});
	});
});
