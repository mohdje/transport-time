<template>
  <div class="line-selection-container">
    <div class="header">
      <h3>{{ lineTypeFormatted }}</h3>
    </div>
    <div class="line-list">
      <div v-for="(line, index) in lines" :key="index">
        <LineLogo
          :line="line"
          @logoClick="onLogoClick(line)"
          :clickable="true"
        />
      </div>
    </div>
    <div class="time-table-list">
        <TimeTableWindow
          v-for="selectedLineId in selectedLinesId"
          :key="selectedLineId"
          :nextPassages="getNextPassagesForLine(selectedLineId)"
          @closeClick="onCloseTimeTableClick"
        />
    </div>
  </div>
</template>

<script>
import LineLogo from "@/components/lineSelection/lineLogo/LineLogo.vue";
import TimeTableWindow from "@/components/timeTableWindow/TimeTableWindow.vue";

export default {
  components: {
    LineLogo,
    TimeTableWindow,
  },
  props: {
    lineType: String,
    nextPassages: Array,
  },
  computed: {
    lineTypeFormatted() {
      if (this.lineType === "rail") return "Train/Rer";
      else
        return this.lineType[0].toUpperCase() + this.lineType.substring(1);
    },
    lines() {
      const linesId = this.nextPassages
        .map((nextPassage) => nextPassage.line.id)
        .filter((lineType, index, self) => self.indexOf(lineType) == index);

      const lines = [];
      const self = this;
      linesId.forEach((lineId) => {
        let nextPassageForLine = self.nextPassages.find(
          (nextPassage) => nextPassage.line.id === lineId
        );
        lines.push(nextPassageForLine.line);
      });

      return lines;
    },
  },
  data() {
    return {
      selectedLinesId: [],
    };
  },
  methods: {
    onLogoClick(line) {
      if (!this.selectedLinesId.includes(line.id))
        this.selectedLinesId.push(line.id);
    },
    getNextPassagesForLine(lineId) {
      return this.nextPassages.filter(
        (nextPassage) => nextPassage.line.id === lineId
      );
    },
    onCloseTimeTableClick(lineId) {
      this.selectedLinesId = this.selectedLinesId.filter((id) => id !== lineId);
    },
  },
};
</script>
<style scoped>
.line-selection-container {
  width: 100%;
  margin-bottom: 10px;
}

.line-selection-container .header {
  background-color: #e0e0e0;
  color: #464141;
  text-align: left;
  padding-left: 10px;
  border-top: solid 1px #5858582f;
  border-bottom: solid 1px #5858582f;
}

.line-selection-container .line-list {
  display: flex;
  flex-wrap: wrap;
}
</style>