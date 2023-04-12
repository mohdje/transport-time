<template>
  <h4 v-if="showMessage" class="err-msg">{{ text }}</h4>
</template>
<script>
export default {
  props: {
    visible: Boolean,
    text: String,
  },
  computed: {
    showMessage() {
      return this.visible;
    },
  },
};
</script>

<style scoped>
.err-msg {
  text-align: center;
  color: red;
  width: 100%;
}
</style>