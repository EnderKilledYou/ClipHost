<template>
  <div class="row">
    <div class="col-4">
    
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator';
import { store, bus, client, checkAuth, toPascalCase, splitOnFirst } from '../shared';
 
import { Routes, redirect } from '../shared/router';

@Component
export class SignUp extends Vue {

    created() {
        //@ts-ignore
        window.location = "/auth/twitch";
    }

}
export default SignUp;
</script>
