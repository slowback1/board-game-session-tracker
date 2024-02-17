<script lang="ts">
    export let errors: string[] = [];
    $: hasErrors = !!errors && errors.length > 0;
</script>
{#if hasErrors}
    <div class="error-summary" data-testid="error-summary">
        <ul class="error-summary__list">
            {#each errors as error}
                <li class="error-summary__list-item" data-testid="error-item">
                    {error}
                </li>
            {/each}
        </ul>
    </div>
{/if}

<style>
    .error-summary {
        background-color: var(--color-error-background);
        color: var(--color-error-font);
        padding: 12px 32px;
    }

    .error-summary__list-item {
        font-weight: bold;
    }
</style>