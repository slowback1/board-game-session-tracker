<script lang="ts">
    import ErrorSummary from "$lib/components/error/ErrorSummary.svelte";

    export let onSubmit: () => void = () => {
    };
    export let errors: string[] = [];
</script>

<div class="form-wrapper" data-testid="form-wrapper">
    <ErrorSummary errors={errors}/>

    <form class="form-wrapper__form" on:submit={onSubmit}>
        <slot/>
    </form>
</div>

<style>
    .form-wrapper {
        margin-inline: auto;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        gap: var(--flex-gap-large);
    }

    .form-wrapper__form {
        --text-box-color__override: color-mix(in lab, var(--color-green-baseline), var(--color-font) 45%);

        display: flex;
        flex-direction: column;
        gap: var(--flex-gap-medium);
        background-color: var(--color-card-background);
        color: var(--color-green-baseline);
        padding: var(--padding-medium) var(--padding-large);
        width: calc(100% - var(--padding-large) * 2);
    }
</style>