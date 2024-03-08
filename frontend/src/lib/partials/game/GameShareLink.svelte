<script lang="ts">
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import {SiteMap} from "$lib/utils/siteMap";

    export let gameId: string;

    let hasCopied = false;

    function onClick() {
        let url = window.location.host + SiteMap.game.join(gameId);

        navigator.clipboard.writeText(url);

        hasCopied = true;

        setTimeout(() => {
            hasCopied = false;
        }, 5000)
    }
</script>

<Button onClick={onClick} testId="game-share-link">
    {hasCopied ? "Link Copied!" : "Copy Share Link"}
</Button>