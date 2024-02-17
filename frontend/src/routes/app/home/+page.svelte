<script lang="ts">
    import type {UserResponse} from "$lib/api/UserApi";
    import {onMount} from "svelte";
    import MessageBus from "$lib/bus/MessageBus";
    import {Messages} from "$lib/bus/Messages";
    import PageTitle from "$lib/components/page/PageTitle.svelte";

    let userData: UserResponse;

    onMount(() => {
        MessageBus.subscribe(Messages.UserData, (v) => userData = v);
    })
</script>

<PageTitle title="Welcome"/>

<p>{userData?.username}</p>
<p>share code: {userData?.userId}</p>