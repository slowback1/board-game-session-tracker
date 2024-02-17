import { beforeEach, expect, type Mock } from 'vitest';
import MessageBus from '$lib/bus/MessageBus';
import UserService, { isUserLoggedIn } from '$lib/services/UserService';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import { testLoginResponse, testUserResponse } from '$lib/testHelpers/testData/testUserData';
import { testErrorApiResponse } from '$lib/testHelpers/testData/testApiErrorResponses';
import type { LoginRequest, UserResponse } from '$lib/api/UserApi';
import { Messages } from '$lib/bus/Messages';
import { SiteMap } from '$lib/utils/siteMap';

describe('UserService', () => {
	let service: UserService;
	let mockNavigate: Mock;
	beforeEach(() => {
		MessageBus.clearAll();
		mockNavigate = vi.fn();

		service = new UserService(mockNavigate);
	});

	async function CreateUserWithSuccessfulResult() {
		let mockFetch = getFetchMock(testUserResponse);

		let result = await service.CreateUser('username', 'password', 'password');

		expect(mockFetch).toHaveBeenCalled();
		return mockFetch;
	}

	async function CreateUserWithFailingResult() {
		let mockFetch = getFetchMock(testErrorApiResponse);

		let result = await service.CreateUser('username', 'password', 'password');

		expect(mockFetch).toHaveBeenCalled();
		return mockFetch;
	}

	async function LoginWithSuccessfulResult() {
		let mockFetch = getFetchMock({ response: testLoginResponse });

		let result = await service.Login('username', 'password');

		expect(mockFetch).toHaveBeenCalled();

		return mockFetch;
	}

	async function LoginWithFailingResult() {
		let mockFetch = getFetchMock(testErrorApiResponse);

		let result = await service.Login('username', 'password');

		expect(mockFetch).toHaveBeenCalled();

		return mockFetch;
	}

	describe('creating a user', () => {
		it('can create a user', async () => {
			let mockFetch = await CreateUserWithSuccessfulResult();

			let [url, options] = mockFetch.mock.lastCall;

			expect(url).toContain('User/CreateUser');
			expect(JSON.parse(options.body as string)).toEqual({
				username: 'username',
				password: 'password',
				confirmPassword: 'password'
			});
		});

		it('creating the user calls the redirect mock', async () => {
			await CreateUserWithSuccessfulResult();

			expect(mockNavigate).toHaveBeenCalledWith(SiteMap.account.login());
		});

		it('creating a user with errors exposes the errors onto the service', async () => {
			await CreateUserWithFailingResult();

			expect(service.errors).toEqual(testErrorApiResponse.errors);
		});

		it('creating a user with an error response does not redirect to login', async () => {
			await CreateUserWithFailingResult();

			expect(mockNavigate).not.toHaveBeenCalled();
		});

		it('creating a user with an error response then one without clears the errors', async () => {
			await CreateUserWithFailingResult();
			await CreateUserWithSuccessfulResult();

			expect(service.errors).toEqual([]);
		});
	});

	describe('logging in', () => {
		it('calls the correct URL, method, and body when logging in', async () => {
			let expectedBody: LoginRequest = {
				username: 'username',
				password: 'password'
			};

			let mockFetch = await LoginWithSuccessfulResult();

			let [url, options] = mockFetch.mock.lastCall;

			expect(url).toContain('Login');
			expect(JSON.parse(options.body as string)).toEqual(expectedBody);
			expect(options.method).toEqual('POST');
		});

		it('calls redirect to next page when given a successful response', async () => {
			await LoginWithSuccessfulResult();

			expect(mockNavigate).toHaveBeenCalledWith(SiteMap.home());
		});

		it('does not call redirect when given a failing request', async () => {
			await LoginWithFailingResult();

			expect(mockNavigate).not.toHaveBeenCalled();
		});

		it('sets the errors when responding with a failing request', async () => {
			await LoginWithFailingResult();

			expect(service.errors).toEqual(testErrorApiResponse.errors);
		});

		it('clears the errors when given a failing request then a successful one', async () => {
			await LoginWithFailingResult();

			expect(service.errors).toEqual(testErrorApiResponse.errors);

			await LoginWithSuccessfulResult();

			expect(service.errors).toEqual([]);
		});

		it('sets  the user  token data in the message bus when there is a successful login', async () => {
			await LoginWithSuccessfulResult();

			let token = MessageBus.getLastMessage<string>(Messages.UserToken);

			expect(token).toEqual(testLoginResponse.token);
		});

		it('sets the user data in the message bus when there is a successful login', async () => {
			await LoginWithSuccessfulResult();

			let userData = MessageBus.getLastMessage<UserResponse>(Messages.UserData);

			expect(userData).toEqual(testLoginResponse.user);
		});
	});

	describe('logging out', () => {
		it('clears the user data from the message bus', async () => {
			await LoginWithSuccessfulResult();

			service.LogOut();

			let userData = MessageBus.getLastMessage(Messages.UserData);
			let token = MessageBus.getLastMessage(Messages.UserToken);

			expect(userData).toBeNull();
			expect(token).toBeNull();
		});

		it('redirects the user to the correct url', () => {
			service.LogOut();

			expect(mockNavigate).toHaveBeenCalledWith(SiteMap.home());
		});
	});

	describe('determining if the user is logged in', () => {
		function fillInUserToken() {
			MessageBus.sendMessage(Messages.UserToken, 'token');
		}

		function fillInUserData() {
			MessageBus.sendMessage(Messages.UserData, testUserResponse);
		}

		it('indicates that the user is logged in if both user token and user data are filled out', () => {
			fillInUserToken();
			fillInUserData();

			let isLoggedIn = isUserLoggedIn();

			expect(isLoggedIn).toEqual(true);
		});

		it('indicates that the user is logged out if only the user data is filled in', () => {
			fillInUserData();

			let isLoggedIn = isUserLoggedIn();

			expect(isLoggedIn).toEqual(false);
		});

		it('indicates that the user is logged out if both user data and token is not filled in', () => {
			let isLoggedIn = isUserLoggedIn();

			expect(isLoggedIn).toEqual(false);
		});

		it('indicates that the user is logged out if just the token is filled in', () => {
			fillInUserToken();

			let isLoggedIn = isUserLoggedIn();

			expect(isLoggedIn).toEqual(false);
		});
	});
});
