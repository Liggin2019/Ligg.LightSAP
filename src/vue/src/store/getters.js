const getters = {
  sidebar: state => state.app.sidebar,
  device: state => state.app.device,
  token: state => state.user.token,
  avatarUrl: state => state.user.avatarUrl,
  account: state => state.user.account,
  userName: state => state.user.userName,
  userId: state => state.user.userId,
  userHasThumbnail: state => state.user.userHasThumbnail,
  languageKey: state => state.user.languageKey,
  tenantCode: state => state.user.tenantCode,
  portalKey: state => state.user.portalKey,
  menu_routes: state => state.menu.routes,
  currentPortal: state => state.portal.currentPortal,
  currentTenant: state => state.tenant.currentTenant
}
export default getters
