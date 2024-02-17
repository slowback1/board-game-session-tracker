import { beforeEach, type Mock } from 'vitest';
import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';
import { testUserResponse } from '$lib/testHelpers/testData/testUserData';
import { handleHomeRedirect } from '$lib/utils/navigationUtils';
import { SiteMap } from '$lib/utils/siteMap';

describe('Navigation Utilities', () => {
	beforeEach(() => {
		MessageBus.clearAll();
	});

	function LogUserIn() {
		MessageBus.sendMessage(Messages.UserData, testUserResponse);
		MessageBus.sendMessage(Messages.UserToken, 'token');
	}

	describe('handleHomeRedirect', () => {
		let navigationMock: Mock;

		beforeEach(() => {
			navigationMock = vi.fn();
		});

		it('redirects to the app home page when the user is logged in', async () => {
			LogUserIn();

			await handleHomeRedirect(navigationMock);

			expect(navigationMock).toHaveBeenCalledWith(SiteMap.app.home());
		});

		it("redirects to the login page when the user isn't logged in", async () => {
			await handleHomeRedirect(navigationMock);

			expect(navigationMock).toHaveBeenCalledWith(SiteMap.account.login());
		});
	});
});
