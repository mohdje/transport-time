<template>
  <div
    :class="['line-logo-container', { clickable: clickable }]"
    :style="logoStyle"
    v-ripple="clickable"
  >
    <div class="line-logo-text" :style="textStyle">
      {{ line ? line.name : "" }}
    </div>
  </div>
</template>

<script>
export default {
  props: {
    line: Object,
    clickable: Boolean,
  },
  computed: {
    logoStyle() {
      return {
        borderRadius: "5px",
        backgroundColor: "#" + this.line.logoBackgroundColor,
        color: "#" + this.line.logoTextColor,
      };
    },
    textStyle() {
      return {
        fontSize: this.line.name.length > 5 ? "16px" : "22px",
      };
    },
  },
};
</script>