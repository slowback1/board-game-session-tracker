<script lang="ts">
    import {onMount} from "svelte";
    import type {GameResponse} from "$lib/api/gameApi";
    import GameService from "$lib/partials/game/GameService";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import {SiteMap} from "$lib/utils/siteMap";
    import Heading from "$lib/ui/typography/Heading/Heading.svelte";

    const service = new GameService(() => {
    });
    let gameList: GameResponse[] = [];

    onMount(async () => {
        gameList = await service.GetListOfGames();
    })
</script>

<div class="game-list">
    <div class="game-list__header-row">
        <Heading level={2}>Game List</Heading>
        <Button size="small" href={SiteMap.game.create()} testId="game-list__add-game">
            Add Game
        </Button>
    </div>

    {#each gameList as game}
        <div class="game-list__game">
            <p data-testid="game-list__game">
                {game.gameName}
            </p>
            <Button size="small" variant="outline" testId="game-list__game-link"
                    href={SiteMap.game.home(game.gameId)}>Dashboard
            </Button>
        </div>
    {/each}
</div>

<style>
    .game-list {
        display: flex;
        flex-direction: column;
        padding: 16px;
        width: clamp(200px, 33%, 800px);
        border-radius: 10px;
        margin-top: 32px;
        gap: 12px;
        background-color: color-mix(in lab, var(--color-secondary-background) 25%, var(--color-green-baseline));
        border: 2px solid color-mix(in lab, currentColor 10%, var(--color-green-baseline));
    }

    .game-list__header-row {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
    }

    .game-list__game {
        display: flex;
        flex-direction: row;
        gap: 12px;
        align-items: center;
    }


</style>