<script lang="ts">
    import FormWrapper from "$lib/ui/containers/formWrapper/FormWrapper.svelte";
    import TextBox from "$lib/ui/inputs/TextBox/TextBox.svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import type {InventoryTypeOption, InventoryTypeResponse} from "$lib/api/inventoryTypeApi";
    import TrashIcon from "$lib/ui/icons/TrashIcon.svelte";

    export let errors: string[] = [];

    export let inventoryType: InventoryTypeResponse = null;

    let name: string = inventoryType?.name ?? "";
    let options: InventoryTypeOption[] = inventoryType?.options ?? [{label: "", value: ""}];

    export let onSubmit: (value: { name: string, options: InventoryTypeOption[] }) => void = () => {
    };

    function onAddOptionClick(e: Event) {
        e.preventDefault();

        options = [...options, {label: "", value: ""}];
    }

    function onRemoveOptionClick(index: number) {
        options = options.filter((_, i) => i != index);
    }

    function submit() {
        onSubmit({name, options});
    }
</script>

<FormWrapper onSubmit={submit} errors={errors}>
    <TextBox
            label="Name"
            id="inventory-type-form__name"
            bind:value={name}
    />

    <div class="inventory-type-form__option-list">
        {#each options as option, index}
            <div class="inventory-type-form__option">
                <TextBox bind:value={option.label} label="Label" id={`inventory-type-form__option-${index}-label`}/>
                <TextBox bind:value={option.value} label="Value" id={`inventory-type-form__option-${index}-value`}/>
                <Button disabled={options.length === 1 }
                        onClick={() => onRemoveOptionClick(index)}
                        testId={`inventory-type-form__remove-option-${index}`}
                        size="small"
                        variant="text"
                >
                    <TrashIcon/>
                </Button>
            </div>
        {/each}

        <Button size="small"
                variant="primary"
                onClick={onAddOptionClick}
                testId="inventory-type-form__add-option"
        >
            Add Option
        </Button>
    </div>

    <Button testId="inventory-type-form__submit">Submit</Button>

</FormWrapper>

<style global>
    .inventory-type-form__option-list {
        display: flex;
        flex-direction: column;
        align-items: flex-end;
        gap: 8px;
        margin-bottom: 20px;
    }

    .inventory-type-form__option {
        display: flex;
        flex-direction: row;
        align-items: center;
        gap: 12px;
    }

    @media screen and (max-width: 560px) {
        .inventory-type-form__option {
            flex-direction: column;
            border: 1px solid;
            padding: 4px;
        }
    }


    :global(.icon) {
        fill: color-mix(in lab, currentColor, 80% var(--color-error-font));
        width: var(--font-size-small);
        height: var(--font-size-small);
    }
</style>