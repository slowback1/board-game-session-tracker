<script lang="ts">
    import PageTitle from "$lib/components/page/PageTitle.svelte";
    import type {CreateUserRequest} from "$lib/api/userApi";
    import TextBox from "$lib/ui/inputs/TextBox/TextBox.svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import FormWrapper from "$lib/ui/containers/formWrapper/FormWrapper.svelte";

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

<FormWrapper onSubmit={handleSubmit} errors={errors}>
    <TextBox label="Username" id="register-form__username" bind:value={username}/>
    <TextBox label="Password" id="register-form__password" type="password" bind:value={password}/>
    <TextBox label="Confirm Password" id="register-form__confirmPassword" type="password"
             bind:value={confirmPassword}/>

    <Button testId="register-form__submit">
        Register
    </Button>
</FormWrapper>