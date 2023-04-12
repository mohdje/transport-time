<template>
  <transition name="fade">
     <div v-if="nextPassages && nextPassages.length > 0" class="time-table-container">
      <div class="btn close" v-delay-click-stop="onCloseClick">
        <v-btn icon color="grey">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </div>
      <div class="header">
        <LineLogo :line="stopInfos ? stopInfos.line : null" />
        <InfoTraffic :lineId="stopInfos ? stopInfos.line.id : null" />
      </div>
      <v-divider></v-divider>
      <TimeTableActions :stopInfos="stopInfos"/>
      <v-divider></v-divider>
      <TrainTimeTable 
        v-if="isTrainLineType" 
        :nextPassages="nextPassages" 
        :showDestinationSelection="!hideDestinationTrainSelection" 
        @onDestinationSelected="onDestinationSelected"/>
      <BasicTimeTable v-else :nextPassages="nextPassages" />
    </div>
  </transition>
</template>

<script>
import LineLogo from "@/components/lineSelection/lineLogo/LineLogo.vue";
import TimeTableActions from "@/components/timeTableWindow/TimeTableActions.vue";
import InfoTraffic from "@/components/timeTableWindow/InfoTraffic.vue";
import BasicTimeTable from "./timeTables/BasicTimeTable.vue";
import TrainTimeTable from "./timeTables/TrainTimeTable.vue";

export default {
  components: {
    LineLogo,
    TimeTableActions,
    InfoTraffic,
    BasicTimeTable,
    TrainTimeTable,
  },
  props: {
    nextPassages: Array,
    hideDestinationTrainSelection: Boolean
  },
  computed: {
    stopInfos(){
      if (!this.nextPassages || this.nextPassages.length === 0) return null;
      return {
        line: this.nextPassages[0].line,
        stopArea: this.nextPassages[0].stopArea,
        stopAreaDestination : this.selectedStopAreaDestination || this.nextPassages[0].destinationStopArea
      } 
    },
    isTrainLineType() {    
      return this.stopInfos ? this.stopInfos.line.type === "rail" : false;
    }
  },
  mounted() {
    if(this.nextPassages && this.nextPassages.length > 0)
      this.scrollToTimeTableWindow();
  },
  data(){
    return {
      selectedStopAreaDestination: null
    }
  },
  watch:{
    nextPassages(){
      if(this.nextPassages && this.nextPassages.length > 0)
        this.scrollToTimeTableWindow();
    }
  },
  methods: {
    onCloseClick() {
      this.$emit("closeClick", this.stopInfos.line.id);
    },
    scrollToTimeTableWindow() {
      const self = this;
      setTimeout(() => {
        let rect = self.$el.getClientRects()[0];
        let activityContainer = document.getElementsByClassName(
          "activity-view-scrollable-content"
        )[0];
        activityContainer.scrollTo({
          top: rect.top,
          behavior: "smooth",
        });
      }, 200);
    },
    onDestinationSelected(stopAreaDestination){
      this.selectedStopAreaDestination = stopAreaDestination;
    }
  },
};
</script>

<style>
.time-table-container {
  border-radius: 5px;
  background-color: white;
  width: 90%;
  box-shadow: rgba(50, 50, 105, 0.15) 0px 2px 5px 0px,
    rgba(0, 0, 0, 0.05) 0px 1px 1px 0px;
  margin: 15px auto;
  position: relative;
  z-index: 1;
}

.time-table-container .header {
  display: flex;
  align-items: center;
  justify-content: center;
}


.time-table-container .btn {
  position: absolute;
  z-index: 2;
  border-radius: 50%;
}

.time-table-container .btn.close {
  top: 0;
  right: 0;
}

.time-table-row {
  display: flex;
  align-items: center;
  justify-content: space-around;
}

.time-table-row.two-columns > div, .time-table-row.two-columns > h4 {
  width: 50%;
  margin: 4px 0;
}

.time-table-row.three-columns > div, .time-table-row.three-columns >h4 {
  width: 33%;
  margin: 4px 0;
}


.time-table-row > div > .cell-time {
  background-color: black;
  color: rgb(241, 199, 10);
  width: 55%;
  margin: 0 auto;
  border-radius: 5px;
  padding: 3px;
  font-weight: 600;
}

.time-table-row.col-header {
  font-size: 20px;
  font-weight: 500;
}
</style>