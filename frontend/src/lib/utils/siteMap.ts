export const SiteMap = {
	account: {
		login: () => '/account/login',
		createUser: () => '/account/create'
	},
	inventoryTypes: {
		create: (gameId: string) => `/app/inventory-type/create/${gameId}`,
		edit: (inventoryTypeId: string) => `/app/inventory-type/edit/${inventoryTypeId}`
	},
	app: {
		home: () => '/app/home'
	},
	game: {
		create: () => '/app/game/create',
		home: (id: string) => `/app/game/${id}/dashboard`
	},
	homeRedirect: () => '/'
};
