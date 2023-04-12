<template>
  <div class="search-view-container">
    <h3>Sélectionnez une station</h3>
    <StopAreaSearchInput
      @stopAreaSelected="onStopAreaSelected"
      @textChanged="onSearchTextChanged"
    />
    <SpinnerProgress :visible="showSearchProgress" />
    <ErrorMessage
      text="Aucun horaire trouvé pour cette station"
      :visible="showNoResultMessage"
    />
    <GreenButton 
      v-if="nextPassages.length > 0" 
      @onClick="onUpdateClick"
      elevation
      text="Mettre à jour"/>
    <LinesPerType :nextPassages="nextPassages" />
  </div>
</template>

<script>
import StopAreaSearchInput from "@/components/common/StopAreaSearchInput.vue";
import SpinnerProgress from "@/components/common/SpinnerProgress.vue";
import LinesPerType from "@/components/lineSelection/LinesPerType.vue";
import ErrorMessage from "@/components/common/ErrorMessage.vue";
import GreenButton from "@/components/common/GreenButton.vue";
//import FakeData from "@/js/phoneInterface/fakeData.js";

export default {
  components: {
    SpinnerProgress,
    StopAreaSearchInput,
    LinesPerType,
    ErrorMessage,
    GreenButton
  },
  mounted() {
    //for test
    // this.nextPassages = 
    //   FakeData.nextPassages.map((x) => ({
    //     ...x,
    //     stopArea: { id: "stopArea_ID", name: "stopArea_Name" },
    //   }));
    window.searchViewComp = this;
  },
  data() {
    return {
      nextPassages: [],
      showSearchProgress: false,
      showNoResultMessage: false,
    };
  },
  methods: {
    onSearchTextChanged() {
      this.showNoResultMessage = false;
      this.nextPassages = [];
    },
    onNoNextPassagesFound() {
      this.showNoResultMessage = true;
    },
    onUpdateClick() {
      this.nextPassages = [];
      this.onStopAreaSelected(window.searchViewComp.selectedStopArea);
    },
    onStopAreaSelected(stopArea) {
      this.showSearchProgress = true;
      window.searchViewComp.selectedStopArea = stopArea;
      this.$phoneInterface.getNextPassagesAsync(stopArea.id, (e) => {
        window.searchViewComp.showSearchProgress = false;
        if (e.result && e.result.length > 0) {
          const nextPassages = e.result.map((x) => ({
            ...x,
            stopArea: window.searchViewComp.selectedStopArea,
          }));
          window.searchViewComp.nextPassages = nextPassages;
        } else window.searchViewComp.showNoResultMessage = true;
      });
    },
  },
};
</script>

<style scoped>
.search-view-container {
  text-align: center;
  margin: 10px 0px;
}
</style>