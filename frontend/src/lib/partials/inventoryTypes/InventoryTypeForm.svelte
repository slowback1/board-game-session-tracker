<script lang="ts">
    import FormWrapper from "$lib/ui/containers/formWrapper/FormWrapper.svelte";
    import TextBox from "$lib/ui/inputs/TextBox/TextBox.svelte";
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import type {InventoryTypeOption, InventoryTypeResponse} from "$lib/api/inventoryTypeApi";

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

    {#each options as option, index}
        <TextBox bind:value={option.label} label="Label" id={`inventory-type-form__option-${index}-label`}/>
        <TextBox bind:value={option.value} label="Value" id={`inventory-type-form__option-${index}-value`}/>
        <Button disabled={options.length === 1 }
                onClick={() => onRemoveOptionClick(index)}
                testId={`inventory-type-form__remove-option-${index}`}>
            Remove Option
        </Button>
    {/each}

    <Button onClick={onAddOptionClick} testId="inventory-type-form__add-option">Add Option</Button>

    <Button testId="inventory-type-form__submit">Submit</Button>

</FormWrapper>