import request from '@/utils/request'

export function login(data) { 
  var response=request({
    url: 'Sys/User/Logon',
    method: 'post',
    params: { account: data.account, password: data.password}
  })
  return  response;
}

export function getCurrentUser(token) {
  return request({
    url: 'Sys/User/getCurrentUser',
    method: 'get'
    // params: { token }
  })
}

export function logoff() {
  return request({
    url: 'Sys/User/Logoff',
    method: 'post'
  })
}
