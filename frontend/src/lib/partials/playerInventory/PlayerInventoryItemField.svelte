<script lang="ts">
	import type { PlayerInventoryItem } from '$lib/api/playerInventoryApi';
	import Button from '$lib/ui/buttons/Button/Button.svelte';

	export let inventoryItem: PlayerInventoryItem;
	export let playerId: string;
	export let onUpdate: (playerId: string, inventoryTypeOptionId: string, amount: number) => void;

	function onIncrement() {
		onUpdate(playerId, inventoryItem.inventoryTypeOptionId, inventoryItem.amount + 1);
	}

	function onDecrement() {
		onUpdate(playerId, inventoryItem.inventoryTypeOptionId, inventoryItem.amount - 1);
	}
</script>
<div class="player-inventory-item">

	<p data-testid="player-inventory-item__name">{inventoryItem.name}</p>

	<Button onClick={onDecrement} size="small" testId="player-inventory-item__decrement">
		-
	</Button>
	<p data-testid="player-inventory-item__amount">{inventoryItem.amount}</p>

	<Button onClick={onIncrement} size="small" testId="player-inventory-item__increment">
		+
	</Button>
</div>

<style>
    .player-inventory-item {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        text-align: center;
        align-content: center;
        align-items: center;
        margin-top: 12px;
    }


</style>