<script lang="ts">
    import PageTitle from "$lib/components/page/PageTitle.svelte";
    import ErrorSummary from "$lib/components/error/ErrorSummary.svelte";
    import type {CreateUserRequest} from "$lib/api/UserApi";
    import TextBox from "$lib/ui/inputs/TextBox/TextBox.svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";

    export let errors: string[] = [];
    export let onSubmit: (value: CreateUserRequest) => void = () => {
    };

    function handleSubmit() {
        onSubmit({password, confirmPassword, username})
    }

    let username = "";
    let password = "";
    let confirmPassword = "";

</script>
<PageTitle title="Register User"/>

<div class="register-form__wrapper">
    <ErrorSummary
            errors={errors}
    />
    <form class="register-form" on:submit={handleSubmit}>
        <TextBox label="Username" id="register-form__username" bind:value={username}/>
        <TextBox label="Password" id="register-form__password" type="password" bind:value={password}/>
        <TextBox label="Confirm Password" id="register-form__confirmPassword" type="password"
                 bind:value={confirmPassword}/>

        <Button testId="register-form__submit">
            Log In
        </Button>

    </form>
</div>

<style>
    .register-form {
        display: flex;
        flex-direction: column;
        gap: 12px;
        margin-top: 24px;
        background-color: var(--color-card-background);
        color: var(--color-card-font);
        padding: 24px 32px;
    }

    .register-form__wrapper {
        width: clamp(300px, 33vw, 1000px);
        margin-inline: auto;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
</style>