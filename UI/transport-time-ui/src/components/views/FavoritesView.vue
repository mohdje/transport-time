<template>
  <div class="favorites-view">
    <TransportTimeList 
      :isLoading="isLoading" 
      :isEmptyList="isEmptyList"
      emptyListMessage="Aucun favori enregistré">
      <div v-for="groupName in groupNames" :key="groupName">
          <div class="favorite-group" v-ripple v-delay-click="() => showChangeGroupNameModal(groupName)">
            <h3>{{ groupName }}</h3>
          </div>
          <div
            v-for="favorite in getFavoritesByGroupName(groupName)"
            :key="favorite.id"
            :class="['favorite-container', { 'selected' : selectedFavoriteIds.includes(favorite.id) } ]"
             v-ripple
            @touchstart="onTouchStart(favorite.id)"
            @touchend="onTouchEnd"
            v-delay-click="()=> {editMode ? toggleFavoriteSelection(favorite.id) : loadTimeTable(favorite.id)}">
            <LineAndStopPresentation :line="favorite.line" :stopData="favorite.transportStopData"/>
            <div
              class="favorite-loading"
              v-if="isNextPassagesLoading(favorite.id)">
              <SpinnerProgress visible/>
            </div>
            <ErrorMessage
              text="Aucun horaire trouvé"
              :visible="shouldShowNoResultMessage(favorite.id)"
            />
            <TimeTableWindow 
              hideDestinationTrainSelection
              @closeClick="closeTimeTable(favorite.id)"
              :nextPassages="getNextPassages(favorite.id)"/>
          </div>
        </div>
    </TransportTimeList>
    <ChangeFavoriteGroupNameModal 
      :visible="modal.visible" 
      :actualGroupName="modal.actualGroupName"
      :existingGroupNames="groupNames"
      @validateClick="changeGroupName"
      @cancelClick="hideChangeGroupNameModal"/>
  </div>
</template>

<script>
import LineAndStopPresentation from "@/components/common/LineAndStopPresentation.vue";
import TransportTimeList from '@/components/common/TransportTimeList.vue';
import TimeTableWindow from "@/components/timeTableWindow/TimeTableWindow.vue";
import SpinnerProgress from '@/components/common/SpinnerProgress.vue';
import ChangeFavoriteGroupNameModal from '@/components/modals/ChangeFavoriteGroupNameModal.vue';
import ErrorMessage from "@/components/common/ErrorMessage.vue";

export default {
  components:{
    LineAndStopPresentation,
    TransportTimeList,
    TimeTableWindow,
    SpinnerProgress,
    ChangeFavoriteGroupNameModal,
    ErrorMessage
  },
  data() {
    return {
      favorites: [],
      isLoading: false,
      nextPassagesStore: [],
      isTouching: false,
      editMode: false,
      selectedFavoriteIds: [],
      groupNames: [],
      modal: {
        visible: false,
        actualGroupName: ''
      }
    };
  },
  created(){
    window.favoritesView = this;
    this.$phoneInterface.onFavoriteAdded = () => {
      window.favoritesView.loadView();
    }
    this.loadView();
  },
  beforeDestroy(){
    this.$phoneInterface.onFavoriteAdded = null
  },
  watch:{
    selectedFavoriteIds(){
      this.editMode = this.selectedFavoriteIds.length > 0;

      if(this.editMode)
        this.nextPassagesStore = [];
      else{
        this.saveChanges();
      }

      this.$emit('favoriteSelected', this.selectedFavoriteIds.length);
    }
  },
  computed:{
    isEmptyList(){
      return !this.isLoading && this.favorites.length === 0;
    }
  },
  methods:{
    loadView(){
      
      this.isLoading = true;
      this.$phoneInterface.getFavorites((e) => {
        window.favoritesView.favorites = e.result;
        window.favoritesView.isLoading = false;
        window.favoritesView.initGroupNames();
      });
    },
    initGroupNames(){
      let distinctGroupNames = [...new Set(this.favorites.map(favorite => favorite.groupName))];
      const defaultGroupIndex = distinctGroupNames.indexOf("Autre");

      if(defaultGroupIndex >= 0 && defaultGroupIndex !== distinctGroupNames.length - 1){
        distinctGroupNames = distinctGroupNames.filter(groupName => groupName !== "Autre");
        distinctGroupNames.push('Autre');
      }
      
      this.groupNames = distinctGroupNames;
    },
    getFavoritesByGroupName(groupName){
      const favorites = this.favorites.filter(favorite => favorite.groupName === groupName);
      favorites.sort((fav1, fav2) => fav1.indexInGroup - fav2.indexInGroup);
      return favorites;
    },
    loadTimeTable(favoriteId){
      const nextPassagesItem = this.getNextPassagesStoreItem(favoriteId);
      if(!nextPassagesItem)
        this.nextPassagesStore.push(
          {
            favoriteId: favoriteId,
            loading: true,
            nextPassages: [],
            showNoResultMessage: false
          }
        )
      else {
        nextPassagesItem.loading = true;
        nextPassagesItem.nextPassages = [];
        nextPassagesItem.showNoResultMessage = false;
      }

      //if this is the last element of the list, scroll to the bottom of the page otherwise the loading spinner is hidden behind nav bar
      if(this.favorites.findIndex(f => f.id === favoriteId) === this.favorites.length - 1)
        this.scrollToBottom();
      
      window.favoriteIdToUpdate = favoriteId;
      this.$phoneInterface.getNextPassagesForFavoriteAsync(favoriteId, (e)=>{
        const favorite = window.favoritesView.favorites.find(fav => fav.id === window.favoriteIdToUpdate);
        const nextPassagesItemToUpdate = window.favoritesView.getNextPassagesStoreItem(window.favoriteIdToUpdate);
        nextPassagesItemToUpdate.loading = false;
        if(e.result && e.result.length > 0){
           nextPassagesItemToUpdate.nextPassages = e.result.map(r => 
            ({  
                ...r, 
                stopArea:{
                  id: favorite.transportStopData.stopAreaId,
                  name: favorite.transportStopData.stopAreaName
                },
                destinationStopArea:{
                  id: favorite.transportStopData.destinationStopAreaId,
                  name: favorite.transportStopData.destinationStopAreaName
                },
            }));
        }
        else
          nextPassagesItemToUpdate.showNoResultMessage = true;
      });
    },
    getNextPassagesStoreItem(favoriteId){
      return this.nextPassagesStore.find(n => n.favoriteId === favoriteId);
    },
    getNextPassages(favoriteId){
      return this.getNextPassagesStoreItem(favoriteId)?.nextPassages;
    },
    isNextPassagesLoading(favoriteId){
      return this.getNextPassagesStoreItem(favoriteId)?.loading;
    },
    shouldShowNoResultMessage(favoriteId){
      return this.getNextPassagesStoreItem(favoriteId)?.showNoResultMessage;
    },
    closeTimeTable(favoriteId){
      const nextPassagesItem = this.getNextPassagesStoreItem(favoriteId);
      nextPassagesItem.nextPassages = [];
    },
    scrollToBottom() {
      setTimeout(()=> {
        const activityContainer = document.getElementsByClassName(
        "activity-view-scrollable-content"
        )[0];
        activityContainer.scrollTo({
          top: 50000, //set very high value to be sure to go to the bottom
          behavior: "smooth",
      });
      }, 200);
    },
    onTouchStart(favoriteId){
      if(this.editMode)
        return;

      this.isTouching = true;
      const self = this;
      setTimeout(()=>{
        if(self.isTouching){
          self.selectedFavoriteIds.push(favoriteId)
        }
      }, 1000);
    },
    onTouchEnd(){
      this.isTouching = false;
    },
    toggleFavoriteSelection(favoriteId){
      if(this.selectedFavoriteIds.includes(favoriteId))
        this.selectedFavoriteIds = this.selectedFavoriteIds.filter(f => f !== favoriteId);
      else
        this.selectedFavoriteIds.push(favoriteId)
    },
    deleteSelectedFavorites(){
      this.favorites = this.favorites.filter(fav => !this.selectedFavoriteIds.includes(fav.id));
      this.selectedFavoriteIds = [];
    },
    moveFavoriteAbove(){
      const selectedFavorite = this.favorites.find(fav => fav.id === this.selectedFavoriteIds[0]);

      if(selectedFavorite.indexInGroup === 0){
        const groupNameIndex = this.groupNames.findIndex(groupName => groupName === selectedFavorite.groupName);
        if(groupNameIndex !== 0){
          const favoritesInGroup = this.getFavoritesByGroupName(this.groupNames[groupNameIndex - 1]);
          const maxIndexInGroup = favoritesInGroup.reduce((acc, cur) => {
            return cur.indexInGroup > acc ? cur.indexInGroup : acc;
          }, 0);

          selectedFavorite.groupName = this.groupNames[groupNameIndex - 1];
          selectedFavorite.indexInGroup = maxIndexInGroup + 1;
        }
      }
      else {
        const favoriteAbove = this.favorites.find(fav => fav.indexInGroup === selectedFavorite.indexInGroup - 1 && fav.groupName === selectedFavorite.groupName);
        favoriteAbove.indexInGroup += 1;
        selectedFavorite.indexInGroup -= 1;
      }
        
    },
    moveFavoriteBelow(){
      const selectedFavorite = this.favorites.find(fav => fav.id === this.selectedFavoriteIds[0]);
      const favoritesInGroup = this.getFavoritesByGroupName(selectedFavorite.groupName);
      const maxIndexInGroup = favoritesInGroup.reduce((acc, cur) => {
            return cur.indexInGroup > acc ? cur.indexInGroup : acc;
          }, 0);

      if(selectedFavorite.indexInGroup === maxIndexInGroup){
         const groupNameIndex = this.groupNames.findIndex(groupName => groupName === selectedFavorite.groupName);
        if(groupNameIndex !== this.groupNames.length - 1){     
          const favoritesInNewGroup = this.getFavoritesByGroupName(this.groupNames[groupNameIndex + 1]);
          favoritesInNewGroup.forEach(fav => fav.indexInGroup += 1);

          selectedFavorite.groupName = this.groupNames[groupNameIndex + 1];
          selectedFavorite.indexInGroup = 0;
        }
      }
      else {
        const favoriteBelow = this.favorites.find(fav => fav.indexInGroup === selectedFavorite.indexInGroup + 1 && fav.groupName === selectedFavorite.groupName);
        favoriteBelow.indexInGroup -= 1;
        selectedFavorite.indexInGroup += 1;
      }
    },
    endEditMode(){
      this.selectedFavoriteIds = [];
    },
    showChangeGroupNameModal(groupName){
      if(groupName !== 'Autre'){
        this.modal.visible = true;
        this.modal.actualGroupName = groupName;
      }
    },
    changeGroupName(newGroupName){
      this.favorites.forEach(fav =>{
        if(fav.groupName === this.modal.actualGroupName)
          fav.groupName = newGroupName
      });
      this.saveChanges();
      this.hideChangeGroupNameModal();
    },
    hideChangeGroupNameModal(){
      this.modal.visible = false;
      this.modal.actualGroupName = '';
    },
    saveChanges(){
      const isUpdateSuccess = this.$phoneInterface.updateFavoritesList(this.favorites);
      if(isUpdateSuccess)
        this.initGroupNames();
      else
        this.loadView();
    }
  }
};
</script>

<style scoped>
.favorites-view{
  height: 100%;
}

.favorite-group {
  width: 100%;
  background-color: var(--light-grey);
  padding: 5px 7px;
}

.favorite-container{
  padding: 15px 7px;
  border-bottom: 1px solid #dfdfdf;
  background-color: white;
}

.favorite-container.selected {
  background: linear-gradient(90deg, rgba(2,0,36,0) 0%, var(--green-ratp) 35%, var(--blue-ratp) 100%);
}

.favorite-loading{
  height: 90px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>