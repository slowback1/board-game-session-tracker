<script>
    import SkipToContentLink from '$lib/components/navigation/SkipToContentLink.svelte';
    import HeaderLink from '$lib/components/navigation/HeaderLink.svelte';
    import ThemeToggle from '$lib/components/navigation/ThemeToggle.svelte';
    import {onMount} from "svelte";
    import MessageBus from "$lib/bus/MessageBus";
    import {isUserLoggedIn} from "$lib/services/UserService";
    import {Messages} from "$lib/bus/Messages";
    import LogOutButton from "$lib/components/navigation/LogOutButton.svelte";
    import {goto} from "$app/navigation";

    let userIsLoggedIn = false;

    function updateUserIsLoggedIn() {
        userIsLoggedIn = isUserLoggedIn();
    }

    onMount(() => {
        MessageBus.subscribe(Messages.UserToken, updateUserIsLoggedIn)
        MessageBus.subscribe(Messages.UserData, updateUserIsLoggedIn)
    })
</script>

<nav data-testid="header">
    <SkipToContentLink/>

    <ThemeToggle/>

    {#if userIsLoggedIn}
        <HeaderLink href="/" label="Home"/>
        <LogOutButton navigate={goto}/>
    {/if}
</nav>

<style>
    nav {
        display: flex;
        flex-direction: row;
        justify-content: flex-end;
        align-items: center;
        margin: 0;
        padding-inline: var(--gutters-x);
        background-color: color-mix(in lab, var(--color-green-baseline), var(--color-background) 30%);
        color: color-mix(in lab, var(--color-green-baseline), var(--color-font) 55%);
        height: var(--header-height);
        gap: 12px;
    }
</style>
