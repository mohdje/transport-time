<template>
  <ModalDialog 
    :visible="visible" 
    title="Favoris"
    validButtonLabel="Appliquer"
    :validButtonDisabled="!isGroupNameValid"
    @outsideClick="cancelClick"
    @cancelClick="cancelClick"
    @validateClick="validateClick">
      <p>Entrez ci-dessous le nom à appliquer au groupe de favoris :</p>
      <GroupNameTextInput 
        @valueChanged="onValueChanged"
        :defaultValue="actualGroupName"
        :existingGroupNames="existingGroupNames"/>
  </ModalDialog>
</template>

<script>
import ModalDialog from "@/components/common/ModalDialog.vue";
import GroupNameTextInput from '@/components/common/GroupNameTextInput.vue';

export default {
  name: "ChangeFavoriteGroupNameModal",
  components: {
    ModalDialog,
    GroupNameTextInput
  },
  props: {
    actualGroupName: String,
    existingGroupNames: Array,
    visible: Boolean,
  },
  data() {
    return {
      groupName: "",
      isGroupNameValid: true,
    };
  },
  watch: {
    actualGroupName() {
      this.groupName = this.actualGroupName;
    },
  },
  methods: {
    onValueChanged(args){
      this.groupName = args.value;
      this.isGroupNameValid = args.isValid;
    },
    validateClick() {
      this.$emit("validateClick", this.groupName);
    },
    cancelClick() {
      this.$emit("cancelClick");
    },
  },
};
</script>

