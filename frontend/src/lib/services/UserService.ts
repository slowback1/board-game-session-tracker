import UserApi, {
	type LoginRequest,
	type LoginResponse,
	type UserResponse
} from '$lib/api/UserApi';
import type { ApiResponse } from '$lib/api/baseApi';
import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';
import { SiteMap } from '$lib/utils/siteMap';

export default class UserService {
	errors: string[] = [];

	constructor(private navigateToPage: (url: string) => void) {}

	async CreateUser(username: string, password: string, confirm: string) {
		this.clearErrors();
		let result = await this.makeCreateUserRequest(username, confirm, password);

		this.handleCreateUserResponse(result);
	}

	async Login(username: string, password: string): Promise<void> {
		this.clearErrors();

		let result = await this.makeLoginRequest(username, password);
		this.handleLoginResponse(result);
	}

	LogOut() {
		this.clearUserData();

		this.navigateToPage(SiteMap.home());
	}

	private clearUserData() {
		MessageBus.clear(Messages.UserToken);
		MessageBus.clear(Messages.UserData);
	}

	private handleLoginResponse(result: ApiResponse<LoginResponse>) {
		if (result.errors) {
			this.handleErrorResponse(result);
			return;
		}

		this.saveUserDataToMessageBus(result);

		this.navigateToPage(SiteMap.home());
	}

	private handleErrorResponse(result: ApiResponse<any>) {
		this.errors = result.errors;
	}

	private saveUserDataToMessageBus(result: ApiResponse<LoginResponse>) {
		MessageBus.sendMessage(Messages.UserToken, result.response.token);
		MessageBus.sendMessage(Messages.UserData, result.response.user);
	}

	private async makeLoginRequest(username: string, password: string) {
		let request: LoginRequest = { username, password };

		let api = new UserApi();
		return await api.Login(request);
	}

	private async makeCreateUserRequest(username: string, confirm: string, password: string) {
		let request = this.buildCreateUserRequest(username, confirm, password);

		let api = new UserApi();
		return await api.CreateUser(request);
	}

	private buildCreateUserRequest(username: string, confirm: string, password: string) {
		return {
			username: username,
			confirmPassword: confirm,
			password: password
		};
	}

	private handleCreateUserResponse(result: ApiResponse<UserResponse>) {
		if (result.errors) {
			this.handleErrorResponse(result);
			return;
		}

		this.navigateToPage(SiteMap.account.login());
	}

	private clearErrors() {
		this.errors = [];
	}
}

export function isUserLoggedIn() {
	let token = MessageBus.getLastMessage<string>(Messages.UserToken);
	let userData = MessageBus.getLastMessage<UserResponse>(Messages.UserData);

	let hasToken = !!token;
	let hasUserData = !!userData;

	return hasToken && hasUserData;
}
