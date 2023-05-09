<template>
  <div class="dashboard-container">

    <el-row :gutter="40" class="panel-group">
      <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
        <div class="card-panel">
          <div class="card-panel-icon-wrapper icon-people">
            <i class="el-icon-user-solid" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">
              My delayed workflows
            </div>
            <span class="card-panel-num">8</span>
          </div>
        </div>
      </el-col>
      <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
        <div class="card-panel">
          <div class="card-panel-icon-wrapper icon-people">
            <i class="el-icon-user-solid" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">
              My coming workflows
            </div>
            <span class="card-panel-num">12</span>
          </div>
        </div>
      </el-col>
      <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
        <div class="card-panel">
          <div class="card-panel-icon-wrapper icon-message">
            <i class="el-icon-document" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">
              My delayed tasks
            </div>
            <span class="card-panel-num">6</span>
          </div>
        </div>
      </el-col>
      <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
        <div class="card-panel">
          <div class="card-panel-icon-wrapper icon-message">
            <i class="el-icon-document" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">
              My undergoing tasks
            </div>
            <span class="card-panel-num">17</span>
          </div>
        </div>
      </el-col>
      <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
        <div class="card-panel">
          <div class="card-panel-icon-wrapper icon-people">
            <i class="el-icon-chat-dot-square" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">
              My delayed reservations
            </div>
            <span class="card-panel-num">9</span>
          </div>
        </div>
      </el-col>
      <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
        <div class="card-panel">
          <div class="card-panel-icon-wrapper icon-people">
            <i class="el-icon-chat-dot-square" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">
              My coming reservations
            </div>
            <span class="card-panel-num">9</span>
          </div>
        </div>
      </el-col>
      <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
        <div class="card-panel">
          <div class="card-panel-icon-wrapper icon-money">
            <i class="el-icon-chat-dot-square" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">
              My messages
            </div>
            <span class="card-panel-num">9</span>
          </div>
        </div>
      </el-col>
    </el-row>

    <el-row :gutter="32">
      <el-col :xs="24" :sm="24" :lg="8">
        <div class="chart-wrapper">
          <raddar-chart />
        </div>
      </el-col>
      <el-col :xs="24" :sm="24" :lg="8">
        <div class="chart-wrapper">
          <pie-chart />
        </div>
      </el-col>
      <el-col :xs="24" :sm="24" :lg="8">
        <div class="chart-wrapper">
          <bar-chart />
        </div>
      </el-col>
    </el-row>

    <el-row :gutter="16">
      <el-col :xs="24" :sm="24" :lg="8">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>My Sessions</span>
          </div>
          <el-table :data="mySessions" style="width: 100%">
            <el-table-column prop="type" label="Type" />
            <el-table-column prop="des" label="Description" />
          </el-table>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="24" :lg="8">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>My Permissions</span>
          </div>
          <el-table :data="myPermissions" style="width: 100%">
            <el-table-column prop="type" label="Type" />
            <el-table-column prop="des" label="Description" />
          </el-table>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="24" :lg="8">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>Development Progress</span>
          </div>
          <div class="block">
            <div style="padding-top:5px;" class="progress-item">
              <span>Ligg.Abp-Version 1.2.1</span>
              <el-progress :percentage="90" />
            </div>
            <div class="progress-item">
              <span>Ligg.Vue-Version 1.2.1</span>
              <el-progress :percentage="92" />
            </div>
            <div class="progress-item">
              <span>Ligg.Eww-Version 3.5.2</span>
              <el-progress :percentage="91" />
            </div>
            <div class="progress-item">
              <span>Ligg.Mvc-Version 2.2.1</span>
              <el-progress :percentage="22" />
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import store from '@/store'
import BarChart from './components/BarChart'
import PieChart from './components/PieChart'
import RaddarChart from './components/RaddarChart'

export default {
  name: 'Dashboard',
  components: {
    BarChart,
    PieChart,
    RaddarChart
  },
  data() {
    return {
      mySessions: [],
      myPermissions: []
    }
  },
  computed: {
    ...mapGetters([
      'name'
    ])
  },
  mounted: function () {
    this.init()
  },
  methods: {
    init() {
      let obj = { type: 'token', des: store.getters.token }
      this.mySessions.push(obj)
      obj = { type: 'account', des: store.getters.account }
      this.mySessions.push(obj)
      obj = { type: 'tenantCode', des: store.getters.tenantCode }
      this.mySessions.push(obj)
      obj = { type: 'portalKey', des: store.getters.portalKey }
      this.mySessions.push(obj)

      let str = ''
      let i = 0
      for (const val of store.getters.roles) {
        if (i === 0) { str = val } else {
          str = str + ', ' + val
        }
        i++
      }
      obj = { type: 'roles', des: str }
      this.myPermissions.push(obj)

      i = 0
      for (const val of store.getters.accesibleViews) {
        if (i === 0) { str = val } else {
          str = str + ', ' + val
        }
        i++
      }
      obj = { type: 'accesibleViews', des: str }

      i = 0
      for (const val of store.getters.accesibleActions) {
        if (i === 0) { str = val.value + ', url=' + val.text } else {
          str = str + ' ; ' + val.value + ', url=' + val.text
        }
        i++
      }

      this.myPermissions.push(obj)
      obj = { type: 'accesibleActions', des: str }
      this.myPermissions.push(obj)
    },
    getToken() {
      return store.getters.token
    },
    getAccount() {
      return store.getters.account
    },
    getAvatarUrl() {
      return store.getters.avatarUrl
    }
  }
}
</script>

<style lang="scss" scoped>
.chart-wrapper {
  background: #fff;
  padding: 16px 16px 0;
  margin-bottom: 32px;
}

.panel-group {
  margin-top: 18px;

  .card-panel-col {
    margin-bottom: 32px;
  }

  .card-panel {
    height: 108px;
    cursor: pointer;
    font-size: 12px;
    position: relative;
    overflow: hidden;
    color: #666;
    background: #fff;
    box-shadow: 4px 4px 40px rgba(0, 0, 0, 0.05);
    border-color: rgba(0, 0, 0, 0.05);

    &:hover {
      .card-panel-icon-wrapper {
        color: #fff;
      }

      .icon-people {
        background: #40c9c6;
      }

      .icon-message {
        background: #36a3f7;
      }

      .icon-money {
        background: #f4516c;
      }

      .icon-shopping {
        background: #34bfa3;
      }
    }

    .icon-people {
      font-size: 48px;
      color: #40c9c6;
    }

    .icon-message {
      font-size: 48px;
      color: #36a3f7;
    }

    .icon-money {
      font-size: 48px;
      color: #f4516c;
    }

    .icon-shopping {
      font-size: 48px;
      color: #34bfa3;
    }

    .card-panel-icon-wrapper {
      float: left;
      margin: 14px 0 0 14px;
      padding: 16px;
      transition: all 0.38s ease-out;
      border-radius: 6px;
    }

    .card-panel-icon {
      float: left;
      font-size: 48px;
    }

    .card-panel-description {
      float: right;
      font-weight: bold;
      margin: 26px;
      margin-left: 0px;

      .card-panel-text {
        line-height: 18px;
        color: rgba(0, 0, 0, 0.45);
        font-size: 16px;
        margin-bottom: 12px;
      }

      .card-panel-num {
        font-size: 20px;
      }
    }
  }
}

@keyframes octocat-wave {
  0%,
  100% {
    transform: rotate(0);
  }
  20%,
  60% {
    transform: rotate(-25deg);
  }
  40%,
  80% {
    transform: rotate(10deg);
  }
}

@media (max-width: 550px) {
  .card-panel-description {
    display: none;
  }

  .card-panel-icon-wrapper {
    float: none !important;
    width: 100%;
    height: 100%;
    margin: 0 !important;

    .svg-icon {
      display: block;
      margin: 14px auto !important;
      float: none !important;
    }
  }
}

@media (max-width: 1024px) {
  .chart-wrapper {
    padding: 8px;
  }
}
</style>
