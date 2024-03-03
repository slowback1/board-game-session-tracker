<script lang="ts">
    import InventoryTypeService from "$lib/partials/inventoryTypes/InventoryTypeService";
    import {goto} from "$app/navigation";
    import type {InventoryTypeOption} from "$lib/api/inventoryTypeApi";
    import InventoryTypeForm from "$lib/partials/inventoryTypes/InventoryTypeForm.svelte";
    import PageTitle from "$lib/components/page/PageTitle.svelte";

    export let data: { gameId: string };
    let errors: string[] = [];
    let service = new InventoryTypeService(data.gameId, goto);


    async function onSubmit(value: { name: string, options: InventoryTypeOption[] }) {
        await service.CreateInventoryType(value);

        errors = service.errors;
    }

</script>
<PageTitle title="Create Inventory Type"/>
<InventoryTypeForm
        errors={errors}
        onSubmit={onSubmit.bind(this)}
/>