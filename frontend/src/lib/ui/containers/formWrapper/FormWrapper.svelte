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
        width: clamp(300px, 33vw, 1000px);
        margin-inline: auto;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }

    .form-wrapper__form {
        display: flex;
        flex-direction: column;
        gap: 12px;
        margin-top: 24px;
        background-color: var(--color-card-background);
        color: var(--color-card-font);
        padding: 24px 32px;
    }
</style>