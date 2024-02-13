import BaseApi, { type ApiResponse } from '$lib/api/baseApi';

export interface CreateUserRequest {
	username: string;
	password: string;
	confirmPassword: string;
}

export interface UserResponse {
	username: string;
	userId: string;
	createdAt: string;
}

export interface LoginRequest {
	username: string;
	password: string;
}

export interface LoginResponse {
	token: string;
	user: UserResponse;
}

export default class UserApi extends BaseApi {
	async CreateUser(request: CreateUserRequest): Promise<ApiResponse<UserResponse>> {
		return this.Post('User/CreateUser', request);
	}

	async Login(request: LoginRequest): Promise<ApiResponse<LoginResponse>> {
		return this.Post('User/Login', request);
	}

	async GetLoggedInUser(): Promise<ApiResponse<UserResponse>> {
		return this.Get('User');
	}
}
