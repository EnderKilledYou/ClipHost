import 'bootstrap/dist/css/bootstrap.css';
import './app.css';
import 'es6-shim';
import Vue from 'vue';
import { Icon } from '@iconify/vue2';
import App from './App.vue'

import Controls from '@servicestack/vue';



import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'

// Import Bootstrap and BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'


Vue.use(Controls);

Vue.component('Icon', Icon)
Vue.use(BootstrapVue)

import { router } from './shared/router';

Vue.config.productionTip = false;

const app = new Vue({
    el: '#app',
    render: (h) => h(App),
    router,
});
