<template>
  <ModalDialog 
    :visible="visible" 
    :title="title"
    :backgroundColor="getAppColor('blue-ratp')"
    textColor="white"
    @outsideClick="onOutsideClick">
    <div class="info-modal-container">
      <ul class="message-list">
        <li v-for="(message, index) in messages" :key="index">{{ message }}</li>
      </ul>
    </div>
  </ModalDialog>
</template>
<script>
import ModalDialog from "@/components/common/ModalDialog.vue";
import { colorAppMixin } from "@/js/mixins";

export default {
  components: {
    ModalDialog,
  },
  mixins: [colorAppMixin],
  props: {
    visible: Boolean,
    title: String,
    messages: Array,
  },
  data() {
    return {
      showDialog: false,
    };
  },
  watch: {
    visible(value) {
      this.showDialog = value;
    },
  },
  methods: {
    onOutsideClick() {
      this.$emit("outsideClick");
    },
  },
};
</script>
<style scoped>
.message-list{
  font-weight: 600;
  margin-top: 15px;
  max-height: 400px;
  overflow-y: auto;
  text-align: left;
}
</style>