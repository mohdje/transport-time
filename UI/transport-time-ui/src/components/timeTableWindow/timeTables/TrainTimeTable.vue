 <template>
  <div>
    <DestinationSelection v-if="showDestinationSelection" @destinationSelected="onDestinationSelected" />
    <v-divider></v-divider>
    <div class="train-time-table-content">
      <div class="center">
        <SpinnerProgress :visible="searchInProgress" />
        <ErrorMessage
          text="Aucun train trouvé pour cette destination"
          :visible="showNoResultMessage"
        />
      </div>
      <div v-if="showTimeTable">
        <div class="time-table-row col-header small two-columns">
          <h4>Direction</h4>
          <h4>Départ dans</h4>
          <h4 v-if="!showInfosColumn">Code</h4>
          <h4 v-if="showInfosColumn">Infos</h4>
        </div>
        <div
          class="time-table-row two-columns"
          v-for="(nextPassage, index) in nextPassagesToShow"
          :key="index"
        >
          <div>{{ nextPassage.directionName }}</div>
          <div>
            <div class="cell-time large">{{ nextPassage.remainingTime }}</div>
          </div>
          <div v-if="!showInfosColumn">{{ nextPassage.missionName }}</div>
          <div v-if="showInfosColumn">
            <v-btn
              fab
              small
              elevation="1"
              v-delay-click="() => showTrainInfoModal(nextPassage)"
              ><v-icon>mdi-plus</v-icon></v-btn
            >
          </div>
        </div>
        <InfoModal
          :visible="trainInfosModal.visible"
          :title="trainInfosModal.title"
          :messages="trainInfosModal.messages"
          @outsideClick="() => (trainInfosModal.visible = false)"
        />
      </div>
    </div>
  </div>
</template>
 
 <script>
import SpinnerProgress from "@/components/common/SpinnerProgress.vue";
import ErrorMessage from "@/components/common/ErrorMessage.vue";
import DestinationSelection from "@/components/timeTableWindow/DestinationSelection.vue";
import InfoModal from "@/components/modals/InfoModal.vue";
import { nextPassagesPerDirectionMixin } from "@/js/mixins.js";

export default {
  components: {
    SpinnerProgress,
    ErrorMessage,
    DestinationSelection,
    InfoModal,
  },
  props: {
    nextPassages: Array,
    showDestinationSelection: Boolean
  },
  mixins: [nextPassagesPerDirectionMixin],
  mounted() {
    window.trainTimeTableComp = this;
  },
  data() {
    return {
      nextPassagesForDestination: [],
      selectedDestination: null,
      searchInProgress: false,
      trainInfosModal: {
        visible: false,
        title: "Infos train",
        messages: [],
      },
    };
  },
  computed: {
    nextPassagesToShow() {
      return this.selectedDestination
        ? this.nextPassagesForDestination
        : this.nextPassagesPerDirection(this.nextPassages, 2);
    },
    showInfosColumn() {
      return (
        this.nextPassagesForDestination &&
        this.nextPassagesForDestination.length > 0
      );
    },
    showNoResultMessage() {
      return (
        !this.searchInProgress &&
        this.selectedDestination &&
        this.nextPassagesForDestination.length === 0
      );
    },
    showTimeTable() {
      return (
        !this.searchInProgress &&
        this.nextPassagesToShow &&
        this.nextPassagesToShow.length > 0
      );
    },
  },
  methods: {
    onDestinationSelected(destination) {
      this.selectedDestination = destination;
      if (destination) this.searchNextPassagesForDestination(destination);
      else this.nextPassagesForDestination = [];
      this.$emit('onDestinationSelected', destination);
    },
    searchNextPassagesForDestination(destination) {
      this.searchInProgress = true;

      const stopAreaId = this.nextPassages[0].stopArea.id;
      const lineId = this.nextPassages[0].line.id;

      this.$phoneInterface.getNextPassagesForDestinationAsync(
        stopAreaId, 
        destination.id,
        lineId,
        (e) => {
          window.trainTimeTableComp.searchInProgress = false;
          window.trainTimeTableComp.nextPassagesForDestination = e.result;
        } 
      );
    },

    showTrainInfoModal(nextPassageForDestination) {
      const messages = [];
      messages.push("Code mission: " + nextPassageForDestination.missionName);
      if (nextPassageForDestination.destinationArrivalTime) {
        const arrivalDateTime = new Date(nextPassageForDestination.destinationArrivalTime);
        arrivalDateTime.setHours(arrivalDateTime.getHours());
        const hours = arrivalDateTime.getHours();
        const minutes = arrivalDateTime.getMinutes();
        const arrivalTimeFormatted =
          hours + ":" + (minutes < 10 ? "0" + minutes : minutes);

        messages.push(
          "Arrivée à " +
            this.selectedDestination.name +
            " prévue à " +
            arrivalTimeFormatted
        );
      } else {
        messages.push("Horaire d'arrivée indisponible");
      }

      this.trainInfosModal.messages = messages;
      this.trainInfosModal.visible = true;
    },
  },
};
</script>
 <style>
.train-time-table-content {
  position: relative;
  min-height: 90px;
  text-align: center;
}

.train-time-table-content .center {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.time-table-row.col-header.small {
  font-size: 15px;
}

.cell-time.large {
  width: 80% !important;
}
</style>