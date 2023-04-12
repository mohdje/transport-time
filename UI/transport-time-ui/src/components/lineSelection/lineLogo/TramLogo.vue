<template>
  <div
    :class="['line-logo-container', { clickable: clickable }]"
    :style="logoStyle"
    v-ripple="clickable"
  >
    <div class="line-logo-text">
      {{ line ? line.name.toLowerCase().replace("t", "") : "" }}
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
        borderRadius: "100%",
        border: "3px solid #" + this.line.logoBackgroundColor,
        backgroundColor: "white",
        color: "#" + this.line.logoBackgroundColor,
      };
    },
  },
};
</script>