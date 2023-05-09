import { login, logoff, getCurrentUser } from '@/api/user'
import { getTokenCooky, setTokenCooky, removeTokenCooky, getLanguageCooky, setLanguageCooky, getTenantCooky, setTenantCooky, getPortalCooky, setPortalCooky } from '@/utils/handleCookies'
import { resetRouter } from '@/router'
import { isEmptyString } from '@/utils/validate'


const getDefaultState = () => {
  return {
    token: getTokenCooky(),
    account: '',
    userName: '',
    userId: '',
    userHasThumbnail: false,
    languageKey: getLanguageCooky(),
    tenantCode: getTenantCooky(),
    portalKey: getPortalCooky()
  }
}
const state = getDefaultState()

const mutations = {
  RESET_STATE: (state) => {
    Object.assign(state, getDefaultState())
  },
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_ACCOUNT: (state, account) => {
    state.account = account
  },
  SET_USERNAME: (state, userName) => {
    state.userName = userName
  },
  SET_USERID: (state, userId) => {
    state.userId = userId
  },
  SET_USERHASTHUMBNAIL: (state, has) => {
    state.userHasThumbnail = has
  },
  SET_AVATAR: (state, avatarUrl) => {
    state.avatarUrl = avatarUrl
  },
  SET_LANGUAGE: (state, key) => {
    state.languageKey= key
  },
  SET_TENANT: (state, code) => {
    state.tenantCode = code
  },
  SET_PORTAL: (state, key) => {
    state.portalKey = key
  },
}

const actions = {
  // user login
  login({ commit }, loginInfo) {
    const { account, password, languageKey, tenantCode, portalKey} = loginInfo
    return new Promise((resolve, reject) => {
      login({account: account.trim(), password: password }).then(response => {
        const rst = response
        commit('SET_TOKEN', rst.message)
        setTokenCooky(rst.message)
        commit('SET_LANGUAGE', languageKey)
        setLanguageCooky(languageKey)
        if (isEmptyString(tenantCode)) {
          tenantCode= 'public'
        }
        commit('SET_TENANT', tenantCode)
        setTenantCooky(tenantCode)
        commit('SET_PORTAL', portalKey)
        setPortalCooky(portalKey)
        // store.dispatch('portal/setCurrent')
        // store.dispatch('tenant/setCurrent')
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // get user info
  getCurrent({ commit, state }) {
    return new Promise((resolve, reject) => {
      getCurrentUser(state.token).then(response => {
        const rst = response
        if(rst.flag===0){reject(rst.message)}
        const data=rst.data
        if (!data) {
          return reject('Verification failed, please Login again.')
        }
        commit('SET_ACCOUNT', data.account)
        commit('SET_USERNAME', data.name)
        commit('SET_USERID', data.actorId) 
        commit('SET_USERHASTHUMBNAIL', !isEmptyString(data.thumbnailPostfix)) 
        resolve(data)
      }).catch(error => {
        reject(error)
      })
    })
  },

  // user logout
  logoff({ commit, state }) {
    return new Promise((resolve, reject) => {
      logoff(state.token).then(() => {
        removeTokenCooky() // must remove  token  first
        resetRouter()
        commit('RESET_STATE')
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      removeTokenCooky() // must remove  token  first
      commit('RESET_STATE')
      resolve()
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}

