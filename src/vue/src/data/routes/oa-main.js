const routes = [
  {
    path: '/',
    component: 'Layout',
    redirect: '/dashboard',
    name: 'Home',
    children: [{
      path: '/dashboard',
      name: 'Dashboard',
      component: 'views/dashboard/index',
      meta: { title: 'Dashboard', icon: 'dashboard' }
    }
    ]
  },

  {
    path: '/document',
    component: 'Layout',
    redirect: '/ads/banner',
    name: 'document',
    meta: { title: 'Document', icon: 'el-icon-help' },
    children: [
      {
        path: '/document/KnowledgeLib',
        name: 'KnowledgeLib',
        component: 'views/dashboard/index',
        meta: { title: 'Knowledge Lib', icon: 'tree' }
      },
      {
        path: '/document/file',
        name: 'document-file',
        component: 'views/dashboard/index',
        meta: { title: 'File', icon: 'table' }
      },
      {
        path: '/document/netdisk',
        name: 'netdisk',
        component: 'views/404',
        meta: { title: 'Net Disk', icon: 'form' }
      }
    ]
  },

  {
    path: '/Workflow',
    component: 'Layout',
    redirect: '/Workflow/mtn',
    name: 'Workflow',
    meta: { title: 'Workflow', icon: 'el-icon-document' },
    children: [
      {
        path: '/Workflow/mtn',
        name: 'Workflow-mtn',
        component: 'views/dashboard/index',
        meta: { title: 'Workflow Mtn.', icon: 'el-icon-document' }
      },
      {
        path: '/Workflow/mngmt',
        name: 'Workflow-mngmt',
        component: 'views/404',
        meta: { title: 'Workflow Mgmt.', icon: 'el-icon-document' }
      }
    ]
  },

  {
    path: '/assert',
    component: 'Layout',
    redirect: '/assert/mngmt',
    name: 'assert',
    meta: { title: 'Assert', icon: 'el-icon-s-data' },
    children: [
      {
        path: '/assert/mngmt',
        name: 'assert-mngmt',
        component: 'views/dashboard/index',
        meta: { title: 'Assert Mgmt.', icon: 'el-icon-s-data' }
      },
      {
        path: '/assert/report',
        name: 'assert-report',
        component: 'views/dashboard/index',
        meta: { title: 'Assert Report', icon: 'el-icon-s-data' }
      }
    ]
  },
  {
    path: '/resource',
    component: 'Layout',
    redirect: '/resource/mngmt',
    name: 'aresource',
    meta: { title: 'Resource', icon: 'el-icon-s-operation' },
    children: [
      {
        path: '/resource/mngmt',
        name: 'resource-mngmt',
        component: 'views/dashboard/index',
        meta: { title: 'Resource Mgmt.', icon: 'el-icon-s-operation' }
      },
      {
        path: '/resource/report',
        name: 'resource-report',
        component: 'views/dashboard/index',
        meta: { title: 'Resource Report', icon: 'el-icon-s-operation' }
      }
    ]
  },
  {
    path: '/project',
    component: 'Layout',
    redirect: '/project/mngmt',
    name: 'project',
    meta: { title: 'Project', icon: 'el-icon-setting' },
    children: [
      {
        path: '/project/mngmt',
        name: 'project-mngmt',
        component: 'views/dashboard/index',
        meta: { title: 'project Mgmt.', icon: 'el-icon-setting' }
      },
      {
        path: '/project/report',
        name: 'project-report',
        component: 'views/404',
        meta: { title: 'project Report', icon: 'el-icon-setting' }
      }
    ]
  },
  {
    path: '/setting',
    component: 'Layout',
    redirect: '/setting/user',
    name: 'Setting',
    meta: { title: 'Setting', icon: 'el-icon-s-tools' },
    children: [
      {
        path: '/setting/user',
        name: 'User',
        component: 'views/dashboard/index',
        meta: { title: 'User', icon: 'el-icon-s-tools' }
      },
      {
        path: '/setting/role',
        name: 'Role',
        component: 'views/dashboard/index',
        meta: { title: 'Role', icon: 'el-icon-s-tools' }
      },
      {
        path: '/setting/menu',
        name: 'Menu',
        component: 'views/dashboard/index',
        meta: { title: 'Menu', icon: 'el-icon-s-tools' }
      },
      {
        path: '/setting/log',
        name: 'Log',
        component: 'views/dashboard/index',
        meta: { title: 'Log', icon: 'el-icon-s-tools' }
      }
    ]
  },

  {
    path: 'external-link',
    component: 'Layout',
    children: [
      {
        path: 'https://panjiachen.gitee.io/vue-element-admin-site/zh/',
        meta: { title: 'Help', icon: 'el-icon-link' }
      }
    ]
  }
]

module.exports =
  {
    data: routes
  }

