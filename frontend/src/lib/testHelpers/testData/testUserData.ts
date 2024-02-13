import type {
	CreateUserRequest,
	LoginRequest,
	LoginResponse,
	UserResponse
} from '$lib/api/UserApi';

export const testCreateUserRequest: CreateUserRequest = {
	password: 'password',
	confirmPassword: 'password',
	username: 'user'
};

export const testUserResponse: UserResponse = {
	userId: '1234',
	createdAt: new Date().toISOString(),
	username: 'user'
};

export const testLoginRequest: LoginRequest = {
	password: 'password',
	username: 'user'
};

export const testLoginResponse: LoginResponse = {
	token: 'token',
	user: testUserResponse
};
