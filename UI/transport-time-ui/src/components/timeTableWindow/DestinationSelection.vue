<template>
  <div>
    <div class="destination-selection">
      <h3>Destination:</h3>
      <v-btn
        elevation="2"
        rounded
        :color="getAppColor('green-ratp')"
        style="color: white"
        v-delay-click="() => (showDestinationSelectionModal = true)"
        width="50%"
      >
        {{ selectedDestinationName }}
      </v-btn>
    </div>
    <DestinationSelectionModal
      :visible="showDestinationSelectionModal"
      @outsideClick="() => (showDestinationSelectionModal = false)"
      @destinationSelected="onDestinationSelected"
    />
  </div>
</template>

<script>
import DestinationSelectionModal from "@/components/modals/DestinationSelectionModal.vue";
import { colorAppMixin } from "@/js/mixins";

export default {
  components: {
    DestinationSelectionModal,
  },
  mixins: [colorAppMixin],
  data() {
    return {
      selectedDestinationName: "Sans importance",
      defaultDestinationName: "Sans importance",
      showDestinationSelectionModal: false
    };
  },
  methods: {
    onDestinationSelected(destination) {
      this.selectedDestinationName = destination
        ? destination.name
        : this.defaultDestinationName;
      this.showDestinationSelectionModal = false;
      this.$emit("destinationSelected", destination);
    },
  },
};
</script>

<style>
.destination-selection {
  display: flex;
  align-items: center;
  justify-content: space-evenly;
  padding: 7px 0;
}

.destination-selection .v-btn {
  text-transform: none;
}

.destination-selection .v-btn .v-btn__content {
  width: 100% !important;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
  display: inline-block;
}
</style>