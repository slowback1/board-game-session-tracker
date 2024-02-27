<script lang="ts">
    import InventoryTypeList from "$lib/partials/inventoryTypes/InventoryTypeList.svelte";

    export let data: { id: string };

</script>

<InventoryTypeList gameId={data.id}/>