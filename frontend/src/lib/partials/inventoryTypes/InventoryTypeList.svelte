<script lang="ts">
    import InventoryTypeApi, {type InventoryTypeResponse} from "$lib/api/inventoryTypeApi";
    import {onMount} from "svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import {SiteMap} from "$lib/utils/siteMap";
    import Heading from "$lib/ui/typography/Heading/Heading.svelte";

    let inventoryTypes: InventoryTypeResponse[] = [];
    export let gameId: string = "";


    onMount(async () => {
        let response = await new InventoryTypeApi().GetInventoryTypesForGame(gameId);

        inventoryTypes = response.response;
    })
</script>

<div class="inventory-type-list">

    <div class="inventory-type-list-header-row">
        <Heading level="2">Inventory Types</Heading>
        <Button variant="outline" size="small" href={SiteMap.inventoryTypes.create(gameId)}
                testId="inventory-type-list__add">
            Add Inventory Type
        </Button>
    </div>

    <table class="inventory-type-list__table">
        <thead>
        <tr>
            <th>Name</th>
            <th># of Options</th>
            <th></th>
        </tr>
        </thead>
        <tbody>
        {#each inventoryTypes as inventoryType}
            <tr data-testid="inventory-type-list__row">
                <td data-testid="inventory-type-list__row-name">
                    {inventoryType.name}
                </td>
                <td data-testid="inventory-type-list__row-options-count">
                    {inventoryType.options.length}
                </td>
                <td>
                    <Button size="small" variant="text" testId="inventory-type-list__row-edit"
                            href={SiteMap.inventoryTypes.edit(gameId, inventoryType.inventoryTypeId)}>
                        Edit
                    </Button>
                </td>
            </tr>
        {/each}
        </tbody>
    </table>
</div>

<style>
    .inventory-type-list {
        width: clamp(300px, 33vw, 1000px);
        background-color: color-mix(in lab, var(--color-background) 40%, var(--color-blue-baseline));
        padding: 16px 12px;
        border-radius: 12px;
    }

    .inventory-type-list-header-row {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-items: center;
    }

    .inventory-type-list__table {
        width: 100%;
        margin-top: 32px;
    }

    td, th {
        border: 1px solid;
        padding: 12px;
    }
</style>