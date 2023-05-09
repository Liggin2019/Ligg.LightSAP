const routes = [

  // {
  //   path: '/1111',
  //   component: 'Layout',
  //   name: 'Nested111',
  //   meta: {
  //     title: 'Product Data',
  //     icon: 'table'
  //   },
  //   children: [
  //     {
  //       path: '12222',
  //       component: 'views/layout/components/empty',
  //       name: 'Menu1',
  //       meta: { title: 'Material', icon: 'el-icon-s-operation' },
  //       children: [
  //         {
  //           path: '12222333',
  //           component: 'views/portals/erp/demo',
  //           meta: { title: 'help21', icon: 'el-icon-s-operation' },
  //         },
  //         {
  //           path: '1222222223333333',
  //           component: 'views/portals/erp/demo1',
  //           meta: { title: 'help21', icon: 'el-icon-s-operation' },
  //         }
  //       ]
  //     }
  //   ]
  // },

  // {
  //   path: '/external-link',
  //   component: 'Layout',
  //   children: [
  //     {
  //       path: 'https://panjiachen.gitee.io/vue-element-admin-site/zh/',
  //       meta: { title: 'Help1', icon: 'el-icon-link' }
  //     }
  //   ]
  // },
  {
    path: '/external-link1',
    component: 'Layout',
    meta: { title: 'Help2', icon: '' },
    children: [
      {
        path: 'menu1kk',
        component: 'views/portals/erp/demo',
        meta: { title: 'help21', icon: 'el-icon-s-operation' },
      }
    ]
  }
]

module.exports =
  {
    data: routes
  }

