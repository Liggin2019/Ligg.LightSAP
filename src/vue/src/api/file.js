import request from '@/utils/request'

export function getImage(data) {
  return request({
    url: 'File/GetImage',
    method: 'get',
    params: { index:data.index,obj:data.obj,objId:data.objId }
  })
}

