import GameApi, { type CreateGameRequest, type GameResponse } from '$lib/api/gameApi';
import { SiteMap } from '$lib/utils/siteMap';
import type { ApiResponse } from '$lib/api/baseApi';

export interface GameConfirmationInfo {
	gameName: string;
	hostPlayerName: string;
	gameId: string;
}

export default class GameService {
	errors: string[] = [];

	constructor(private navigate: (url: string) => void) {}

	async CreateGame(request: CreateGameRequest) {
		let api = new GameApi();

		let response = await api.CreateGame(request);
		this.handleCreateGameResponse(response);
	}

	async GetListOfGames() {
		let api = new GameApi();

		let response = await api.GetGamesForUser();

		if (!!response.errors) this.errors = response.errors;

		return response.response ?? [];
	}

	async AddSelfToGame(gameId: string) {
		let response = await this.HandleAddSelfRequest(gameId);

		if (response.errors) {
			this.errors = response.errors;
			return;
		}

		this.navigate(SiteMap.game.home(gameId));
	}

	private async HandleAddSelfRequest(gameId: string) {
		let api = new GameApi();

		return await api.AddSelfToGame(gameId);
	}

	async GetGameById(gameId: string): Promise<GameConfirmationInfo> {
		let response = await this.FetchGameById(gameId);

		if (response.errors) {
			this.errors = response.errors;
			return null;
		}

		return this.BuildGameConfirmationInfo(response.response);
	}

	private BuildGameConfirmationInfo(game: GameResponse): GameConfirmationInfo {
		return {
			gameName: game.gameName,
			hostPlayerName: game.host.username,
			gameId: game.gameId
		};
	}

	private async FetchGameById(gameId: string) {
		let api = new GameApi();

		return await api.GetGameById(gameId);
	}

	private handleCreateGameResponse(response: ApiResponse<GameResponse>) {
		if (response.errors) this.errors = response.errors;
		else this.navigate(SiteMap.app.home());
	}
}
