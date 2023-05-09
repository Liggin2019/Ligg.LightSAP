import Vue from 'vue'
import Vuex from 'vuex'
import getters from './getters'
import app from './modules/app'
import settings from './modules/settings'
import user from './modules/user'
import menu from './modules/menu'
import portal from './modules/portal'
import tenant from './modules/tenant'

Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
    app,
    settings,
    user,
    menu,
    portal,
    tenant
  },
  getters
})

export default store
