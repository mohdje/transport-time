<template>
    <v-progress-circular :style="style" v-if="showProgressCircle" indeterminate :color="getAppColor('green-ratp')"></v-progress-circular> 
</template>

<script>
import { colorAppMixin } from "@/js/mixins";
export default {
    props:{
        visible: Boolean,
        center: Boolean
    },
    mixins: [colorAppMixin],
    computed:{
        showProgressCircle(){
            return this.visible;
        },
        style(){
            if(this.center){
                return {
                    position: 'absolute',
                    left: '50%',
                    top: '50%',
                    transform: 'translate(-50%, 50%)'
                }
            }

            return null;
        }
    }
}
</script>
