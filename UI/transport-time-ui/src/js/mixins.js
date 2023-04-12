export const nextPassagesPerDirectionMixin = {
    methods: {
      nextPassagesPerDirection(nextPassages, nbNextPassagesPerDirection) {
        if (!nextPassages || nextPassages.length === 0) return [];

      //distinct directions from next passages
      const directions = nextPassages
        .map((nextPassage) => nextPassage.directionName)
        .filter((directionName, index, self) => self.indexOf(directionName) == index);

      const nextPassagesPerDirection = [];
      const self = this;
      directions.forEach((directionName) => {
        let nextPassagesFiltered = self.nextPassages.filter(
          (nextPassage) => nextPassage.directionName === directionName
        );
        nextPassagesPerDirection.push(...nextPassagesFiltered.slice(0, nbNextPassagesPerDirection));
      });
      return nextPassagesPerDirection;
      }
    }
  }

export const colorAppMixin = {
  methods:{
    getAppColor(colorName){
      return getComputedStyle(document.body).getPropertyValue('--' + colorName).replace(' ','');
    }
  }
};

export const timeFormatMixin = {
  methods:{
    getFormattedTime(hours, minutes){
      return hours.toString().padStart(2, "0") + ":" + minutes.toString().padStart(2, "0");
    }
  }
};

export const daysOfWeekMixin = {
  data(){
    return {
      daysOfWeek: [
        {
          name: "Lundi",
          index: 1,
        },
        {
          name: "Mardi",
          index: 2,
        },
        {
          name: "Mercredi",
          index: 3,
        },
        {
          name: "Jeudi",
          index: 4,
        },
        {
          name: "Vendredi",
          index: 5,
        },
        {
          name: "Samedi",
          index: 6,
        },
        {
          name: "Dimanche",
          index: 0,
        },
      ],
    }
  }
}