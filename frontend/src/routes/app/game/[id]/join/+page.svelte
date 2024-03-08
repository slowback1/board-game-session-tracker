<script lang="ts">
    import {onMount} from "svelte";
    import GameService from "$lib/partials/game/GameService";
    import {goto} from "$app/navigation";
    import ConfirmJoinGame from "$lib/partials/game/ConfirmJoinGame.svelte";
    import PageTitle from "$lib/components/page/PageTitle.svelte";

    export let data: { id: string };
    let service: GameService;
    let isLoading = true;
    let confirmationInfo = null;

    onMount(async () => {
        service = new GameService(goto);

        confirmationInfo = await service.GetGameById(data.id);
        isLoading = false;
    });


    function onConfirm(gameId: string) {
        service.AddSelfToGame(gameId);
    }
</script>
<PageTitle
        title="Confirm Join Game"
/>

{#if isLoading}
    <p>Loading...</p>
{:else}
    <ConfirmJoinGame
            confirmationInfo={confirmationInfo}
            onConfirm={id => onConfirm(id)}
    />
{/if}