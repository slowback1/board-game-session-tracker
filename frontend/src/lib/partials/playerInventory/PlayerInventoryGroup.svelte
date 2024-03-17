<script lang="ts">
    import type {PlayerInventoryItemGroup} from "$lib/api/playerInventoryApi";
    import PlayerInventoryItemField from "$lib/partials/playerInventory/PlayerInventoryItemField.svelte";

    export let inventoryGroup: PlayerInventoryItemGroup;
    export let playerId: string;
    export let playerName: string;
    export let onUpdate: (playerId: string, inventoryTypeOptionId: string, amount: number) => void;
</script>

<details>
    <summary data-testid="player-inventory-group__player-name">{playerName}</summary>
    {#each inventoryGroup.items as item}
        <PlayerInventoryItemField
                inventoryItem={item}
                onUpdate={onUpdate}
                playerId={playerId}
        />
    {/each}
</details>
