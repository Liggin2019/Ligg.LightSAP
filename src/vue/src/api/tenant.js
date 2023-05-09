import request from '@/utils/request'

export function getCurrentTenant(code) {
  return request({
    url: 'Sys/Tenant/getCurrentTenant',
    method: 'get',
    params: { code:code }
  })
}

