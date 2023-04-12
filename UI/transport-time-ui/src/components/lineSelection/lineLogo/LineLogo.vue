<template>
  <div>
    <div
      :class="{ 'clickable' : clickable}"
      v-delay-click="onClick"
    >
      <metro-logo v-if="line.type === 'metro'" :line="line" :clickable="clickable"/>
      <tram-logo v-if="line.type === 'tram'" :line="line" :clickable="clickable"/>
      <bus-logo v-if="line.type === 'bus'" :line="line" :clickable="clickable"/>
      <train-logo v-if="line.type === 'rail'" :line="line" :clickable="clickable"/>
    </div>
  </div>
</template>

<script>
import MetroLogo from './MetroLogo.vue';
import TramLogo from './TramLogo.vue';
import BusLogo from './BusLogo.vue';
import TrainLogo from './TrainLogo.vue';

export default {
  name: 'LineLogo',
  components: { MetroLogo, TramLogo, BusLogo, TrainLogo },
  props: {
    line: Object,
    clickable: Boolean,
  },
  methods: {
    onClick() {
      this.$emit("logoClick");
    },
  },
};
</script>

<style>
.line-logo-container {
  margin: 5px 10px;
  height: 40px;
  min-width: 40px;
  width: fit-content;
  display: flex;
  justify-content: center;
  align-items: center;
 
}

.line-logo-container.clickable {
  box-shadow: rgba(50, 50, 93, 0.25) 0px 2px 5px -1px,
    rgba(0, 0, 0, 0.3) 0px 1px 3px -1px;
}

.line-logo-text {
  font-size: 22px;
  font-family: LogoFont !important;
}
</style>