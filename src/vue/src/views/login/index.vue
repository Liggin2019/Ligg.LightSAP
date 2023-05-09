<template>
  <div class="login-container">
    <el-form ref="loginForm" :model="loginForm" :rules="loginRules" class="login-form" auto-complete="on" label-position="left">
      <div class="title-container">
        <h3 class="title">用户登录</h3>
        <div style="width:120px" v-show="false">
          <el-select v-model="loginForm.languageKey" placeholder="简体中文">
            <el-option v-for="item in loginForm.languages" :key="item.key" :label="item.name" :value="item.key" />
          </el-select>
        </div>
      </div>

      <el-form-item prop="tenantCode" v-show="false">
        <span class="svg-container">
          <svg-icon icon-class="peoples" />
        </span>
        <el-input ref="tenantCode" v-model="loginForm.tenantCode" placeholder="pub" name="tenantCode" type="text" tabindex="1" auto-complete="on" />
      </el-form-item>

      <el-form-item prop="portalKey">
        <span class="svg-container">
          <svg-icon icon-class="table" />
        </span>
        <el-select v-model="loginForm.portalKey" placeholder="ERP">
          <el-option v-for="item in loginForm.portals" :key="item.key" :label="item.key+'-'+item.name" :value="item.key" stle="width:400px" />
        </el-select>
      </el-form-item>

      <!-- <el-form-item prop="accountType">
        <span class="svg-container">
          <svg-icon icon-class="link" />
        </span>
        <el-radio-group v-model="loginForm.accountType">
          <el-radio label="domain" disabled>Domain account</el-radio>
          <el-radio label="sys">System account</el-radio>
        </el-radio-group>
      </el-form-item> -->

      <el-form-item prop="account">
        <span class="svg-container">
          <svg-icon icon-class="user" />
        </span>
        <el-input ref="account" v-model="loginForm.account" placeholder="account" name="account" type="text" tabindex="1" auto-complete="on" />
      </el-form-item>

      <el-form-item prop="password">
        <span class="svg-container">
          <svg-icon icon-class="password" />
        </span>
        <el-input :key="passwordType" ref="password" v-model="loginForm.password" :type="passwordType" placeholder="Password" name="password" tabindex="2" auto-complete="on" @keyup.enter.native="handleLogin" />
        <span class="show-pwd" @click="showPassword">
          <svg-icon :icon-class="passwordType === 'password' ? 'eye' : 'eye-open'" />
        </span>
      </el-form-item>

      <el-button :loading="loading" type="primary" style="width:100%;margin-bottom:20px;" @click.native.prevent="handleLogin">登录</el-button>
    </el-form>
    <div class="tips" style=" width:460px; margin-left: auto;margin-right: auto">
      <h3>演示账号</h3>
      <p>账号: root, 密码: 123456, 根用户</p>
      <p>账号: admin, 密码: 123456, 角色: 系统管理员</p>
      <p>账号: user, 密码: 123456, 角色: 无</p>
      <p>账号: ppp, 密码: 123456, 角色: 物料管理员 考勤管理员</p>
      <p>账号: qqq, 密码: 123456, 角色: 物料主管 考勤主管</p>
      <p>账号: www, 密码: 123456, 以用户身份操作考勤管理 物料管理 </p>
    </div>

  </div>
</template>

<script>
import { validateAccount, isEmptyString } from '@/utils/validate'
import { getPortals } from '@/utils/get-static-data'

export default {
  name: 'Login',
  data() {
    const validateAccountEx = (rule, value, callback) => {
      if (!validateAccount(value)) {
        //if (value.length < 3) {
        callback(new Error('please enter the correct user name'))
      } else {
        callback()
      }
    }
    const validatePasswordEx = (rule, value, callback) => {
      if (value.length < 6) {
        callback(new Error('password can not be less than 6 digits'))
      } else {
        callback()
      }
    }

    return {
      loginForm: {
        account: 'root',
        password: '123456',
        tenantCode: 'public',
        accountType: 'sys',
        portalKey: '',
        portals: [],
        languageKey: 'zh-CN',

        languages: [{
          key: 'en-US',
          name: 'English'
        }, {
          key: 'zh-CN',
          name: '简体中文'
        }, {
          key: 'de',
          name: 'Deutsch'
        }]
      },
      loginRules: {
        account: [{ required: true, trigger: 'change', validator: validateAccountEx }],
        password: [{ required: true, trigger: 'change', validator: validatePasswordEx }]

      },
      loading: false,
      passwordType: 'password',
      redirect: undefined
    }
  },
  watch: {
    $route: {
      handler: function (route) {
        this.redirect = route.query && route.query.redirect
      },
      immediate: true
    }
  },
  mounted: function () {
    this.refreshPortal()
    document.title = "用户登录"
  },
  methods: {
    refreshPortal() {
      this.loginForm.portals = getPortals().data
      this.loginForm.portalKey = 'oa'

    },
    showPassword() {
      if (this.passwordType === 'password') {
        this.passwordType = ''
      } else {
        this.passwordType = 'password'
      }
      this.$nextTick(() => {
        this.$refs.password.focus()
      })
    },
    handleLogin() {
      this.$refs.loginForm.validate(valid => {
        if (valid) {
          this.loading = true
          this.$store.dispatch('user/login', this.loginForm).then(() => {
            this.$router.push({ path: this.redirect || '/' })
            this.loading = false
          }).catch(() => {
            this.loading = false
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    }
  }
}
</script>

<style lang="scss">
/* 修复input 背景不协调 和光标变色 */
/* Detail see https://github.com/PanJiaChen/vue-element-admin/pull/927 */

$bg: #283443;
$light_gray: #fff;
$cursor: #fff;

@supports (-webkit-mask: none) and (not (cater-color: $cursor)) {
  .login-container .el-input input {
    color: $cursor;
  }
}

/* reset element-ui css */
.login-container {
  .el-input {
    display: inline-block;
    height: 32px;
    width: 85%;

    input {
      background: transparent;
      border: 0px;
      -webkit-appearance: none;
      border-radius: 0px;
      padding: 5px 5px 5px 15px;
      color: $light_gray;
      height: 32px;
      caret-color: $cursor;

      &:-webkit-autofill {
        box-shadow: 0 0 0px 1000px $bg inset !important;
        -webkit-text-fill-color: $cursor !important;
      }
    }
  }
  .el-radio-group {
    display: inline-block;
    height: 37px;
    width: 85%;
    // padding: "6px 5px 2px 15px;";

    label {
      background: transparent;
      border: 0px;
      -webkit-appearance: none;
      border-radius: 0px;
      padding: 11px 5px 5px 15px;
      color: $light_gray;
      height: 37px;
      caret-color: $cursor;

      &:-webkit-autofill {
        box-shadow: 0 0 0px 1000px $bg inset !important;
        -webkit-text-fill-color: $cursor !important;
      }
    }
  }

  .el-form-item {
    border: 1px solid rgba(255, 255, 255, 0.1);
    background: rgba(0, 0, 0, 0.1);
    border-radius: 1px;
    color: #454545;
    height: 40px;
    margin-bottom: 22px;
  }
  .el-form-item__content {
    line-height: 30px;
  }
}
.el-input {
  width: 200px !important;
}
</style>

<style lang="scss" scoped>
$bg: #2d3a4b;
$dark_gray: #889aa4;
$light_gray: #eee;

.login-container {
  min-height: 100%;
  width: 100%;
  background-color: $bg;
  overflow: hidden;

  .login-form {
    position: relative;
    width: 520px;
    max-width: 100%;
    padding: 160px 35px 0;
    margin: 0 auto;
    overflow: hidden;
  }

  .tips {
    font-size: 14px;
    color: #fff;
    margin-bottom: 10px;

    p {
      margin-top: 0px;
      margin-right: 16px;
      margin-bottom: 5px;
    }
  }

  .svg-container {
    padding: 6px 5px 6px 15px;
    color: $dark_gray;
    vertical-align: middle;
    width: 30px;
    display: inline-block;
  }

  .title-container {
    position: relative;

    .title {
      font-size: 26px;
      color: $light_gray;
      margin: 0px auto 40px auto;
      text-align: center;
      font-weight: bold;
    }
  }

  .show-pwd {
    position: absolute;
    right: 10px;
    top: 7px;
    font-size: 16px;
    color: $dark_gray;
    cursor: pointer;
    user-select: none;
  }
}
</style>
