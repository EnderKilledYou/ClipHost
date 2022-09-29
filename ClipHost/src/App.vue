<template>
    <div>
        <b-container>

            <b-navbar toggleable="lg" type="dark" variant="info">
                <b-navbar-brand href="/">Home</b-navbar-brand>


                <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
                <b-collapse id="nav-collapse" is-nav>
                    <b-navbar-nav>
                        <b-nav-item-dropdown text="Create" right>
                            <b-dropdown-item v-for="item in navCreateItems" :href="item.href">{{item.label}}</b-dropdown-item>

                        </b-nav-item-dropdown>
                        <b-nav-item-dropdown text="List" right>
                            <b-dropdown-item v-for="item in navListItems" :href="item.href">{{item.label}}</b-dropdown-item>

                        </b-nav-item-dropdown>
                        <b-nav-form class="ml-auto">
                            <b-button size="sm" class="mr-sm-2" placeholder="Login" @click="Login">Login</b-button>
                        </b-nav-form>
                    </b-navbar-nav>

                </b-collapse>

            </b-navbar>
        </b-container>


        <b-container fluid>


            <router-view></router-view>
        </b-container>
    </div>

</template>

<script lang="ts">
    import Vue from 'vue';
    import { Component, Prop } from 'vue-property-decorator';
    import { bus, store } from './shared';
    import { routes } from "@/shared/router";

    const navItems = [
        { "href": "/", "label": "Home", "exact": true },
        { "href": "/signin", "label": "Sign In", "hide": "auth" },
    ]

    @Component
    export class App extends Vue {
        get store() {
            return store
        }
        get navCreateItems() {
            return routes.filter(a => {
                return a.path.toLowerCase().startsWith("/create")
            }).map(a => {
                return {
                    "href": a.path,
                    "label": a.path.substring(7)
                }
            })
        }
        get navListItems() {
            return routes.filter(a => {
                return a.path.toLowerCase().startsWith("/list")
            }).map(a => {
                return {
                    "href": a.path,
                    "label": a.path.substring(5)
                }
            })
        }
        Login() {
            //@ts-ignore
            window.location = "/auth/twitch"
        }
        get Routes() {
            return routes;
        }
    }

    export default App
</script>
