<template>
    <div class="stop-area-label">{{ text }}</div>
</template>
 
<script>
export default {
    name: 'StopAreaLabel',
    props: {
        text: String
    }
};
</script>

<style scoped>
.stop-area-label{
    background-color: var(--deepblue-ratp);
    color: white;
    font-weight: 600;
    text-align: center;
    padding: 7px;
    width: 100%;
}
</style>