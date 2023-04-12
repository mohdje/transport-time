<template>
  <div class="activity-view-container">
    <div class="acitivity-header">
      <h2>
        {{ activeNavigationButton.label }}
      </h2>
      <div class="left-actions">
        <v-btn
          v-for="(headerLeftAction, index) in headerLeftActions"
          :key="index"
          color="white"
          icon
          @click="headerLeftAction.onClick"
          >
            <v-icon>{{ headerLeftAction.icon }}</v-icon>
        </v-btn>
      </div>
      <div class="right-actions">
        <v-btn
          v-for="(headerRightAction, index) in headerRightActions"
          :key="index"
          color="white"
          icon
          @click="headerRightAction.onClick"
          >
            <v-icon>{{ headerRightAction.icon }}</v-icon>
        </v-btn>
      </div>
    </div>
    <div class="activity-view-scrollable-content">
      <SearchView v-if="bottomNavigation.selectedButton === 0" />
      <FavoritesView v-else-if="bottomNavigation.selectedButton === 1" 
        ref="favoritesView"
        @favoriteSelected="onFavoriteSelected"/>
      <ScheduledNotificationsView v-else-if="bottomNavigation.selectedButton === 2" />
    </div>
    <div class="activity-footer">
      <v-bottom-navigation
        v-model="bottomNavigation.selectedButton"
        color="primary"
        grow
        height="100%"
      >
        <v-btn
          v-for="button in bottomNavigation.buttons"
          :key="button.label"
          height="100%"
        >
          <span>{{ button.label }}</span>
          <v-icon>{{ button.icon }}</v-icon>
        </v-btn>
      </v-bottom-navigation>
    </div>
    <NotificationWarningModal 
      :visible="showNotificationWarningModal" 
      @cancelClick="showNotificationWarningModal = false" />
  </div>
</template>
<script>
import SearchView from "@/components/views/SearchView.vue";
import ScheduledNotificationsView from "@/components/views/ScheduledNotificationsView.vue";
import NotificationWarningModal from "@/components/modals/NotificationWarningModal.vue";
import FavoritesView from '@/components/views/FavoritesView.vue';

export default {
  name: 'MainActivity',
  components: { 
    SearchView, 
    ScheduledNotificationsView,
    NotificationWarningModal,
    FavoritesView
  },
  computed: {
    activeNavigationButton() {
      return this.bottomNavigation.buttons[this.bottomNavigation.selectedButton];
    },
    headerLeftActions(){
      return this.headers.find(ha => ha.navButtonId === this.bottomNavigation.selectedButton)?.leftActions;
    },
    headerRightActions(){
      return this.headers.find(ha => ha.navButtonId === this.bottomNavigation.selectedButton)?.rightActions;
    }
  },
  data() {
    return {
      showNotificationWarningModal: false,
      headers:[
        {
          navButtonId: 2,
          rightActions:[
            {
              icon: 'mdi-information-outline',
              onClick: () => { this.showNotificationWarningModal = true }
            }
          ]
        }
      ],
      bottomNavigation: {
        selectedButton: 0,
        buttons: [
          {
            label: "Recherche",
            icon: "mdi-magnify"
          },
          {
            label: "Favoris",
            icon: "mdi-heart"
          },
          {
            label: "Planifié",
            icon: "mdi-timer-outline"
          },
        ],
      },
    };
  },
  methods:{
    onFavoriteSelected(nbFavoritesSelected){
      //remove favorite header to replace it with the one with new actions
      this.headers = this.headers.filter(header => header.navButtonId !== 1);
      
      const favoriteHeader = {
        navButtonId: 1,
        rightActions: [],
        leftActions: []
      };

      if(nbFavoritesSelected === 1){
        favoriteHeader.rightActions.push({
          icon: 'mdi-arrow-up',
          onClick: () => { this.$refs.favoritesView.moveFavoriteAbove() }
        });
        favoriteHeader.rightActions.push({
          icon: 'mdi-arrow-down',
          onClick: () => { this.$refs.favoritesView.moveFavoriteBelow() }
        })
      }

      if(nbFavoritesSelected >= 1){
        favoriteHeader.leftActions.push({
            icon: 'mdi-check',
            onClick: () => { this.$refs.favoritesView.endEditMode() }
          });
         favoriteHeader.rightActions.push({
          icon: 'mdi-trash-can-outline',
          onClick: () => { this.$refs.favoritesView.deleteSelectedFavorites() }
        })

      }
    
      this.headers.push(favoriteHeader);
    }
  }
};
</script>
    
<style src="@/styles/activity.css"></style>
<style scoped>
.acitivity-header {
  background-color: var(--green-ratp);
  color: white;
  position: relative;
}

.acitivity-header .right-actions {
  position: absolute;
  right: 1%;
  display: flex;
}

.acitivity-header .left-actions {
  position: absolute;
  left: 1%;
  display: flex;
}

.activity-footer {
  background-color: white;
  border-top: 1px solid #edededad;
}

.activity-footer .v-btn {
  background-color: white !important;
}

.activity-footer .v-item-group.v-bottom-navigation {
  box-shadow: none !important;
  border: none !important;
}

.test {
  width: 100%;
  background-color: white;
}

.test div {
  margin-top: 70px;
  width: 100%;
  text-align: center;
  font-size: 20px;
  color: grey;
}
</style>