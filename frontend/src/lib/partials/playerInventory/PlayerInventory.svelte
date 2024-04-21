<script lang="ts">
	import LoadingIndicator from '$lib/ui/indicators/LoadingIndicator/LoadingIndicator.svelte';
	import PlayerInventoryService from '$lib/partials/playerInventory/PlayerInventoryService.js';
	import { onMount } from 'svelte';
	import type { PlayerInventory, PlayerInventoryItemGroup } from '$lib/api/playerInventoryApi';
	import MessageBus from '$lib/bus/MessageBus';
	import { Messages } from '$lib/bus/Messages';
	import PlayerInventoryGroup from '$lib/partials/playerInventory/PlayerInventoryGroup.svelte';
	import Heading from '$lib/ui/typography/Heading/Heading.svelte';

	export let gameId: string;

	let playerInventoryService: PlayerInventoryService;
	let playerInventory: PlayerInventory[] = [];
	let selectedInventoryTypeId: string = '';
	onMount(async () => {
		playerInventoryService = new PlayerInventoryService(gameId);

		MessageBus.subscribe<PlayerInventory[]>(Messages.PlayerInventory, value => {
			playerInventory = value ?? [];
			if (playerInventory.length > 0 && selectedInventoryTypeId === '' && playerInventory[0].inventory.length > 0)
				selectedInventoryTypeId = playerInventory[0].inventory[0].inventoryTypeId;

		});
	});

	function getSelectedInventoryType(inventoryTypes: PlayerInventoryItemGroup[]) {
		return inventoryTypes.find(i => i.inventoryTypeId == selectedInventoryTypeId);
	}

	$:isLoaded = !!playerInventory && playerInventory.length > 0;
	$:hasInventoryTypes = isLoaded && playerInventory[0].inventory.length > 0;
	$:inventoryTypes = isLoaded ? playerInventory[0].inventory.map(si => ({
		inventoryTypeId: si.inventoryTypeId,
		inventoryTypeName: si.inventoryTypeName,
		items: si.items.map(i => ({ inventoryTypeOptionId: i.inventoryTypeOptionId, amount: 0, name: i.name }))
	})) : [];
	$:selectedInventoryType = getSelectedInventoryType(inventoryTypes);
</script>

<div class="player-inventory">
	<div class="player-inventory__header-row">
		<Heading level="2">Player Inventory</Heading>

		{#if isLoaded && hasInventoryTypes}
			<div>
				<label for="player-inventory__inventory-type-select">Select Inventory Type:</label>
				<select id="player-inventory__inventory-type-select" bind:value={selectedInventoryTypeId}
								data-testid="player-inventory__inventory-type-select">
					{#each inventoryTypes as inventoryType}
						<option value={inventoryType.inventoryTypeId}>
							{inventoryType.inventoryTypeName}
						</option>
					{/each}
				</select>
			</div>
		{/if}
	</div>

	{#if !isLoaded}
		<LoadingIndicator />
	{:else if !hasInventoryTypes}
		<p data-testid="player-inventory__add-inventory-type-message">
			No inventory types found. Please add an inventory type to get started.
		</p>
	{:else}

		<div class="player-inventory__groups">
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
		</div>
	{/if}
</div>

<style>
    .player-inventory {
        background-color: color-mix(in lab, var(--color-background) 40%, var(--color-blue-baseline));
        border-radius: 12px;
        padding: var(--padding-medium);
    }

    .player-inventory__header-row {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-items: center;
        margin-bottom: var(--padding-large);
        flex-wrap: wrap;
        gap: var(--flex-gap-large);
    }

    .player-inventory__groups {
        display: flex;
        flex-direction: column;
        gap: var(--flex-gap-large);
    }

</style>