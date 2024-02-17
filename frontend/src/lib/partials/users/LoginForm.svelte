<script lang="ts">
    import TextBox from "$lib/ui/inputs/TextBox/TextBox.svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import ErrorSummary from "$lib/components/error/ErrorSummary.svelte";
    import PageTitle from "$lib/components/page/PageTitle.svelte";
    import {SiteMap} from "$lib/utils/siteMap";

    export let onSubmit: (username: string, password: string) => void = () => {
    };
    export let errors: string[] = [];

    function handleSubmit() {
        onSubmit(username, password);
    }

    let username = "";
    let password = "";
</script>

<PageTitle title="Log In"/>

<div class="login-form__wrapper">
    <ErrorSummary
            errors={errors}
    />
    <form class="login-form" on:submit={handleSubmit}>
        <TextBox label="Username" id="login-form__username" bind:value={username}/>
        <TextBox label="Password" id="login-form__password" type="password" bind:value={password}/>

        <Button testId="login-form__submit">
            Log In
        </Button>
        <Button testId="login-form__register-link" href={SiteMap.account.createUser()}>
            New User
        </Button>
    </form>
</div>

<style>
    .login-form {
        display: flex;
        flex-direction: column;
        gap: 12px;
        margin-top: 24px;
        background-color: var(--color-card-background);
        color: var(--color-card-font);
        padding: 24px 32px;

    }

    .login-form__wrapper {
        width: clamp(300px, 33vw, 1000px);
        margin-inline: auto;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
</style>