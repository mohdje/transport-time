<template>
  <div
    class="info-traffic-container"
    :style="{ backgroundColor: backgroundColor }"
  >
    <div v-if="state === 0" class="state">
      <div>Traffic</div>
      <v-progress-circular
        size="20"
        indeterminate
        color="white"
        width="3"
      ></v-progress-circular>
    </div>
      <div v-else-if="state === 1" class="state">
      <div>Perturbations</div>
      <v-btn icon color="white"  v-delay-click-stop="() => (showModal = true)">
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </div>
    <div v-else>Traffic normal</div>
    <info-traffic-modal
      :visible="showModal"
      :messages="messages"
      title="Perturbations"
      @outsideClick="() => (showModal = false)"
    />
  </div>
</template>

<script>
import InfoTrafficModal from "@/components/modals/InfoModal.vue";
import { colorAppMixin } from "@/js/mixins";

export default {
  components: {
    InfoTrafficModal,
  },
  mixins: [colorAppMixin],
  props: {
    lineId: String,
  },
  mounted() {
    window.infoTrafficComponent = this;
    this.$phoneInterface.getInfoTrafficAsync(this.lineId, (e) => {
      if (e.result.messages && e.result.messages.length > 0) {
        window.infoTrafficComponent.state = 1;
        window.infoTrafficComponent.messages = e.result.messages;
      } else {
        window.infoTrafficComponent.state = 2;
        window.infoTrafficComponent.messages = [];
      }
    });
  },
  computed: {
    backgroundColor() {
      if (this.state === 0) return this.getAppColor("blue-ratp");
      else if (this.state === 1) return "orange";
      else  return this.getAppColor("green-ratp");
    },
  },
  data() {
    return {
      state: 0,
      messages: [],
      showModal: false,
    };
  },
};
</script>

<style scoped>
.info-traffic-container {
  width: 45%;
  height: 35px;
  border-radius: 30px;
  padding: 5px;
  color: white;
  font-weight: 600;
  text-align: center;
}

.state {
  display: flex;
  align-items: center;
  justify-content: space-evenly;
  width: 100%;
  height: 100%;
}
</style>