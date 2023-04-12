<template>
  <div class="list-container">
    <SpinnerProgress :visible="isLoading" center />
    <transition name="fade">
      <div v-if="!isLoading">
        <slot></slot>
      </div>
    </transition>
    <div class="empty-list" v-if="isEmptyList">
      <h3>{{ emptyListMessage }}</h3>
    </div>
  </div>
</template>

<script>
import SpinnerProgress from "@/components/common/SpinnerProgress.vue";

export default {
  name: "TransportTimeList",
  components: {
    SpinnerProgress,
  },
  props: {
    isLoading: Boolean,
    emptyListMessage: String,
    isEmptyList: Boolean
  },
};
</script>

<style scoped>
.list-container{
  height: 100%;
  width: 100%;
  background-color: var(--background-grey);
}

.empty-list {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  color: grey;
}
</style>