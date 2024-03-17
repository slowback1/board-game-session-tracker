<script lang="ts">
    import LoadingIndicator from "$lib/ui/indicators/LoadingIndicator/LoadingIndicator.svelte";
    import PlayerInventoryService from "$lib/partials/playerInventory/PlayerInventoryService.js";
    import {onMount} from "svelte";
    import type {PlayerInventory, PlayerInventoryItemGroup} from "$lib/api/playerInventoryApi";
    import MessageBus from "$lib/bus/MessageBus";
    import {Messages} from "$lib/bus/Messages";
    import PlayerInventoryGroup from "$lib/partials/playerInventory/PlayerInventoryGroup.svelte";

    export let gameId: string;

    let playerInventoryService: PlayerInventoryService;
    let playerInventory: PlayerInventory[] = [];
    let selectedInventoryTypeId: string = "";
    onMount(async () => {
        playerInventoryService = new PlayerInventoryService(gameId);

        MessageBus.subscribe<PlayerInventory[]>(Messages.PlayerInventory, value => {
            playerInventory = value ?? []
            if (playerInventory.length > 0 && selectedInventoryTypeId === "")
                selectedInventoryTypeId = playerInventory[0].inventory[0].inventoryTypeId;

        });
    });

    function getSelectedInventoryType(inventoryTypes: PlayerInventoryItemGroup[]) {
        return inventoryTypes.find(i => i.inventoryTypeId == selectedInventoryTypeId);
    }

    $:isLoaded = !!playerInventory && playerInventory.length > 0;
    $:inventoryTypes = isLoaded ? playerInventory[0].inventory.map(si => ({
        inventoryTypeId: si.inventoryTypeId,
        inventoryTypeName: si.inventoryTypeName,
        items: si.items.map(i => ({inventoryTypeOptionId: i.inventoryTypeOptionId, amount: 0, name: i.name}))
    })) : [];
    $:selectedInventoryType = getSelectedInventoryType(inventoryTypes)
</script>

{#if !isLoaded}
    <LoadingIndicator/>
{:else}
    <select bind:value={selectedInventoryTypeId} data-testid="player-inventory__inventory-type-select">
        {#each inventoryTypes as inventoryType}
            <option value={inventoryType.inventoryTypeId}>
                {inventoryType.inventoryTypeName}
            </option>
        {/each}
    </select>

    <PlayerInventoryGroup
            inventoryGroup={selectedInventoryType}
            playerId=""
            playerName="All Players"
            onUpdate={(playerId, inventoryTypeOptionId , amount) => playerInventoryService.UpdateAllPlayerInventory(inventoryTypeOptionId, amount)}
    />

    {#each playerInventory as player}
        <PlayerInventoryGroup
                playerName={player.playerName}
                playerId={player.playerId}
                inventoryGroup={getSelectedInventoryType(player.inventory)}
                onUpdate={playerInventoryService.UpdatePlayerInventory.bind(playerInventoryService)}
        />
    {/each}
{/if}