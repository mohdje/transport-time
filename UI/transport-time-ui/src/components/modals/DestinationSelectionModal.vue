<template>
  <ModalDialog :visible="visible" @outsideClick="onOutsideClick">
    <div :key="renderKey" class="selection-modal-container">
      <h3>Choisissez une destination</h3>
      <BlueButton small text="Sans importance" @click="onNoDestinationClick" />
      <div class="space"></div>
      <StopAreaSearchInput  @stopAreaSelected="onStopAreaSelected" />
    </div>
  </ModalDialog>
</template>

<script>
import ModalDialog from "@/components/common/ModalDialog.vue";
import StopAreaSearchInput from "@/components/common/StopAreaSearchInput.vue";
import BlueButton from "@/components/common/BlueButton.vue";

export default {
  components: {
    ModalDialog,
    StopAreaSearchInput,
    BlueButton,
  },
  props: {
    visible: Boolean,
  },
  data() {
    return {
      renderKey: 0,
      searchInProgress: false,
    };
  },

  methods: {
    onOutsideClick() {
      this.$emit("outsideClick");
    },
    onStopAreaSelected(stopArea) {
      this.$emit("destinationSelected", stopArea);
    },
    onNoDestinationClick() {
        this.renderKey++;
      this.$emit("destinationSelected");
    },
    onSearchTextChanged() {},
  },
};
</script>

<style scoped>
.selection-modal-container {
  padding: 10px 15px 30px 15px;
  text-align: center;
  position: relative;
  height: 200px;
}

.space{
  height: 20px;
}
</style>