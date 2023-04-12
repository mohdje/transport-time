<template>
  <v-text-field
    placeholder="Entrez un nom de groupe"
    :rules="validationRules"
    filled
    rounded
    dense
    clearable
    center
    v-model="textInput"
    @input="onValueChanged"
  ></v-text-field>
</template>

<script>
export default {
  name: "GroupNameTextInput",
  props:{
    defaultValue: String,
    existingGroupNames: Array
  },
  data(){
    return{
      textInput: '',
      groupNameMaxLength: 25,
    }
  },
  computed:{
    validationRules() {
      return [
        (v) => (!!v && !!v.trim()) || "Renseignez un nom",
        (v) => (!!v && v.length < this.groupNameMaxLength) || this.groupNameMaxLength + " charactères maximum",
        (v) => !this.existingGroupNames.includes(v) || "Ce nom est déjà utilisé"
      ];
    },
    isInputValid() {
      let error = 0;
      this.validationRules.forEach((rule) => {
        if (rule(this.textInput) !== true) error++;
      });

      return error === 0;
    }
  },
  created(){
    if(this.defaultValue) this.textInput = this.defaultValue;
  },
  methods:{
    onValueChanged(){
      this.$emit("valueChanged", { value: this.textInput, isValid: this.isInputValid });
    }
  }
};
</script>

<style>
</style>