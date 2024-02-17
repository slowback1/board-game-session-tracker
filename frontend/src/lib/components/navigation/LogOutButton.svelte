<script lang="ts">
    import Button from "$lib/ui/buttons/Button/Button.svelte";
    import UserService from "$lib/services/UserService";

    export let navigate: (url: string) => void = () => {
    };

    const service = new UserService(navigate);

    function onLogOut() {
        service.LogOut();
    }
</script>

<Button size="small" variant="text" testId="log-out" onClick={onLogOut}>Log Out</Button>