import request from '@/utils/request'

export function GetUnavailableClientViewButtons(viewKey) {
  return request({
    url: '/Sys/Permission/GetUnavailableClientViewButtons',
    method: 'get',
    params: { view:viewKey}
  })
}
