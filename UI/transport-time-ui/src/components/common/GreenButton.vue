<template>
   <v-btn
      v-delay-click="onClick"
      :color="getAppColor('green-ratp')"
      :elevation="elevation ? 2 : 0"
      :disabled="disabled"
      style="color: white"
      >{{ text }}</v-btn
    >
</template>

<script>
import { colorAppMixin } from "@/js/mixins.js";

export default {
  name: 'GreenButton',
  mixins: [ colorAppMixin ],
  props:{
    text: String,
    elevation: Boolean,
    disabled: Boolean
  },
  methods:{
    onClick(){
      this.$emit("onClick");
    }
  }
}
</script>

<style>

</style>