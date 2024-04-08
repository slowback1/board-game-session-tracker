<script lang="ts">
    import {slugify} from "$lib/utils/stringUtils";

    export let type = "text";
    export let label = "";
    export let onChange = (event: Event) => {
    };
    export let id = slugify(label);
    export let value = "";
    let boundValue = "";

    $: updateBound(value);

    function updateBound(newValue: string) {
        boundValue = newValue;
    }

    const handleInput = e => {
        // in here, you can switch on type and implement
        // whatever behaviour you need
        value = type.match(/^(number|range)$/)
            ? +e.target.value
            : e.target.value;

    };

</script>

<div class="text-box__wrapper">
    <label class="text-box__label" data-testid={id + "-label"} for={id}>
        {label}
    </label>
    <input class="text-box" data-testid={id} {type} value={boundValue} on:input={handleInput} on:change={onChange}
           {id}/>
</div>

<style global>
    .text-box__wrapper {
        --text-box-color: var(--text-box-color__override, var(--color-font));
        --text-box-background: var(--text-box-background__override, transparent);

        display: flex;
        align-items: flex-start;
        flex-direction: column;
        gap: var(--flex-gap-small);
        color: var(--text-box-color);
    }

    .text-box {
        padding: 4px;
        border: 1px solid var(--text-box-color);
        border-radius: 4px;
        background-color: var(--text-box-background);
        color: var(--text-box-color);
    }
</style>