<script lang="ts">
    import UserService from "$lib/services/UserService";
    import {goto} from "$app/navigation";
    import LoginForm from "$lib/partials/users/LoginForm.svelte";

    const service = new UserService(goto);

    let errors: string[] = [];

    async function onSubmit(u: string, p: string) {
        await service.Login(u, p);
        errors = service.errors;
    }
</script>

<LoginForm
        onSubmit={onSubmit}
        errors={errors}
/>