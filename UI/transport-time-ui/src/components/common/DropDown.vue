<template>
  <div ref="dropdownContainer" class="dropdown-container">
    <v-btn
      v-delay-click="toggleDropdown"
      :color="getAppColor('blue-ratp')"
      :style="dropdownButtonStyle"
      :elevation="1">
      {{ selectedValue }}
      <v-icon right>mdi-arrow-down-drop-circle</v-icon>
    </v-btn>
    <transition name="fade">
      <ul v-if="dropdownListVisible" class="dropdown-list">
        <li v-for="(value, index) in values" 
          :key="value" 
          v-ripple
          @click="dropdownValueSelected(index)">
          {{ value }}
        </li>
      </ul>
    </transition>
  </div>
</template>

<script>
import { colorAppMixin } from "@/js/mixins.js";

export default {
  name: 'DropDown',
  mixins: [ colorAppMixin ],
  props:{
    values: Array
  },
  data(){
    return {
      selectedIndex: 0,
      dropdownListVisible: false,
      dropdownButtonStyle:{
        width: '200px', 
        color: 'white', 
        textTransform: 'none',
        fontSize: '18px'
      }
    }
  },
  computed:{
    selectedValue(){
      const selectedValue = this.values[this.selectedIndex];
      const maxLength = 15;
      return selectedValue.length > maxLength ? selectedValue.substring(0, maxLength) + "..." : selectedValue;
    }
  },
  created(){
    window.addEventListener('click', this.checkIfClickOutside, false);
  },
  beforeDestroy(){
    window.removeEventListener('click', this.checkIfClickOutside, false);
  },
  methods:{
    toggleDropdown(){
      this.dropdownListVisible = !this.dropdownListVisible;
    },
    dropdownValueSelected(selectedIndex){
      this.selectedIndex = selectedIndex;
      this.dropdownListVisible = false;
      this.$emit("valueSelected", this.values[selectedIndex]);
    },
    checkIfClickOutside(e){
      if(!this.$refs.dropdownContainer.contains(e.target))
        this.dropdownListVisible = false;
    }
  }
}
</script>

<style scoped>
.dropdown-container{
  position: relative;
  width: fit-content;
  margin: auto;
}

.dropdown-list {
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translate(-50%, 100%);
  background-color: var(--darkblue-ratp);
  z-index: 5;
  width: 100%;
  padding: 0;
  border-radius: 10px;
  box-shadow: rgba(0, 0, 0, 0.1) 0px 4px 6px -1px, rgba(0, 0, 0, 0.06) 0px 2px 4px -1px;
}

.dropdown-list li{
  list-style: none;
  font-weight: 600;
  color: white;
  padding: 5px 0;
  border-top: 1px solid rgba(255, 255, 255, 0.479);
}
</style>