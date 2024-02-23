export const SiteMap = {
	account: {
		login: () => '/account/login',
		createUser: () => '/account/create'
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
