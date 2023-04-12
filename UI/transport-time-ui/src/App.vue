<template>
  <v-app style="height: 100% !important; width: 100% !important">
    <v-main style="height: 100% !important; width: 100% !important">
      <Activity />
    </v-main>
  </v-app>
</template>
<script>

import Activity from '@/activities/MainActivity.vue';

export default {
  name: "App",

  components: {
    Activity,
  },
};
</script>
<style src="@/styles/transitions.css"></style>
<style>
html,
body {
    height: 100vh;
    width: 100vw;
    --green-ratp: #00c4b3;
    --blue-ratp: #216eb4;
    --darkblue-ratp: #035096;
    --deepblue-ratp:#0a0082;
    --light-grey: #f1f1f1c7;
    --background-grey: #f7f7f7;
    user-select: none;
}

html,
body,
div,
span {
  font-family: AppFont !important;
  letter-spacing: 0 !important;
}

/*to remove focus on button when clicked*/
.v-btn:not(.v-btn--text):not(.v-btn--outlined):hover:before {
  opacity: 0 !important;
}

.theme--dark.v-btn:hover::before {
  opacity: 0 !important;
}

.v-btn:not(.v-btn--text):not(.v-btn--outlined):focus:before {
  opacity: 0 !important;
}

.v-btn{
  font-weight: 600 !important;
}

/*to remove focus on v-switch when clicked*/
.v-input--selection-controls__ripple:before {
  opacity: 0 !important;
}

/*to remove focus on v-list-item when clicked*/
.theme--light.v-list-item:hover::before {
  opacity: 0 !important;
}

.v-dialog--fullscreen {
  height: 100vh !important;
}

</style>
