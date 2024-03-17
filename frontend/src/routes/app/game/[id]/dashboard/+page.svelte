<script lang="ts">
    import InventoryTypeList from "$lib/partials/inventoryTypes/InventoryTypeList.svelte";
    import GameShareLink from "$lib/partials/game/GameShareLink.svelte";
    import PlayerInventory from "$lib/partials/playerInventory/PlayerInventory.svelte";

    export let data: { id: string };

    $:gameId = data.id;
</script>
<GameShareLink gameId={gameId}/>
<InventoryTypeList gameId={gameId}/>
<PlayerInventory gameId={gameId}/>