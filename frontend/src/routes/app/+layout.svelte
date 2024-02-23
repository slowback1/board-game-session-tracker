<script lang="ts">
    import {onMount} from "svelte";
    import {isUserLoggedIn} from "$lib/services/UserService";
    import {handleHomeRedirect} from "$lib/utils/navigationUtils";
    import {goto} from "$app/navigation";

    onMount(() => {
        if (!isUserLoggedIn())
            handleHomeRedirect(goto)
    })
</script>

<slot/>