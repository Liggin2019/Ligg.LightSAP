

export function validateAccount(str) {
  //const validate_map = ['root', 'ppp', 'qqq']
  return str.length>2
}

export function isEmptyString(str) {
  if (str === null || typeof str === 'undefined' || str.trim() === '') {
    return true
  } else {
    return false
  }
}

export function isExternal(path) {
  return /^(https?:|mailto:|tel:)/.test(path)
}
