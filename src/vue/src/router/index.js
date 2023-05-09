import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/views/layout'
import store from '@/store'

export const constantRoutes = [
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },

  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true
  },
  // {
  //   path: '/',
  //   component: Layout,
  //   redirect: '/dashboard',
  //   children: [{
  //     path: 'dashboard',
  //     name: '首页',
  //     // component: () => import('@/views/portals/'+store.getters.currentPortal.key+'/dashboard/index'),
  //     component: () => import('@/views/portals/oa/dashboard/index'),
  //     // component: () => import(store.getters.currentPortal.key=='oa'?'@/views/portals/oa/dashboard/index':'@/views/portals/erp/dashboard/index'),
  //     meta: { title: '首页', icon: 'dashboard' }
  //   }]
  // },

  // {
  //   path: '/Test',
  //   component: Layout,
  //   name:'test1',
    
  //   meta: { title: 'tEST', icon: 'el-icon-s-operation' },
  //   children: [
  //     {
  //       path: 'Test',
  //       component: () => import('@/views/portals/custReport'),
  //       meta: { title: 'Test', icon: 'el-icon-s-operation' },
  //     }
  //   ]
  // }
]


const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes,
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
