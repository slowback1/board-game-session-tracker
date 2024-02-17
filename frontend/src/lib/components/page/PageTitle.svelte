<script lang="ts">
    export let title: string;
</script>

<h1 data-testid="page-title" class="page-title">{title}</h1>

<style>
    .page-title {
        margin-top: var(--page-title-spacing, 48px);
        margin-bottom: var(--page-title-spacing, 48px);
        font-size: 2rem;
        font-weight: bold;
    }
</style>