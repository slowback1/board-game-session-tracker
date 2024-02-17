import { isUserLoggedIn } from '$lib/services/UserService';
import { SiteMap } from '$lib/utils/siteMap';

export async function handleHomeRedirect(navigate: (url: string) => Promise<void>) {
	//hacky workaround to ensure that the message bus is initialized before checking if the user is logged in
	await new Promise((res) => setTimeout(res, 5));

	let isLoggedIn = isUserLoggedIn();

	if (!isLoggedIn) await navigate(SiteMap.account.login());
	else await navigate(SiteMap.app.home());
}
