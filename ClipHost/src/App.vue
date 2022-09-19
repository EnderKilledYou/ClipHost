<template>
  <div>
    <nav class="navbar navbar-expand-lg navbar-dark">
      <div class="container">
        <router-link class="navbar-brand" to="/" exact>
          <span class="align-middle">ClipHost</span>
        </router-link>
        <navbar :items="navItems" :attributes="store.userAttributes"/>
      </div>
    </nav>

    <div id="content" class="container mt-4">
      <router-view></router-view>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import {Component, Prop} from 'vue-property-decorator';
import {bus, store} from './shared';
import {routes} from "@/shared/router";

const navItems = [
  {"href": "/", "label": "Home", "exact": true},
  {"href": "/about", "label": "About"},
  {"href": "/signin", "label": "Sign In", "hide": "auth"},
  {"href": "/profile", "label": "Profile", "show": "auth"},
  {"href": "/admin", "label": "Admin", "show": "role:Admin"}
]

@Component
export class App extends Vue {
  get store() {
    return store
  }

  get navItems() {
      return routes.filter(a => {
          return a.path.toLowerCase().startsWith("/create") || a.path.toLowerCase().startsWith("/list")
      }).map(a => {
      return {
        "href": a.path,
        "label": a.path.substring(1)
      }
    })
  }

  get Routes() {
    return routes;
  }
}

export default App
</script>
