<template>
  <div class="line-and-stop-presentation">
      <LineLogo :line="line" />
      <div style="flex-grow: 3">
        <StopAreaLabel :text="stopData.stopAreaName" />
        <div
          style="width: 100%; text-align: center"
          v-if="stopData.destinationStopAreaName">
          <v-icon color="grey">mdi-arrow-down</v-icon>
          <StopAreaLabel :text="stopData.destinationStopAreaName" />
        </div>
      </div>
  </div>
</template>

<script>
import LineLogo from "@/components/lineSelection/lineLogo/LineLogo.vue";
import StopAreaLabel from "@/components/common/StopAreaLabel.vue";

export default {
  name: "LineAndStopPresentation",
  components: {
    LineLogo,
    StopAreaLabel,
  },
  props: {
    line: Object,
    stopData: Object,
  },
};
</script>
<style scoped>
.line-and-stop-presentation{
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  flex-grow: 3;
}
</style>
