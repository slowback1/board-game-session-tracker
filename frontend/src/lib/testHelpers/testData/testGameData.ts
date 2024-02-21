import type { CreateGameRequest, GameResponse } from '$lib/api/gameApi';
import { testUserResponse } from '$lib/testHelpers/testData/testUserData';

export let testCreateGameRequest: CreateGameRequest = {
	gameName: 'test game'
};

export let testGameResponse: GameResponse = {
	gameName: 'test game',
	gameId: '1234',
	host: testUserResponse,
	players: [testUserResponse]
};
