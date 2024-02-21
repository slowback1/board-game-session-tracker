<script lang="ts">
    import UserService from "$lib/services/UserService";
    import {goto} from "$app/navigation";
    import type {CreateUserRequest} from "$lib/api/userApi";
    import RegisterForm from "$lib/partials/users/RegisterForm.svelte";

    const service = new UserService(goto);

    let errors: string[] = service.errors;

    async function onSubmit(request: CreateUserRequest) {
        await service.CreateUser(request.username, request.password, request.confirmPassword);
        errors = service.errors;
    }
</script>

<RegisterForm
        errors={errors}
        onSubmit={onSubmit}
/>