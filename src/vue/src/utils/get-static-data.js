

import store from '@/store'

export function getPortals() {
  const ptls = require('@/data/portals.js')
  const ptlsData = ptls.data
  const availablePortals = []
  for (const val of ptlsData) {
    availablePortals.push(val)
  }

  return {
    code: 20000,
    data: availablePortals
  }
}


export function getCurrentPortal() {
  var portalKey = store.getters.portalKey;
  const ptls = require('@/data/portals.js')
  const ptlsData = ptls.data
  const currentPortal={}
  for (const val of ptlsData) {
      if (val.key === portalKey) {
        currentPortal.key = val.key
        currentPortal.menuId = val.menuId
        currentPortal.name = val.name
        currentPortal.description = val.description
        break
      }
  }
  return {
    code: 20000,
    data: currentPortal
  }
}
