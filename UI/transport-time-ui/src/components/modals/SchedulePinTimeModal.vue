<template>
  <ModalDialog
    :visible="visible"
    title="Planifier"
    validButtonLabel="Planifier"
    :validButtonDisabled="!isDateTimeSelected"
    @outsideClick="cancelClick"
    @cancelClick="cancelClick"
    @validateClick="validateClick"
  >
    <p>
      Choisissez les jours et l'heure à laquelle les horaires de cet arrêt
      seront affichés sur votre écran de notifications
    </p>
    <div class="schedule-day-time-container">
      <input type="time" class="schedule-time" v-model="scheduledTime" />
    </div>
    <div class="day-checkboxes">
      <div class="check-box-container">
        <v-checkbox
          style="margin: 0 auto"
          v-for="day in checkBoxDays"
          :key="day.name"
          v-model="day.checked"
          :label="day.name"
        ></v-checkbox>
      </div>
    </div>
  </ModalDialog>
</template>

<script>
import ModalDialog from "@/components/common/ModalDialog.vue";
import { daysOfWeekMixin } from "@/js/mixins.js";

export default {
  components: {
    ModalDialog
  },
  props: {
    visible: Boolean,
    transportInfos: Object,
  },
  created() {
    this.checkBoxDays = this.daysOfWeek.map((day) => ({
      ...day,
      checked: false,
    }));
  },
  mixins: [daysOfWeekMixin],
  data() {
    return {
      showDialog: false,
      checkBoxDays: [],
      scheduledTime: "08:30",
    };
  },
  computed: {
    isDateTimeSelected: function () {
      return Boolean(
        this.checkBoxDays.filter((d) => {
          return d.checked;
        }).length > 0 && this.scheduledTime
      );
    },
  },
  methods: {
    cancelClick: function () {
      this.$emit("cancelClick");
    },
    checkStateChanged: function (day) {
      var dayToModify = this.checkBoxDays.find((d) => {
        return d.name === day.name;
      });
      dayToModify.checked = day.checked;
    },
    validateClick: function () {
      var time = this.scheduledTime.split(":");
      var hours = parseInt(time[0]);
      var minutes = parseInt(time[1]);

      var daysIndex = "";
      this.checkBoxDays.forEach((day) => {
        if (day.checked) {
          daysIndex += "," + day.index.toString();
        }
      });
      daysIndex = daysIndex.substring(1);

      this.$emit("scheduleClick", {
        hours: hours,
        minutes: minutes,
        days: daysIndex,
      });
    },
  },
};
</script>
<style>
.schedule-day-time-container {
  text-align: center;
}

.day-checkboxes {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin: 10px 15px;
}

.check-box-container {
  width: 30%;
  text-align: left;
}

.day-checkboxes .v-input--selection-controls {
  margin: 0 !important;
  padding: 0 !important;
}

.day-checkboxes .v-messages {
  display: none !important;
}

.schedule-time {
  border: none;
  color: white;
  font-size: x-large;
  background-color: var(--blue-ratp);
  text-align: center;
  width: 120px;
  outline: none;
  border-radius: 10px;
  box-shadow: rgba(99, 99, 99, 0.2) 0px 2px 8px 0px;
}
</style>
