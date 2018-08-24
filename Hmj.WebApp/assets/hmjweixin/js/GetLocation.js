$.ajax({
    url: '/wechat/Business.ashx?para=jsapi',

    type: 'POST',

    data: { apiurl: encodeURIComponent(location.href)},

    dataType: 'html',

    timeout: 1000,

    error: function (e) { },

    success: function (result) {
        result = JSON.parse(result);
        if (result.state == 0) {
            wx.config({
                debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                appId: result.appId, // 必填，公众号的唯一标识
                timestamp: result.timestamp, // 必填，生成签名的时间戳
                nonceStr: result.nonceStr, // 必填，生成签名的随机串
                signature: result.signature,// 必填，签名，见附录1
                jsApiList: ["getLocation", "onMenuShareTimeline", "onMenuShareAppMessage", "onMenuShareQQ", "onMenuShareWeibo", "scanQRCode", "openLocation", "chooseWXPay"] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            });

            wx.ready(function () {
                //alert("ready");
                ExMethod();
                wx.hideMenuItems({
                    menuList: ["share:qq", "weiboApp", "share:facebook", "share:QZone", "copyUrl", "originPage", "openWithQQBrowser", "openWithSafari", "email"] // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
                });
                wx.onMenuShareTimeline({ //分享朋友圈
                    title: "佰草集心约会", // 分享标题
                    link: "http://herbclubwechat.jahwa.com.cn/BcjMember/Index.do", // 分享链接
                    imgUrl: "http://herbclubwechat.jahwa.com.cn/fx.png", // 分享图标
                    success: function () {
                        $.ajax({
                            url: "/Home/SavePage",
                            type: "post",
                            data: { wheres: "groupto" },
                            success: function () {

                            }
                        })
                    },
                    cancel: function () {

                    }
                });
                wx.onMenuShareAppMessage({ //分享给朋友
                    title: "佰草集心约会", // 分享标题
                    desc: "佰草集心约会", // 分享描述
                    link: "http://herbclubwechat.jahwa.com.cn/BcjMember/Index.do", // 分享链接
                    imgUrl: "http://herbclubwechat.jahwa.com.cn/fx.png", // 分享图标
                    type: '', // 分享类型,music、video或link，不填默认为link
                    dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                    success: function () {
                        $.ajax({
                            url: "/Home/SavePage",
                            type: "post",
                            data: { wheres: "personinto" },
                            success: function () {

                            }
                        })
                    },
                    cancel: function () {
                        // 用户取消分享后执行的回调函数
                    }
                });

                //调用页面上的方法
                // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
            });

            wx.error(function (res) {
                //alert("error");
                // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。

            });
        }
    }

});