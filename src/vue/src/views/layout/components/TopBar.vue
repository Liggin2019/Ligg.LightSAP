<template>
  <div class="topbar">
    <logo v-if="showLogo" :collapse="isCollapse" />
    <div class="top-menu" />

    <div>
      <span style="display:block; color:#ccc;margin-top:20px;margin-right:5px;font-size: 14px;">
        {{ userTip }}
      </span>
    </div>
    <div class=" top-split-button">
      <div class="right-menu">
        <el-dropdown class="menu-container" trigger="click">
          <div class="icon-wrapper">
            <img :src="getAvatarUrl()" class="icon">
            <i class="el-icon-caret-bottom" style="color:#ffffff;" />
          </div>
          <el-dropdown-menu slot="dropdown" class="user-dropdown">
            <router-link to="/">
              <el-dropdown-item>
                个人中心
              </el-dropdown-item>
            </router-link>
            <!-- <a target="_blank" href="">
              <el-dropdown-item>Preference</el-dropdown-item>
            </a> -->
            <el-dropdown-item divided @click.native="logoff">
              <span style="display:block;">退出登录</span>
            </el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </div>
    <div class="top-split-button">
      <div class="right-menu">
        <el-dropdown class="menu-container" trigger="click">
          <div class="icon-wrapper">
            <img :src="getHelpImageUrl()" class="icon">
            <i class="el-icon-caret-bottom" style="color:#ffffff;" />
          </div>
          <el-dropdown-menu slot="dropdown" class="user-dropdown">
            <a target="_blank" href="https://www.github.com/liggin2019">
              <el-dropdown-item>Github</el-dropdown-item>
            </a>
            <a target="_blank" href="https://www.gitee.com/liggin2019">
              <el-dropdown-item>Gitee</el-dropdown-item>
            </a>
            <el-dropdown-item divided @click.native="aboutus">
              <span style="display:block;">About Us</span>
            </el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </div>

  </div>
</template>

<script>
import API_CONFIG from '../../../../vue.config.js'
import store from '@/store'
import { mapGetters } from 'vuex'
import path from 'path'
import { isExternal } from '@/utils/validate'
import variables from '@/styles/variables.scss'
import Logo from './Logo'
import {getImage } from '@/api/file'

export default {
  components: { Logo },
  data() {
    return {
      userTip: store.getters.userName+", welcome" ,
    }
  },
  computed: {
    ...mapGetters([
      'permission_routes',
      'sidebar'
    ]),
    activeMenu() {
      const route = this.$route
      const { meta, matched } = route
      if (meta.activeMenu) {
        return meta.activeMenu
      }
      store.dispatch('permission/changeSecondRoutes', matched[0])
      return matched[0].path || '/'
    },
    showLogo() {
      return this.$store.state.settings.sidebarLogo
    },
    variables() {
      return variables
    },
    isCollapse() {
      return !this.sidebar.opened
    }
  },
  methods: {
    getAvatarUrl() {
      var target=API_CONFIG.devServer.proxy[process.env.VUE_APP_BASE_API].target
      var userId=store.getters.userId;
      var userHasThumbnail=store.getters.userHasThumbnail;
      var imageUrl=require('@/assets/images/' + 'user.png')
      if(userHasThumbnail)
      imageUrl=target+'/File/GetImage?index=attachment&obj=userthumbnail&objId='+userId;
      return imageUrl
    },
    getHelpImageUrl() {
      return require('@/assets/images/' + 'help.png')
    },
    resolvePath(route) {
      if (isExternal(route.path)) {
        return route.path
      }
      if (isExternal(route.redirect)) {
        return route.redirect
      }
      return route.path
    },
    jumpTo(route) {
      if (isExternal(route.path)) {
        window.open(route.path, '_blank')
        return
      }
      if (isExternal(route.redirect)) {
        window.open(route.redirect, '_blank')
        return
      }
      this.$router.push(path.resolve(route.path, route.redirect || ''))
    },
    async logoff() {
      await this.$store.dispatch('user/logoff')
      //alert(${this.$route.fullPath});
      this.$router.push(`/login?redirect=/`)
      //this.$router.push(`/login?redirect=${this.$route.fullPath}`)
    }
  }
}
</script>

<style lang="scss" scoped>
.topbar {
  height: 50px;
  // overflow: hidden;
  // position: relative;
  // background: #fff;
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);

  .right-menu {
    float: right;
    height: 100%;
    line-height: 50px;

    &:focus {
      outline: none;
    }

    .right-menu-item {
      display: inline-block;
      padding: 0 8px;
      height: 100%;
      font-size: 18px;
      color: #5a5e66;
      vertical-align: text-bottom;

      &.hover-effect {
        cursor: pointer;
        transition: background 0.3s;

        &:hover {
          background: rgba(0, 0, 0, 0.025);
        }
      }
    }

    .menu-container {
      margin-right: 30px;

      .icon-wrapper {
        margin-top: 9px;
        position: relative;

        .icon {
          cursor: pointer;
          width: 36px;
          height: 36px;
          border-radius: 10px;
        }

        .el-icon-caret-bottom {
          cursor: pointer;
          position: absolute;
          right: -20px;
          top: 25px;
          font-size: 12px;
        }
      }
    }
  }
}
</style>
