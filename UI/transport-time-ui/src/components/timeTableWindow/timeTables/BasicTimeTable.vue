 <template>
  <div style="text-align: center">
    <div class="time-table-row col-header two-columns">
      <h5>Direction</h5>
      <h5>Arrive dans</h5>
    </div>
    <div
      class="time-table-row two-columns"
      v-for="(nextPassage, index) in nextPassagesToShow"
      :key="index"
    >
      <div>{{ nextPassage.directionName }}</div>
      <div>
        <div class="cell-time">{{ nextPassage.remainingTime }}</div>
      </div>
    </div>
  </div>
</template>
 
<script>
import { nextPassagesPerDirectionMixin } from "@/js/mixins.js";

export default {
  props: {
    nextPassages: Array,
  },
  mixins: [nextPassagesPerDirectionMixin],
  computed: {
    nextPassagesToShow() {
      return this.nextPassagesPerDirection(this.nextPassages, 3);
    },
  },
};
</script>
 
<style>
</style>