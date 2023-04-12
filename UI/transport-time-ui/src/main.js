import App from './App.vue'
import Vue from 'vue'
import vuetify from './plugins/vuetify'
import VueDirectives from "./js/directives";
import { PhoneInterface } from "./js/phoneInterface/phoneInterface";

Vue.config.productionTip = false;

Vue.prototype.$phoneInterface = PhoneInterface;

VueDirectives.import();

new Vue({
  vuetify,
  render: h => h(App)
}).$mount('#app')
