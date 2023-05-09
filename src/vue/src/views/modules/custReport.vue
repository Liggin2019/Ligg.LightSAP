<template>
  <div class="msg-container">
    <div class="msg-wrapper">
      <div>
            <img :src="getImageUrl()" class="icon image-left" height="38" width="38">
      </div>
      <div style="text-align: left; padding-left:45px;margin-top: 5px">
        <span class="msg demo">{{message}}</span>
      </div>
    <div style="text-align: left; padding-left:40px;padding-left:-45px; margin-top: 10px">
      <span class="addl-msg">{{addlMessage}}</span>
    </div>
  </div>
</div>
</template>

<script>

export default {
  name: 'PageDemo',
  data(){
  return {
    name:''
}
  },
  computed: {
    message() {
      const {meta } = this.$route
      return meta.title
    },
    addlMessage() {
      return 'It is a demo page for testing'
    }
  },
  methods: {
    getImageUrl() {
      var imageUrl=require('@/assets/images/' + 'demo.png')
      return imageUrl
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
        height: 100px;
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
