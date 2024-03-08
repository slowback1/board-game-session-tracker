<script lang="ts">
    import InventoryTypeList from "$lib/partials/inventoryTypes/InventoryTypeList.svelte";
    import GameShareLink from "$lib/partials/game/GameShareLink.svelte";

    export let data: { id: string };

</script>
<GameShareLink gameId={data.id}/>
<InventoryTypeList gameId={data.id}/>