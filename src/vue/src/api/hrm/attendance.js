import request from '@/utils/request'

export function ImportAttendances(orgId,data) {
    return request({
      url: '/Hrm/Attendance/AddMany',
      method: 'post',
      params: { orgId:orgId},
      data: data
    })
}

export function GetPagedApproveAttendances(params) {
  return request({
    url: '/Hrm/Attendance/GetPagedApproveModels',
    method: 'get',
    params: { month:params.month}
  })
}

export function ApproveSelectedAttendances(ids,data) {
  return request({
    url: '/Hrm/Attendance/Approve',
    method: 'post',
    params: { ids:ids.ids},
    data: data
  })
}
