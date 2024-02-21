import type { UserResponse } from '$lib/api/userApi';
import BaseApi, { type ApiResponse } from '$lib/api/baseApi';

export interface CreateGameRequest {
	gameName: string;
}

export interface GameResponse {
	gameId: string;
	gameName: string;
	players: UserResponse[];
	host: UserResponse;
}

export default class GameApi extends BaseApi {
	async CreateGame(request: CreateGameRequest): Promise<ApiResponse<GameResponse>> {
		return await this.Post('/Game', request);
	}

	async GetGamesForUser(): Promise<ApiResponse<GameResponse[]>> {
		return await this.Get('/Game/ForUser');
	}
}
