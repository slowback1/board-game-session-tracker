import { getFetchMock, mockApi } from '$lib/testHelpers/getFetchMock';
import UserApi from '$lib/api/UserApi';
import {
	testCreateUserRequest,
	testLoginRequest,
	testLoginResponse,
	testUserResponse
} from '$lib/testHelpers/testData/testUserData';
import { beforeEach, type Mock } from 'vitest';

describe('UserApi', () => {
	let mockFetch: Mock;
	beforeEach(() => {
		mockFetch = mockApi({
			'User/CreateUser': testUserResponse,
			User: testUserResponse,
			'User/Login': testLoginResponse
		});
	});

	let api = new UserApi();
	it('calls the correct url and method to create a user', async () => {
		let result = await api.CreateUser(testCreateUserRequest);

		expect(mockFetch).toHaveBeenCalled();

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain('User/CreateUser');
		expect(options.method).toEqual('POST');
	});

	it('calls the correct url and method to login', async () => {
		let result = await api.Login(testLoginRequest);

		expect(mockFetch).toHaveBeenCalled();

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain('User/Login');
		expect(options.method).toEqual('POST');
	});

	it('calls the correct url and method to get the currently logged in user', async () => {
		let result = await api.GetLoggedInUser();

		expect(mockFetch).toHaveBeenCalled();

		let [url, options] = mockFetch.mock.lastCall;

		expect(url).toContain('User');
		expect(options.method).toEqual('GET');
	});
});
