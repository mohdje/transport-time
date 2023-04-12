<template>
  <ModalDialog 
  :visible="visible" 
  title="Notifications"
  validButtonLabel="Supprimer"
  @cancelClick="cancel"
  @validateClick="deleteClick"
  @outsideClick="cancel">
    <p>Voulez vous supprimer la notification pour l'arrêt {{ stopInfo }} à {{ time }}</p>
  </ModalDialog>
</template>

<script>
import ModalDialog from "@/components/common/ModalDialog.vue";
import { timeFormatMixin } from "@/js/mixins.js";

export default {
  name: "DeleteScheduledNotificationModal",
  components: {
    ModalDialog,
  },
  mixins: [ timeFormatMixin ],
  props: {
    visible: Boolean,
    scheduledNotification: Object
  },
  computed: {
    stopInfo(){
      return this.scheduledNotification ? this.scheduledNotification.transportStopData.stopAreaName + " (ligne " + this.scheduledNotification.line.name + ")" : null;
    },
    time(){
      return this.scheduledNotification ? this.getFormattedTime(this.scheduledNotification.scheduledTime.hours, this.scheduledNotification.scheduledTime.minutes) : null;
    }
  },
  methods: {
    cancel() {
      this.$emit("cancel");
    },
    deleteClick(){
      this.$emit("deleteClick");
    }
  },
};
</script>
