<template>
  <v-dialog
    v-model="showDialog"
    persistent
    max-width="90%"
    @click:outside="onOutsideClick"
  >
    <v-card :style="cardStyle">
      <div class="modal-content-container">
        <h3>{{ title }}</h3>
        <slot></slot>
        <v-card-actions v-if="!!validButtonLabel" style="justify-content: center">
          <v-btn text v-delay-click="onCancelClick">Annuler</v-btn>
          <GreenButton
            :text="validButtonLabel"
            :disabled="validButtonDisabled"
            elevation
            @onClick="onValidateClick"
          />
        </v-card-actions>
      </div>
    </v-card>
  </v-dialog>
</template>

<script>
import GreenButton from "@/components/common/GreenButton.vue";

export default {
  components: {
    GreenButton,
  },
  props: {
    visible: Boolean,
    backgroundColor: String,
    textColor: String,
    validButtonLabel: String,
    validButtonDisabled: Boolean,
    title: String,
  },
  data() {
    return {
      showDialog: false,
    };
  },
  computed: {
    cardStyle() {
      return {
        backgroundColor: this.backgroundColor,
        color: this.textColor
      };
    },
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
    onCancelClick() {
      this.$emit("cancelClick");
    },
    onValidateClick() {
      this.$emit("validateClick");
    },
  },
};
</script>

<style>
.v-dialog.v-dialog--active {
  overflow: visible !important;
}

.modal-content-container {
  text-align: center;
  padding: 20px 14px;
}
</style>