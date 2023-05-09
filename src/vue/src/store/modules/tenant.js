
import store from '@/store'
import { getCurrentTenant} from '@/api/tenant'
const state = {
  currentTenant: {},
}
const mutations = {
  SET_CURRENTTENANT: (state, data) => {
    state.currentTenant=data
  },
}
const actions = {
  setCurrent({ commit, state }) {
    new Promise((resolve, reject) => {
      getCurrentTenant(store.getters.tenantCode).then(response => {
        const rst = response
        if(rst.flag===0){reject(rst.message)}
        const data=rst.data
        if (!data) {
          return reject('getCurrentTenant failed')
        }
        commit('SET_CURRENTTENANT', data)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
    
  },

}



export default {
  namespaced: true,
  state,
  mutations,
  actions
}
