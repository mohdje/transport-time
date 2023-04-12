import Vue from 'vue';

const VueDirectives = {
    import(){
        Vue.directive('delay-click', {
            inserted(el, binding) {
              el.addEventListener('click', 
                () => { 
                  setTimeout(() => {
                     binding.value() 
                  }, 300); 
                });
            }
          });

          Vue.directive('delay-click-stop', {
            inserted(el, binding) {
              el.addEventListener('click', 
                (e) => { 
                  setTimeout(() => {
                     binding.value() 
                  }, 300); 
                  e.stopPropagation() 
                });
            }
          });
    }
}

export default VueDirectives;