<script lang="ts">
    import type {GameConfirmationInfo} from "$lib/partials/game/GameService";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import {SiteMap} from "$lib/utils/siteMap";

    export let confirmationInfo: GameConfirmationInfo;
    export let onConfirm: (id: string) => void = () => {
    };

    function handleConfirm() {
        onConfirm(confirmationInfo.gameId);
    }
</script>

<p data-testid="confirm-join__summary">
    Join the game {confirmationInfo.gameName}, hosted by {confirmationInfo.hostPlayerName}?
</p>
<Button testId="confirm-join__confirm" onClick={handleConfirm}>Confirm</Button>
<Button href={SiteMap.app.home()} testId="confirm-join__cancel">Cancel</Button>