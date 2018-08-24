//var ZFT	社区发帖
//ZFW	访问社区
//ZHT	社区回帖
//ZZR	积分转移
//ZHD	社区奖励活动
//ZTZ	手工调整积分	相对SAP CRM新增
//ZZJ	手工增加积分
//ZJS	手工减少积分
//ZTIER	手工调增级别
//ZRE	家化会员退货
//ZPRODUCT	家化会员购买
//PRODUCT_REDEEM	家化会员兑换
//ZRC	社区日常活动
//ZXY 促销奖励活动
//ZRDH 会员兑换冲销
//ZTJ 会员推荐获得积分
//ZBT 被推荐会员获得积分
//ZPGQ 积分过期	相对SAP CRM新增
//ZTHFC 家化会员退货反冲	相对SAP CRM新增
//ZPDX 积分抵现	相对SAP CRM新增
//ZCJ 会员抽奖扣减积分
//ZDH 会员手机兑换扣减积分
//ZXZ 奖励活动积分扣减
//ZHK 会员换卡
//ZZH
var arr = {
    'ZFT': '社区发帖'
    , 'ZFW': '访问社区'
    , 'ZHT': '社区回帖'
    , 'ZZR': '积分转移'
    , 'ZHD': '社区奖励活动'
    , 'ZTZ': '手工调整积分'
    , 'ZZJ': '手工增加积分'
    , 'ZJS': '手工减少积分'
    , 'ZTIER': '手工调增级别'
    , 'ZRE': '家化会员退货'
    , 'ZPRODUCT': '家化会员购买'
    , 'PRODUCT_REDEEM': '家化会员兑换'
    , 'ZRC': '社区日常活动'
    , 'ZXY': '促销奖励活动'
    , 'ZRDH': '会员兑换冲销'
    , 'ZTJ': '会员推荐获得积分'
    , 'ZBT': '被推荐会员获得积分'
    , 'ZPGQ': '积分过期'
    , 'ZTHFC': '家化会员退货反冲'
    , 'ZPDX': '积分抵现'
    , 'ZCJ': '会员抽奖扣减积分	'
    , 'ZDH': '会员手机兑换扣减积分'
    , 'ZXZ': '奖励活动积分扣减'
    , 'ZHK': '会员换卡'
    , 'ZZH': '转化至华美家'
};

var m = localStorage.getItem("tel");
$(".ava_points").html(localStorage.getItem(m + "AVA_POINTS"));


$.ajax({
    type: 'get',
    url:$.domainUrl+ 'Member/GetPointDetail?MOBILE=' + localStorage.getItem('tel'),
    success: function (res) {
        var html = '';
        for (var i = 0; i < res.data.length; i++) {
            var obj = res.data[i];
            var a = res.data[i].ORDER_TYPE;
            html += '<div class="box">\
	        		<div class="fl">\
	        			<em class="add">'+ obj.POINTS + '分</em>\
	        			<span class="time">'+ obj.CREATED_TIME + '</span>\
	        		</div>\
	        		<div class="fr">\
	        			<span>'+ arr[obj.ORDER_TYPE] + '</span>\
	        		</div>\
	       </div>';
        }
        $(".list_box").html(html);
    },
    error: function () {
    }
})
