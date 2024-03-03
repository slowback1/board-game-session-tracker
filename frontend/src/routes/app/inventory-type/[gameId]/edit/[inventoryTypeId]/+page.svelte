<script lang="ts">

    import InventoryTypeService from "$lib/partials/inventoryTypes/InventoryTypeService";
    import {onMount} from "svelte";
    import {goto} from "$app/navigation";
    import type {InventoryTypeOption, InventoryTypeResponse} from "$lib/api/inventoryTypeApi";
    import PageTitle from "$lib/components/page/PageTitle.svelte";
    import InventoryTypeForm from "$lib/partials/inventoryTypes/InventoryTypeForm.svelte";

    export let data: { id: string; gameId: string };

    let errors: string[];
    let service = new InventoryTypeService("", goto);
    let isLoading = true;
    let inventoryType: InventoryTypeResponse = null;

    onMount(async () => {
        service = new InventoryTypeService(data.gameId, goto);

        inventoryType = await service.GetInventoryType(data.id);

        isLoading = false;
    })

    async function onSubmit(value: { name: string, options: InventoryTypeOption[] }) {
        await service.EditInventoryType({...value, inventoryTypeId: data.id});

        errors = service.errors;
    }
</script>

<PageTitle title={`Editing ${inventoryType?.name}`}/>

{#if isLoading}

    <p>Loading...</p>
{:else}
    <InventoryTypeForm
            errors={errors}
            inventoryType={inventoryType}
            onSubmit={onSubmit}
    />
{/if}
