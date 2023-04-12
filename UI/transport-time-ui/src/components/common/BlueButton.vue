<template>
  <v-btn
    :large="large"
    :small="small"
    :style="btnStyle"
    :disabled="disable"
    rounded
    :color="getAppColor('blue-ratp')"
    v-delay-click="onClick"
    >{{ text }}</v-btn
  >
</template>

<script>
import { colorAppMixin } from "@/js/mixins";

export default {
  props: {
    disable: Boolean,
    text: String,
    large: Boolean,
    small: Boolean,
  },
  mixins: [colorAppMixin],
  data() {
    return {
      btnStyle: {
        textTransform: "none",
        color: "white",
        margin: "7px auto",
        fontSize: "18px",
        fontWeight: "600",
        padding: "5px 30px",
      },
    };
  },
  methods: {
    onClick(){
        this.$emit('click');
    }
  },
};
</script>

<style>
</style>