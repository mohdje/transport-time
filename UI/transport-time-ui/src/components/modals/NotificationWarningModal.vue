<template>
  <ModalDialog
    :visible="visible"
    title="Notifications"
    validButtonLabel="Accéder"
    @outsideClick="cancelClick"
    @cancelClick="cancelClick"
    @validateClick="openSettingsClick">
    <p>
      Pour certaines versions d'Android, il est nécessaire d'activer l'option
      Démarrage Automatique dans les réglages de l'application pour que cette
      fonctionnalité fonctionne. Pour cela cliquez sur le bouton ci-dessous pour
      accéder aux réglages et activez cette option.
    </p>
  </ModalDialog>
</template>

<script>
import ModalDialog from "@/components/common/ModalDialog.vue";
import { colorAppMixin } from "@/js/mixins.js";

export default {
  components: {
    ModalDialog
  },
  mixins: [colorAppMixin],
  props: {
    visible: Boolean,
  },
  methods: {
    openSettingsClick() {
      this.$phoneInterface.openAppSettings();
      this.cancelClick();
    },
    cancelClick() {
      this.$emit("cancelClick");
    },
  },
};
</script>
