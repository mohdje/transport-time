<template>
  <div class="search-input-container">
    <v-text-field
      placeholder="Entrez un nom de station"
      filled
      rounded
      dense
      clearable
      center
      @input="onTextChanged"
      :value="selectedStopArea ? selectedStopArea.name : null"
    ></v-text-field>
    <div class="relative-content">
      <spinner-progress :visible="showSearchProgress" />
      <error-message text="Aucune station trouvée" :visible="showNoResultMsg" />
      <li v-if="showResultList" class="list-result-container">
        <ul
          v-for="(stopArea, index) in stopAreasToDisplay"
          :key="index"
          v-ripple
          v-delay-click="() => onStopAreaClick(stopArea)"
        >
        <StopAreaLabel :text=" displayStopAreaLabel(stopArea)"/>
        </ul>
        <div class="more-btn" v-ripple v-if="showMoreButton" v-delay-click="increaseStopAreasToDisplay">Voir plus</div>
      </li>
    </div>
  </div>
</template>

<script>
import SpinnerProgress from "@/components/common/SpinnerProgress.vue";
import ErrorMessage from "@/components/common/ErrorMessage.vue";
import StopAreaLabel from "@/components/common/StopAreaLabel.vue";

export default {
  components: {
    SpinnerProgress,
    ErrorMessage,
    StopAreaLabel
  },
  computed: {
    showResultList: function () {
      return this.fullStopAreas.length > 0;
    },
    stopAreasToDisplay: function() {
      return this.fullStopAreas.slice(0, this.nbStopAreasToDisplay - 1);
    },
    showMoreButton: function() {
      return this.fullStopAreas.length > this.nbStopAreasToDisplay;
    }
  },
  data() {
    return {
      selectedStopArea: null,
      fullStopAreas: [],
      showSearchProgress: false,
      showNoResultMsg: false,
      searchText: "",
      nbStopAreasToDisplay: 10,
      minSearchTextLength: 3,
    };
  },
  methods: {
    onTextChanged(value) {
      this.$emit("textChanged");
      this.showNoResultMsg = false;
      this.fullStopAreas = [];
      this.nbStopAreasToDisplay = 10;
      this.searchText = value;
      window.searchInput = this;
      if (value && value.length >= this.minSearchTextLength) {
        this.showSearchProgress = true;
        this.$phoneInterface.searchStopsAreasAsync(value, (e) => {
          const sameSearchText = e.result.searchText === window.searchInput.searchText;
          window.searchInput.showSearchProgress = !sameSearchText;
          window.searchInput.showNoResultMsg = false;
          if (
            e.result &&
            sameSearchText &&
            e.result.list &&
            e.result.list.length > 0
          )
            window.searchInput.fullStopAreas = e.result.list;
          else {
            window.searchInput.fullStopAreas = [];
            window.searchInput.showNoResultMsg = sameSearchText;
          }
        });
      }
    },
    onStopAreaClick(stopArea) {
      this.selectedStopArea = stopArea;
      this.fullStopAreas = [];
      this.$emit("stopAreaSelected", stopArea);
    },
    displayStopAreaLabel(stopArea) {
      let nameIsNotUnique =
        this.fullStopAreas.filter((s) => s.name === stopArea.name).length > 1;
      return nameIsNotUnique
        ? stopArea.name + " (" + stopArea.town + ")"
        : stopArea.name;
    },
    increaseStopAreasToDisplay(){
      if(this.nbStopAreasToDisplay < this.fullStopAreas.length)
        this.nbStopAreasToDisplay += 10;     
    }
  },
};
</script>

<style>
.search-input-container {
  position: relative;
  margin: 0 7px;
}

.search-input-container .v-text-field__slot > label {
  text-align: center;
  width: 100%;
}

.search-input-container .v-text-field__slot > input {
  text-align: center;
}

.search-input-container .relative-content {
  position: absolute;
  top: 75%;
  width: 100%;
}

.search-input-container .list-result-container {
  max-height: 170px;
  overflow-y: scroll;
  width: 100%;
  box-shadow: rgba(0, 0, 0, 0.16) 0px 1px 4px;
  list-style-type: none;
}

.search-input-container .list-result-container ul{
  border-bottom: 1px solid white;
  padding: 0;
}


.more-btn{
  width: 100%;
  background-color: var(--green-ratp);
  color: white;
  font-weight: 600;
  padding: 5px 0;
}
</style>