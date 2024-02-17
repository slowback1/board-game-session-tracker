<script lang="ts">
    import {onMount} from "svelte";
    import {goto} from "$app/navigation";
    import {handleHomeRedirect} from "$lib/utils/navigationUtils";

    onMount(() => {
        handleHomeRedirect(goto);
    })
</script>

<p>Redirecting you...</p>
