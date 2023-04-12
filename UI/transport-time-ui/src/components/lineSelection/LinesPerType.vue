<template>
  <transition name="fade">
    <div v-if="nextPassagesPerLine.length > 0" class="lines-per-type-container">
      <LineSelection
        v-for="(nextPassagesForLine, index) in nextPassagesPerLine"
        :key="index"
        :lineType="nextPassagesForLine.lineType"
        :nextPassages="nextPassagesForLine.nextPassages"
      />
    </div>
  </transition>
</template>

<script>
import LineSelection from "@/components/lineSelection/LineSelection.vue";

export default {
  components: { LineSelection },
  props: {
    nextPassages: Array,
  },
  computed: {
    nextPassagesPerLine() {
      if (!this.nextPassages || this.nextPassages.length === 0) return [];

      //distinct line types from next passages
      const lineTypes = this.nextPassages
        .map((nextPassage) => nextPassage.line.type)
        .filter((lineType, index, self) => self.indexOf(lineType) == index);

      const nextPassagesPerLine = [];
      const self = this;
      lineTypes.forEach((lineType) => {
        let nextPassages = self.nextPassages.filter(
          (nextPassage) => nextPassage.line.type === lineType
        );
        nextPassagesPerLine.push({
          lineType: lineType,
          nextPassages: nextPassages,
        });
      });
      return nextPassagesPerLine;
    },
  },
  methods: {},
};
</script>

<style scoped>
.lines-per-type-container {
  width: 100%;
  margin-top: 20px;
}
</style>