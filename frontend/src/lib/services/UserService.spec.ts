import { beforeEach, type Mock } from 'vitest';
import MessageBus from '$lib/bus/MessageBus';
import UserService from '$lib/services/UserService';
import { getFetchMock } from '$lib/testHelpers/getFetchMock';
import { testUserResponse } from '$lib/testHelpers/testData/testUserData';
import { testErrorApiResponse } from '$lib/testHelpers/testData/testApiErrorResponses';

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

		expect(mockNavigate).toHaveBeenCalled();
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
