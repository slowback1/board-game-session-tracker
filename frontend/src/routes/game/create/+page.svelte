<script lang="ts">
    import GameService from "$lib/partials/game/GameService";
    import {goto} from "$app/navigation";
    import type {CreateUserRequest} from "$lib/api/userApi";
    import type {CreateGameRequest} from "$lib/api/gameApi";
    import CreateGameForm from "$lib/partials/game/CreateGameForm.svelte";

    const service = new GameService(goto);
    let errors: string[] = service.errors;

    async function onSubmit(request: CreateGameRequest) {
        await service.CreateGame(request);
        errors = service.errors;
    }

</script>

<CreateGameForm
        errors={errors}
        onSubmit={onSubmit}
/>