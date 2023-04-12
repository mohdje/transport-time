<template>
  <ModalDialog
    :visible="visible"
    title="Favoris"
    validButtonLabel="Ajouter"
    :validButtonDisabled="!selectedGroupName"
    @outsideClick="cancelClick"
    @cancelClick="cancelClick"
    @validateClick="validateClick">
    <p>Choisissez à quel groupe ajouter ce favori :</p>
    <div class="favoris-group-choice">
      <v-radio-group v-model="radioGroupChoice">
        <v-radio label="Nouveau groupe" :value="1"></v-radio>
        <v-radio label="Groupe existant" :value="2"></v-radio>
      </v-radio-group>
    </div>
    <div class="group-input-container">
      <GroupNameTextInput
        v-if="radioGroupChoice === 1"
        @valueChanged="onValueChanged"
        :existingGroupNames="existingGroupNames"
      />
      <DropDown
        v-else-if="radioGroupChoice === 2"
        :values="existingGroupNames"
        @valueSelected="onValueSelected"
      />
    </div>
  </ModalDialog>
</template>

<script>
import ModalDialog from "@/components/common/ModalDialog.vue";
import DropDown from "@/components/common/DropDown.vue";
import GroupNameTextInput from "@/components/common/GroupNameTextInput.vue";

export default {
  name: "AddFavoriteModal",
  components: {
    ModalDialog,
    GroupNameTextInput,
    DropDown,
  },
  props: {
    visible: Boolean,
  },
  data() {
    return {
      radioGroupChoice: 1,
      selectedGroupName: "",
      existingGroupNames: [],
    };
  },
  watch: {
    radioGroupChoice() {
      this.selectedGroupName =
        this.radioGroupChoice === 2 ? this.existingGroupNames[0] : "";
    },
  },
  created() {
    this.existingGroupNames = this.$phoneInterface.getFavoritesGroupNames();
  },
  methods: {
    cancelClick() {
      this.$emit("cancelClick");
    },
    validateClick() {
      this.$emit("validateClick", this.selectedGroupName);
    },
    onValueChanged(args) {
      this.selectedGroupName = args.isValid ? args.value : "";
    },
    onValueSelected(value) {
      this.selectedGroupName = value;
    },
  },
};
</script>

<style scoped>
.group-input-container {
  height: 60px;
}

.favoris-group-choice {
  margin: auto;
  width: fit-content;
}
</style>