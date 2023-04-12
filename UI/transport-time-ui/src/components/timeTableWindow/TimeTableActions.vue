<template>
  <div class="actions-container">
    <div class="action-btn" v-ripple v-delay-click-stop="pinToNotifications">
      <v-icon :color="getAppColor('blue-ratp')">mdi-pin</v-icon>
    </div>
    <div class="action-btn" v-ripple v-delay-click-stop="openAddFavoriteModal">
      <v-icon color="red">mdi-heart</v-icon>
    </div>
    <div
      class="action-btn"
      v-ripple
      v-delay-click-stop="() => (showSchedulePinModal = true)"
    >
      <v-icon :color="getAppColor('green-ratp')">mdi-timer-outline</v-icon>
    </div>
    <AddFavoriteModal 
      :visible="addFavoriteModalVisible"
      @validateClick="addFavorite"
      @cancelClick="closeAddFavoriteModal"/>
    <SchedulePinTimeModal
      :visible="showSchedulePinModal"
      @scheduleClick="scheduleNotification"
      @cancelClick="() => (showSchedulePinModal = false)"
    />
  </div>
</template>

<script>
import SchedulePinTimeModal from "@/components/modals/SchedulePinTimeModal.vue";
import AddFavoriteModal from "@/components/modals/AddFavoriteModal.vue";
import { colorAppMixin } from "@/js/mixins";


export default {
  components: {
    SchedulePinTimeModal,
    AddFavoriteModal
  },
  mixins: [colorAppMixin],
  props: {
    stopInfos: Object,
  },
  data() {
    return {
      showSchedulePinModal: false,
      addFavoriteModalVisible: false
    };
  },
  methods: {
    openAddFavoriteModal(){
      this.addFavoriteModalVisible = true;
    },
    closeAddFavoriteModal(){
      this.addFavoriteModalVisible = false;
    },
    addFavorite(groupName) {
      this.$phoneInterface.addFavorite(this.stopInfos, groupName);
      if(this.$phoneInterface.onFavoriteAdded) this.$phoneInterface.onFavoriteAdded()
      this.closeAddFavoriteModal();
    },
    pinToNotifications() {
      this.$phoneInterface.pinToNotifications(this.stopInfos);
    },
    scheduleNotification(schedule) {
      this.showSchedulePinModal = false;
      this.$phoneInterface.scheduleNotification(this.stopInfos, schedule.days, schedule.hours, schedule.minutes);
    },
  },
};
</script>

<style scoped>
.actions-container {
  padding: 7px;
  text-transform: none;
  display: flex;
  align-items: center;
  justify-content: space-evenly;
}

.action-btn {
  border: 1px solid rgba(212, 212, 212, 0.507);
  background-color: white;
  border-radius: 15px;
  width: 25%;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 3px;
}
</style>