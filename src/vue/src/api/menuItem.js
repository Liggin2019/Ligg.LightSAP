import request from '@/utils/request'

export function getMenuItems(params) {
  return request({
    url: '/Sys/MenuItem/GetListModels',
    method: 'get',
    params: { portal:params.portalKey,menuId: params.menuId}
  })
}
