<script lang="ts">
    import {isUserLoggedIn} from "$lib/services/UserService";
    import {onMount} from "svelte";
    import {goto} from "$app/navigation";
    import {SiteMap} from "$lib/utils/siteMap";
    import MessageBus from "$lib/bus/MessageBus";

    async function handleHome() {
        //hacky workaround to ensure that the message bus is initialized before checking if the user is logged in
        await new Promise(res => setTimeout(res, 5));

        let isLoggedIn = isUserLoggedIn();

        if (!isLoggedIn)
            await goto(SiteMap.account.login())
    }

    onMount(() => {
        handleHome();
    })
</script>

<h1>Welcome to Svelte Starter Kit</h1>
<p> Now build out your app! </p>
