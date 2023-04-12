<template>
  <div
    :class="['line-logo-container', { clickable: clickable }]"
    :style="logoStyle"
    v-ripple="clickable"
  >
    <div class="line-logo-text">{{ line ? line.name : "" }}</div>
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
        borderRadius: "100%",
        backgroundColor: "#" + this.line.logoBackgroundColor,
        color: "#" + this.line.logoTextColor,
      };
    },
  }
};
</script>