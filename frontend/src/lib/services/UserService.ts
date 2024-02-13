import UserApi, { type UserResponse } from '$lib/api/UserApi';
import type { ApiResponse } from '$lib/api/baseApi';

export default class UserService {
	errors: string[] = [];
	constructor(private goToLogin: () => void) {}

	async CreateUser(username: string, password: string, confirm: string) {
		this.clearErrors();
		let request = this.buildCreateUserRequest(username, confirm, password);

		let api = new UserApi();
		let result = await api.CreateUser(request);

		this.handleResponse(result);
	}

	private buildCreateUserRequest(username: string, confirm: string, password: string) {
		return {
			username: username,
			confirmPassword: confirm,
			password: password
		};
	}

	private handleResponse(result: ApiResponse<UserResponse>) {
		if (result.errors) {
			this.errors = result.errors;
			return;
		}

		this.goToLogin();
	}

	private clearErrors() {
		this.errors = [];
	}
}
