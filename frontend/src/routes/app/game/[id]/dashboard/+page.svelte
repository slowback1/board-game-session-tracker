<script lang="ts">
	import InventoryTypeList from '$lib/partials/inventoryTypes/InventoryTypeList.svelte';
	import GameShareLink from '$lib/partials/game/GameShareLink.svelte';
	import PlayerInventory from '$lib/partials/playerInventory/PlayerInventory.svelte';
	import PageTitle from '$lib/components/page/PageTitle.svelte';

	export let data: { id: string };

	$:gameId = data.id;
</script>
<PageTitle title="Dashboard" />
<div class="game-dashboard">
	<div class="game-dashboard__share-link-wrapper">
		<GameShareLink gameId={gameId} />
	</div>
	<div class="game-dashboard__control-wrapper">
		<InventoryTypeList gameId={gameId} />
		<PlayerInventory gameId={gameId} />
	</div>
</div>

<style>
    .game-dashboard {
        display: flex;
        flex-direction: column;
        gap: var(--flex-gap-large);
    }

    .game-dashboard__control-wrapper {
        display: grid;
        gap: var(--flex-gap-large);
        width: 100%;
        grid-template-columns: 2fr 5fr;
    }

    @media screen and (max-width: 950px) {
        .game-dashboard__control-wrapper {
            grid-template-columns: 1fr;
        }
    }
</style>
