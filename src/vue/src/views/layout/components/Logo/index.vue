<template>
  <div class="logo-container" :class="{'collapse':collapse}">
    <transition name="logoFade">
      <router-link v-if="collapse" key="collapse" class="logo-link" to="/">
        <img v-if="logo" :src="logo" class="logo-icon">
        <h1 v-else class="logo-title">{{ title }} </h1>
      </router-link>
      <router-link v-else key="expand" class="logo-link" to="/">
        <img v-if="logo" :src="logo" class="logo-icon">
        <h1 class="logo-title">{{ title }} </h1>
      </router-link>
    </transition>
  </div>
</template>

<script>
// import { getAppInfo } from '@/utils/get-functions'
import store from '@/store'
import API_CONFIG from '../../../../../vue.config'
import { isEmptyString } from '@/utils/validate'

export default {
  name: 'Logo',
  props: {
    collapse: {
      type: Boolean,
      required: true
    }
  },
  data() {
    return {
      title: '',
      logo: ''
    }
  },
  mounted() {
    this.init()
  },
  methods: {
    init() {

      this.title = 'appInfo.data.appName' + '--' + 'appInfo.data.appDes'
      this.title = store.getters.currentTenant.key + ' ' + (store.getters.currentPortal.key).toUpperCase() +"   "+ store.getters.currentPortal.description
      this.logo = this.getTenantThumbnailUrl() 
    },

    getTenantThumbnailUrl() {
      const tenantId = store.getters.currentTenant.id
      var thumbnailPostfix=store.getters.currentTenant.thumbnailPostfix;
      var hasThumbnail=true;
      if(isEmptyString(thumbnailPostfix)) hasThumbnail=false;
      var imageUrl=require('@/assets/images/' + 'tenant.png')
      if(hasThumbnail&tenantId!=0)
      {
        var target=API_CONFIG.devServer.proxy[process.env.VUE_APP_BASE_API].target
        imageUrl=target+'/File/GetImage?index=attachment&obj=tenantthumbnail&objId='+tenantId;
      }
      
      return imageUrl
    },
  }
}
</script>

<style lang="scss" scoped>
.logoFade-enter-active {
  transition: opacity 1.5s;
}

.logoFade-enter,
.logoFade-leave-to {
  opacity: 0;
}

.logo-container {
  position: relative;
  height: 50px;
  line-height: 50px;
  background: #2b2f3a;
  text-align: center;
  overflow: hidden;

  & .logo-link {
    height: 100%;
    width: 100%;

    & .logo-icon {
      width: 32px;
      height: 32px;
      vertical-align: middle;
      margin-right: 3px;
      border-radius: 16px;
    }

    & .logo-title {
      display: inline-block;
      margin: 0;
      color: #fff;
      font-weight: 600;
      line-height: 50px;
      font-size: 14px;
      font-family: Avenir, Helvetica Neue, Arial, Helvetica, sans-serif;
      vertical-align: middle;
    }
  }

  &.collapse {
    .logo-icon {
      margin-right: 0px;
    
    }
  }
}
</style>
