import { constantRoutes } from '@/router'
import Layout from '@/views/layout'
import { getMenuItems} from '@/api/menuItem'
import store from '@/store'

const state = {
  routes: [],
  addRoutes: []
}
const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  }
}
const actions = {
  getAccessibleRoutes({ commit, state }) {
    //if(!store.getters.currentPortal.menuId)
    store.dispatch('portal/setCurrent')
    store.dispatch('tenant/setCurrent')
    const menuId=store.getters.currentPortal.menuId
    const portalKey=store.getters.currentPortal.key
    return new Promise((resolve, reject) => {
      getMenuItems({portalKey:portalKey,menuId: menuId}).then(response => {
        var data=response.data
        const dynamicRoutes=transFormMenuItemsToRoutes(data,menuId)
        const availableRoutes = loopCreateRouter(dynamicRoutes)
        
        availableRoutes.push({ path: '*', redirect: '/404', hidden: true })
        commit('SET_ROUTES', availableRoutes)
        resolve(availableRoutes)
      }).catch(error => {
        reject(error)
      });
    });
  }
}


function transFormMenuItemsToRoutes(menuItems, menuId) {
  const menu=[];
  for (let i = 0;  i< menuItems.length; i++) {
    const menuItem={}
 
    if(menuItems[i].type==3)
    {
      let icon='';
      if(menuItems[i].icon) icon=menuItems[i].icon
      menuItem.meta={title:menuItems[i].name, icon:icon};
      menuItem.path=menuItems[i].url
      menuItem.id=menuItems[i].id
      menuItem.pid=menuItems[i].parentId
      menu.push(menuItem)
      continue;
    }

    if(menuItems[i].parentId===menuId) {
      if(menuItems[i].type!==0) continue
      if(menuItems[i].code.toLowerCase()=='dashboard') menuItem.path="/"
      else menuItem.path="/"+menuItems[i].code
      menuItem.component='Layout'
    }
    else {
      menuItem.path=menuItems[i].code
      if(menuItems[i].type===0) 
        menuItem.component='views/layout/components/Empty'
      else //view or outlink
        menuItem.component=menuItems[i].url;
    }

    menuItem.id=menuItems[i].id
    menuItem.pid=menuItems[i].parentId
    //menuItem.name=menuItems[i].code
    if(menuItems[i].redirect) menuItem.redirect=menuItems[i].redirect
    let icon='';
    if(menuItems[i].icon) icon=menuItems[i].icon
    menuItem.meta={title:menuItems[i].name, icon:icon};
    menu.push(menuItem)
  }
  const routes=arrayToTree(menu,menuId);
  console.log(routes);
  return routes;
}

  function arrayToTree(arr, parentId) {
        // 将所有对象存到 map 中
        const map = arr.reduce((prev, cur) => {
            // 注意：这里是浅拷贝，会修改原数组，当后续再使用原数组会出问题
            prev[cur.id] = cur;
            return prev;
        }, {});
        // 定义返回结果数组
        let result = [];
        // 遍历传入的对象
        for (let j = 0; j < arr.length; j++) {
            const item = arr[j];
            // 当遍历到的对象的 pid 等于 传入的根节点的 id，说明为根节点，直接 push 到数组中
            if (item.pid === parentId) {
                result.push(item);
                // 退出本次循环
                continue;
            }
            // 如果不是根节点，从 map 中找到 pid 对应的对象，该对象即为当前遍历到的对象的父节点
            const parent = map[item.pid];
            if (parent) {
                // 给父节点添加 children 属性,并定义为数组
                parent.children = parent.children || [];
                // 在数组中 push 当前子节点
                parent.children.push(item);
            }
        }
    return result;
  }


  function loopCreateRouter(routes) {
    const res = []
    routes.forEach(route => {
      const tmp = { ...route }
      const tpl = Object.assign({}, tmp)
      if (typeof tmp['component'] !== 'undefined' && tmp['component']) {
        if (tmp['component'] === 'Layout') {
          tpl['component'] = Layout
        } else {
          tpl['component'] = resolve => require([`../../${tmp['component']}.vue`], resolve)
        }
      }
      if (tmp['children']) {
        tpl['children'] = loopCreateRouter(tmp['children'])
      }
      res.push(tpl)
    })
    return res
  }

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
