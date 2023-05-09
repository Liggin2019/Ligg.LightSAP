
import { getCurrentPortal} from '@/utils/get-static-data'

const state = {
  currentPortal: {},
}
const mutations = {
  SET_CURRENTPORTAL: (state, data) => {
    state.currentPortal=data
  },
}
const actions = {
  setCurrent({ commit }) {
    const data=getCurrentPortal().data
    commit('SET_CURRENTPORTAL', data)
  }
}



export default {
  namespaced: true,
  state,
  mutations,
  actions
}
