import GameApi, { type CreateGameRequest, type GameResponse } from '$lib/api/gameApi';
import { SiteMap } from '$lib/utils/siteMap';
import type { ApiResponse } from '$lib/api/baseApi';

export default class GameService {
	errors: string[] = [];

	constructor(private navigate: (url: string) => void) {}

	async CreateGame(request: CreateGameRequest) {
		let api = new GameApi();

		let response = await api.CreateGame(request);
		this.handleCreateGameResponse(response);
	}

	private handleCreateGameResponse(response: ApiResponse<GameResponse>) {
		if (response.errors) this.errors = response.errors;
		else this.navigate(SiteMap.app.home());
	}
}
