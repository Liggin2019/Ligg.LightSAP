
<template>
  <div class="msg-container">
    <div class="msg-wrapper">
      <div>
            <img :src="getImageUrl()" class="icon image-left" height="38" width="38">
      </div>
      <div style="text-align: left; padding-left:45px;margin-top: 5px">
        <span class="msg demo" v-html="title"></span>
      </div>
      <div style="text-align: left; padding-left:45px;margin-top: 5px">
        <el-button :type="primary"  style="width:65px;margin:20px 30px" @click.native.prevent="search" v-show="true">搜索</el-button>
        <el-button :loading="loading" type="primary" style="width:65px;margin:20px 30px" @click.native.prevent="add" v-show="showButton('btnAdd')">新增</el-button>
        <el-button :loading="loading" type="primary" style="width:65px;margin:20px 30px" @click.native.prevent="deleteSelected" v-show="showButton('btnDelete')">删除</el-button>
      </div>
    <div style="text-align: left; padding-left:40px;padding-left:-45px; margin-top: 10px">
      <span>物料列表</span>
    </div>
    <div style="text-align: left; padding-left:40px;padding-left:-45px; margin-top: 10px">
      <span class="" v-html="message"></span>
    </div>
    <div style="text-align: left; padding-left:40px;padding-left:-45px; margin-top: 10px">
      <span class="addl-msg" v-html="addlMessage"></span>
    </div>
  </div>
</div>
</template>

<script>
import { GetPagedMaterialManageDtos } from '@/api/prd/material'
import { AddMaterial } from '@/api/prd/material'
import { DeleteSelectedMaterials } from '@/api/prd/material'
import { GetUnavailableClientViewButtons } from '@/api/config'
export default {
  name: 'PageDemo',
  data(){
    return {
      loading: false,
      title: "物料管理",
      message:"",
      addlMessage:"",
      removedButtons:[]
    }

  },
  computed: {
  },
  mounted: function () {
    this.getRemovedButtons()
  },
  methods: {
    getImageUrl() {
      var imageUrl=require('@/assets/images/' + 'demo.png')
      return imageUrl
    },
    getRemovedButtons() {
      new Promise((resolve, reject) => {
        GetUnavailableClientViewButtons('mngMaterialAm').then(response => {
        var data=response.data
        this.removedButtons=data
        
      }).catch(error => {
        reject(error)
        })
      });
    },
    showButton(code) {
      if(this.removedButtons.indexOf(code)>-1) return false
      return true
    },
    search() {
      new Promise((resolve, reject) => {
        GetPagedMaterialManageDtos({mark:'xxx',text: 'yyyy', pageSize: 15, pageIndex: 5}).then(response => {
        var data=response.data
        this.message=data
        this.loading = false
      }).catch(error => {
        reject(error)
        this.loading = false
        })
      });
    },
    add() {
      new Promise((resolve, reject) => {
        AddMaterial({type:99},{code:'xxx',description: 'yyyy'}).then(response => {
        var data=response.data
        this.addlMessage="add succeeded"
        this.loading = false
      }).catch(error => {
        reject(error)
        this.loading = false
        this.addlMessage="add failed"
        })
      });
    },

    deleteSelected() {
       new Promise((resolve, reject) => {
        DeleteSelectedMaterials({ids:'777,888,999'},{}).then(response => {
        var data=response.data
        this.addlMessage="deleteSelected succeeded"
        this.loading = false
      }).catch(error => {
        reject(error)
        this.loading = false
        this.addlMessage="deleteSelected failed"
        })
      });
    }
  }
}
</script>

<style>
    .msg-container {
        padding: 100px 35px;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: #f3f3f4;
    }

    .msg-wrapper {
        width: 800px;
        height: 180px;
    }

    .demo {
        color: #4fc08D;
    }

    .image-left {
        float: left;
    }

    .msg {
        font-weight: 400;
        font-size: 28px;
    }

    .addl-msg {
        color: burlywood;
        font-size: 15px;
    }
</style>
