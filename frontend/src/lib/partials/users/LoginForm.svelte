<script lang="ts">
    import TextBox from "$lib/ui/inputs/TextBox/TextBox.svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import PageTitle from "$lib/components/page/PageTitle.svelte";
    import {SiteMap} from "$lib/utils/siteMap";
    import FormWrapper from "$lib/ui/containers/formWrapper/FormWrapper.svelte";

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

<FormWrapper onSubmit={handleSubmit} errors={errors}>
    <TextBox label="Username" id="login-form__username" bind:value={username}/>
    <TextBox label="Password" id="login-form__password" type="password" bind:value={password}/>

    <Button testId="login-form__submit">
        Log In
    </Button>
    <Button testId="login-form__register-link" href={SiteMap.account.createUser()}>
        New User
    </Button>
</FormWrapper>
