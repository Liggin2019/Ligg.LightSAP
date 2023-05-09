import router from './router'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getTokenCooky } from '@/utils/handleCookies' // get token from cookie
import getPageTitle from '@/utils/get-page-title'
import { isEmptyString } from '@/utils/validate'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login'] // no redirect whitelist

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title

  //if(!to.meta.title)
  document.title = to.meta.title +'-'+ (store.getters.currentTenant.shortName)

  // determine whether the user has logged in
  const hasToken = getTokenCooky()
  if (hasToken) {
    if (to.path === '/login') {
      
      // if is logged in, redirect to the home page
      next({ path: '/' })
      NProgress.done()
    } else {
      const hasAccount=!isEmptyString(store.getters.account)
      // hasAccount= (store.getters.account) && (store.getters.account.length>0)
      if (hasAccount) {
        next()
      } else {
        try {
          // get user info
          await store.dispatch('user/getCurrent')
          const accessibleRoutes = await store.dispatch('menu/getAccessibleRoutes')
          router.addRoutes(accessibleRoutes)
          // hack method to ensure that addRoutes is complete
          // set the replace: true, so the navigation will not leave a history record.即不能回退
          next({ ...to, replace: true })
        } catch (error) {
          // remove token and go to login page to re-login
          await store.dispatch('user/resetToken')
          Message.error(error || 'Has Error')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  } else {
    /* has no token*/

    if (whiteList.indexOf(to.path) !== -1) {
      // in the free login whitelist, go directly
      next()
    } else {
      // other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
