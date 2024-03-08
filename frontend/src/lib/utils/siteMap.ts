export const SiteMap = {
	account: {
		login: () => '/account/login',
		createUser: () => '/account/create'
	},
	inventoryTypes: {
		create: (gameId: string) => `/app/inventory-type/${gameId}/create`,
		edit: (gameId: string, inventoryTypeId: string) =>
			`/app/inventory-type/${gameId}/edit/${inventoryTypeId}`
	},
	app: {
		home: () => '/app/home'
	},
	game: {
		create: () => '/app/game/create',
		home: (id: string) => `/app/game/${id}/dashboard`,
		join: (id: string) => `/app/game/${id}/join`
	},
	homeRedirect: () => '/'
};
