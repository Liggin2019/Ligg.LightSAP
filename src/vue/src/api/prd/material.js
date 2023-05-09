import request from '@/utils/request'

export function GetPagedMaterialManageDtos(params) {
  return request({
    url: '/Prd/Material/GetPagedManageDtos',
    method: 'get',
    params: { mark:params.mark,text: params.text, pageSize: params.pageSize, pageIndex: params.pageIndex}
  })
}

export function AddMaterial(params,data) {
  return request({
    url: '/Prd/Material/Add',
    method: 'post',
    params: { type:params.type},
    data: data
  })
}


export function DeleteSelectedMaterials(params,data) {
  return request({
    url: '/Prd/Material/DeleteSelected',
    method: 'post',
    params: { ids:params.ids},
    data: data
  })
}
