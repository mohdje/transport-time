<template>
  <div class="scheduled-notification-view">
    <TransportTimeList 
      :isLoading="isLoading" 
      :isEmptyList="isEmptyList"
      emptyListMessage="Aucune recherche d'horaires planifiée">
       <div class="day" v-for="day in daysToShow" :key="day.index">
          <h3 class="day-title">{{ day.name }}</h3>
          <div 
            class="time-and-stop" 
            v-for="(scheduledNotif, index) in getScheduledNotificationsByDay(day.index)" 
            :key="index"
            v-ripple
            v-delay-click="() => { scheduledNotifClick(scheduledNotif) }">
            <h4 class="time">{{ getFormattedTime(scheduledNotif.scheduledTime.hours, scheduledNotif.scheduledTime.minutes)}}</h4>
            <div class="line-and-stop-container">
              <LineAndStopPresentation :line="scheduledNotif.line" :stopData="scheduledNotif.transportStopData" />
            </div>
          </div>
        </div>
    </TransportTimeList>
    <DeleteScheduledNotificationModal 
      :visible="showDeleteModal" 
      :scheduledNotification="scheduledNotificationToDelete"
      @cancel="hideDeleteModal"
      @deleteClick="delteScheduledNotif"/>
  </div>
</template> 
<script>
import { daysOfWeekMixin, timeFormatMixin } from "@/js/mixins.js";
import DeleteScheduledNotificationModal from "@/components/modals/DeleteScheduledNotificationModal.vue";
import LineAndStopPresentation from '@/components/common/LineAndStopPresentation.vue';
import TransportTimeList from '@/components/common/TransportTimeList.vue';

export default {
  name: "ScheduledNotificationsView",
  components: { 
    TransportTimeList,
    DeleteScheduledNotificationModal,
    LineAndStopPresentation },
  mixins: [
    daysOfWeekMixin,
    timeFormatMixin],
  data() {
    return {
      scheduledNotifications: [],
      isLoading: false,
      showDeleteModal: false,
      scheduledNotificationToDelete: null
    };
  },
  mounted() {
    this.loadScheduledNotifications();
  },
  computed:{
      daysToShow(){
        return this.daysOfWeek.filter(day => 
            this.scheduledNotifications.find((scheduledNotif) => scheduledNotif.scheduledTime.day === day.index));
      },
      isEmptyList(){
        return !this.isLoading && this.scheduledNotifications && this.scheduledNotifications.length === 0;
      }
  },
  methods: {
    loadScheduledNotifications() {
        this.isLoading = true;
        window.scheduledNotifView = this;
        this.$phoneInterface.getScheduledNotifications(
        (scheduledNotifications) => {
            if (scheduledNotifications.result){
                window.scheduledNotifView.scheduledNotifications = scheduledNotifications.result;
                window.scheduledNotifView.isLoading = false;
            }
        }
        );
    },
    getScheduledNotificationsByDay(dayIndex) {
      const scheduledNotifOfDay = this.scheduledNotifications.filter((scheduledNotif) => scheduledNotif.scheduledTime.day === dayIndex);
      return scheduledNotifOfDay.sort((a, b) => {
        return a.scheduledTime.hours === b.scheduledTime.hours ? a.scheduledTime.minutes - b.scheduledTime.minutes : a.scheduledTime.hours - b.scheduledTime.hours;
      });
    },
    showDay(dayIndex) {
      return this.scheduledNotifications.find((scheduledNotif) => scheduledNotif.scheduledTime.day === dayIndex);
    },
    scheduledNotifClick(scheduledNotif){
      this.scheduledNotificationToDelete = scheduledNotif;
      this.showDeleteModal = true;
    },
    hideDeleteModal(){
      this.showDeleteModal = false;
    },
    delteScheduledNotif(){
      this.showDeleteModal = false;
      this.$phoneInterface.deleteScheduledNotifications(this.scheduledNotificationToDelete);
      this.scheduledNotificationToDelete = null;
      this.loadScheduledNotifications();
    }
  }
}
</script>
 
<style scoped>
.scheduled-notification-view{
  height: 100%;
}

.scheduled-notification-view .day {
  background-color: var(--light-grey);
  border-bottom: 3px solid #e6e6e6;
}

.scheduled-notification-view .day .day-title {
  padding-left: 5px;
}

.scheduled-notification-view .time-and-stop .time{
  padding-left: 7%;
  color: #919191;
}

.line-and-stop-container{
  border-radius: 20px;
  background-color: white;
  width: 90%;
  min-height: 80px;
  border: 3px solid #b6b6b6;
  margin: 0 auto 12px auto;
  padding: 7px;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>