<script lang="ts">
    import TextBox from "$lib/ui/inputs/TextBox/TextBox.svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import type {CreateGameRequest} from "$lib/api/gameApi";
    import PageTitle from "$lib/components/page/PageTitle.svelte";
    import FormWrapper from "$lib/ui/containers/formWrapper/FormWrapper.svelte";

    export let onSubmit: (value: CreateGameRequest) => void = () => {
    }
    export let errors: string[] = [];

    let gameName: string = "";

    function handleSubmit() {
        onSubmit({gameName})
    }

</script>
<PageTitle title="Create Game"/>

<FormWrapper onSubmit={handleSubmit} errors={errors}>
    <TextBox
            id="create-game-form__game-name"
            label="Name"
            bind:value={gameName}
    />

    <Button testId="create-game-form__submit">Submit</Button>
</FormWrapper>
