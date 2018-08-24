// -------------------------------
// Demos
// -------------------------------
$(document).ready(
  function () {
      search();
      $("#xxfx2014").click();
      $("#zmfx2014").click();
      try {
          //Set nicescroll on notifications
          $(".scrollthis").niceScroll({ horizrailenabled: false });
          $('.dropdown').on('shown.bs.dropdown', function () {
              $(".scrollthis").getNiceScroll().resize();
              $(".scrollthis").getNiceScroll().show();
          });
          $('.dropdown').on('hide.bs.dropdown', function () {
              $(".scrollthis").getNiceScroll().hide();
          });

          $(window).scroll(function () {
              $(".scrollthis").getNiceScroll().resize();
          });
      } catch (e) { }

      var good = true;
      $("#storeLoad").click(function () {
          if (good == true) {
              _showInfoMessage("同步开始请稍后....", "info");
              good = false;

              $.ajax({
                  url: "/Home/LoadStore.do",
                  method: "post",
                  success: function (data) {
                      if (data.Status == "1") {
                          _showInfoMessage(data.Message, "info");
                      } else {
                          _showInfoMessage(data.Message, "error");
                      }
                  }
              })
          } else {
              _showInfoMessage("同步中请不要重复点击！！！！", "info");
          }
      })

      var goods = true;
      $("#cityLoad").click(function () {
          if (goods == true) {
              _showInfoMessage("同步开始请稍后....", "info");
              goods = false;

              $.ajax({
                  url: "/Home/LoadCity.do",
                  method: "post",
                  success: function (data) {
                      if (data.Status == "1") {
                          _showInfoMessage(data.Message, "info");
                      } else {
                          _showInfoMessage(data.Message, "error");
                      }
                  }
              })
          } else {
              _showInfoMessage("同步中请不要重复点击！！！！", "info");
          }
      })

  });

//初始化日期时间
var nowDay = new Date().Format("yyyy-MM-dd ");
$('#btnDateRangePicker span').html(moment().startOf('month').format('YYYY-MM-DD') + "至" + moment().endOf('month').format('YYYY-MM-DD'));

//时间范围快速查询
$('#btnDateRangePicker').daterangepicker(
    {
        ranges: {
            '今天': [moment(), moment()],
            '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
            '本周': [moment().subtract('days', 6), moment()],
            //'上周': [moment().subtract('days', 13), moment()],
            '本月': [moment().startOf('month'), moment().endOf('month')],
            //'上月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')],
            '今年': [moment().startOf('year'), moment().endOf('year')],
            '去年': [moment().subtract('year', 1).startOf('year'), moment().subtract('year', 1).endOf('year')],
        },
        opens: 'left',
        startDate: moment().subtract('days', 29),
        endDate: moment()

    },
    function (start, end) {
        //选择日期事件
        $('#btnDateRangePicker span').html(start.format('YYYY-MM-DD') + ' 至 ' + end.format('YYYY-MM-DD'));
        $("#Dashboard_Start_Date").val(start.format('YYYYMMDD'));
        $("#Dashboard_End_Date").val(end.format('YYYYMMDD'));
        //alert($("#Dashboard_Start_Date").val());
        //提交表单查询
        search();
    }
);


function search() {

    //$('#confirmModal').modal('hide');
    //var id = $("#hdCurrentId").val();
    var Dashboard_Start_Date = $("#Dashboard_Start_Date").val();
    var Dashboard_End_Date = $("#Dashboard_End_Date").val();
    if (Dashboard_Start_Date == "") {
        Dashboard_Start_Date = moment().startOf('month').format('YYYYMMDD');
    }
    if (Dashboard_End_Date == "") {
        Dashboard_End_Date = moment().endOf('month').format('YYYYMMDD');
    }
    // alert(Dashboard_Start_Date);
    $.post("IndexSearch.do", { Dashboard_Start_Date: Dashboard_Start_Date, Dashboard_End_Date: Dashboard_End_Date },
          function (res) {
              //alert(res.C1);
              $("#wdxx").html(res.C1);
              $("#xzfs").html(res.C2);
              $("#plfs").html(res.C3);
              $("#syfs").html(res.C4);
          },
          "json"
    );
}

$("#xxfx2014").click(function (e) {
    $("#xxfx2014").attr("class", "btn btn-default btn-sm active");
    $("#xxfx2013").attr("class", "btn btn-default btn-sm");
    $('#salesContainer').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '2014年销售分析'
        },
        xAxis: {
            categories: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']
        },
        yAxis: {
            min: 0,
            title: {
                text: '百分比'
            }
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
            shared: true
        },

        plotOptions: {
            column: {
                stacking: 'percent'
            }
        },
        series: [{
            name: '卡收入',
            data: [0, 0, 0, 0, 0, 5, 3, 4, 0, 2, 1, 0]
        }, {
            name: '项目收入',
            data: [3, 4, 4, 2, 5, 5, 7, 2, 3, 8, 6, 0]
        }, {
            name: '产品收入',
            data: [3, 4, 4, 2, 5, 5, 7, 2, 3, 8, 6, 0]
        }]
    });
})

$("#xxfx2013").click(function (e) {
    $("#xxfx2013").attr("class", "btn btn-default btn-sm active");
    $("#xxfx2014").attr("class", "btn btn-default btn-sm");
    $('#salesContainer').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '2013年销售分析'
        },
        xAxis: {
            categories: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']
        },
        yAxis: {
            min: 0,
            title: {
                text: '百分比'
            }
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
            shared: true
        },

        plotOptions: {
            column: {
                stacking: 'percent'
            }
        },
        series: [{
            name: '卡收入',
            data: [9, 8, 2, 0, 3, 5, 3, 4, 0, 2, 1, 5]
        }, {
            name: '项目收入',
            data: [3, 4, 4, 2, 5, 5, 7, 2, 3, 0, 6, 0]
        }, {
            name: '产品收入',
            data: [3, 4, 5, 2, 7, 5, 0, 2, 3, 8, 2, 1]
        }]
    });
})


//图表salesContainer
//$(function () {
//    $('#salesContainer').highcharts({
//        chart: {
//            type: 'column'
//        },
//        title: {
//            text: '2014年销售分析'
//        },
//        xAxis: {
//            categories: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']
//        },
//        yAxis: {
//            min: 0,
//            title: {
//                text: '百分比'
//            }
//        },
//        tooltip: {
//            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
//            shared: true
//        },

//        plotOptions: {
//            column: {
//                stacking: 'percent'
//            }
//        },
//        series: [{
//            name: '会员卡收入',
//            data: [5, 3, 4, 7, 2, 5, 3, 4, 7, 2, 1, 5]
//        }, {
//            name: '非会员卡收入',
//            data: [3, 4, 4, 2, 5, 5, 7, 2, 3, 8, 6, 5]
//        }]
//    });
//});		

$("#zmfx2013").click(function (e) {
    $("#zmfx2013").attr("class", "btn btn-default btn-sm active");
    $("#zmfx2014").attr("class", "btn btn-default btn-sm");
    $("#zmfx2012").attr("class", "btn btn-default btn-sm");
    $('#container1').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '2013年会员招募分析'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: [
                'Jan',
                'Feb',
                'Mar',
                'Apr',
                'May',
                'Jun',
                'Jul',
                'Aug',
                'Sep',
                'Oct',
                'Nov',
                'Dec'
            ]
        },
        yAxis: {
            min: 0,
            title: {
                text: '人数'
            }
        },
        tooltip: {
            pointFormat: '<table><tbody><tr><td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y:.1f}</b></td></tr></tbody></table>',
            shared: true,
            useHTML: true
        },

        series: [{
            type: 'column',
            name: '会员人数',
            data: [49, 71, 58, 66, 39, 80, 75, 68, 74, 39, 55, 54]

        }, {
            type: 'spline',
            name: '总人数',
            data: [49, 120, 178, 209, 252, 275, 287, 300, 37, 69, 76, 45]

        }, {
            type: 'pie',
            name: '消费人数',
            data: [49, 120, 178, 209, 252, 275, 287, 300, 337, 369, 376, 405]

        }]
    });
})

$("#zmfx2014").click(function (e) {
    $("#zmfx2014").attr("class", "btn btn-default btn-sm active");
    $("#zmfx2013").attr("class", "btn btn-default btn-sm");
    $("#zmfx2012").attr("class", "btn btn-default btn-sm");
    $('#container1').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '2014年会员招募分析'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: [
                'Jan',
                'Feb',
                'Mar',
                'Apr',
                'May',
                'Jun',
                'Jul',
                'Aug',
                'Sep',
                'Oct',
                'Nov',
                'Dec'
            ]
        },
        yAxis: {
            min: 0,
            title: {
                text: '人数'
            }
        },
        tooltip: {
            pointFormat: '<table><tbody><tr><td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y:.1f}</b></td></tr></tbody></table>',
            shared: true,
            useHTML: true
        },

        series: [{
            type: 'column',
            name: '会员人数',
            data: [49, 71, 58, 66, 39, 80, 75, 68, 74, 39, 376, 405]

        }, {
            type: 'spline',
            name: '总人数',
            data: [49, 120, 178, 209, 252, 275, 287, 300, 337, 369, 376, 405]

        }, {
            type: 'line',
            name: '消费人数',
            data: [49, 120, 178, 209, 252, 275, 287, 300, 37, 69, 76, 45]

        }]
    });
})

$("#zmfx2012").click(function (e) {
    $("#zmfx2012").attr("class", "btn btn-default btn-sm active");
    $("#zmfx2013").attr("class", "btn btn-default btn-sm");
    $("#zmfx2014").attr("class", "btn btn-default btn-sm");
    $('#container1').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '2012年会员招募分析'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: [
                'Jan',
                'Feb',
                'Mar',
                'Apr',
                'May',
                'Jun',
                'Jul',
                'Aug',
                'Sep',
                'Oct',
                'Nov',
                'Dec'
            ]
        },
        yAxis: {
            min: 0,
            title: {
                text: '人数'
            }
        },
        tooltip: {
            pointFormat: '<table><tbody><tr><td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y:.1f}</b></td></tr></tbody></table>',
            shared: true,
            useHTML: true
        },

        series: [{
            type: 'column',
            name: '会员人数',
            data: [49, 71, 58, 66, 39, 80, 75, 68, 74, 39, 55, 54]

        }, {
            type: 'spline',
            name: '总人数',
            data: [49, 120, 178, 209, 252, 275, 287, 300, 337, 369, 376, 405]

        }, {
            type: 'pie',
            name: '消费人数',
            data: [49, 120, 178, 209, 252, 275, 287, 300, 337, 369, 376, 405]

        }]
    });
})

//$(function () {
//    $('#container1').highcharts({
//        chart: {
//            type: 'column'
//        },
//        title: {
//            text: '2014年会员招募分析'
//        },
//        subtitle: {
//            text: ''
//        },
//        xAxis: {
//            categories: [
//                'Jan',
//                'Feb',
//                'Mar',
//                'Apr',
//                'May',
//                'Jun',
//                'Jul',
//                'Aug',
//                'Sep',
//                'Oct',
//                'Nov',
//                'Dec'
//            ]
//        },
//        yAxis: {
//            min: 0,
//            title: {
//                text: '人数'
//            }
//        },
//        tooltip: {
//            pointFormat: '<table><tbody><tr><td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y:.1f}</b></td></tr></tbody></table>',
//            shared: true,
//            useHTML: true
//        },
////        plotOptions: {
////            column: {
////                pointPadding: 0.2,
////                borderWidth: 0
////            }
////        },
////        series: [{
////            type: 'column',
////            name: '点评团',
////            data: [49, 71, 106, 129, 144, 176, 135, 148, 216, 194, 95, 54]

////        }, {
////            type: 'column',
////            name: '朋友介绍',
////            data: [83, 78, 98, 93, 106, 84, 105, 104, 91, 83, 106, 92]

////        }, {
////            type: 'column',
////            name: '媒体',
////            data: [48, 38, 39, 41, 47, 48, 59, 59, 52, 65, 59, 51]

////        }, {
////            type: 'column',
////            name: '路过',
////            data: [42, 33, 34, 39, 52, 75, 57, 60, 47, 39, 46, 5]

////        }, {
////            type: 'spline',
////            name: 'total',
////            data: [42, 33, 34, 39, 52, 75, 57, 60, 47, 39, 46, 5]

////        }]

//        series: [{
//            type: 'column',
//            name: '顾客人数',
//            data: [49, 71, 58, 66, 39, 80, 75, 68, 74, 39, 55, 54]

//        }, {
//            type: 'spline',
//            name: 'total',
//            data: [49, 120, 178, 209, 252, 275, 287, 300, 337, 369, 376, 405]

//        }]
//    });
//});
$(document).ready(function () {
    var storeId = $("#hideStoreId").val();
    //未约进列表
    //$.post("/Home/BookingList.do", { storeId: storeId },
    //     function (data) {
    //         if (data.Status > 0) {
    //             var json = data.Data;
    //             $(json).each(function () {
    //                 var html = [];
    //                 html.push('<li><a href="#" class="notification-user active">');
    //                 html.push('<span class="time">' + this.ONDATE + '</span><i class="fa fa-user"></i>');
    //                 html.push('<span class="msg">'+this.NAME+' '+this.MOBILE+'</span>');
    //                 html.push('</a></li>');

    //                 $(html.join('')).appendTo("#DivBookingList");

    //             });
    //         }
    //     },
    //     "json"
    //    );

    //未付款订单
    //$.post("/Home/OrderList.do", { storeId: storeId },
    //     function (data) {
    //         if (data.Status > 0) {
    //             var json = data.Data;
    //             $(json).each(function () {
    //                 var html = [];
    //                 html.push('<li><a href="/Order/OrderD.do?cid='+this.CUST_ID+'&oid='+this.ID+'" class="active">');
    //                 html.push('<span class="time">' + this.ONDATE + '</span>');
    //                 html.push('');
    //                 html.push('<div><span class="name">订单号：' + this.ORDER_NO + '</span>');
    //                 html.push('<span class="msg">姓名：' + this.CUST_NAME + '</span></div>');
    //                 html.push('</a></li>');

    //                 $(html.join('')).appendTo("#divOrderList");

    //             });
    //         }
    //     },
    //     "json"
    //    );

    $('#txtMemberSearch').bind('keypress', function (event) {
        if (event.keyCode == "13") {
            MemberSearch();
        }
    });

});
function MemberSearch() {
    //查会员信息信息
    var condition = $("#txtMemberSearch").val();
    if (condition == "" || condition == null) {
        return false;
    }
    $.post("/Home/SearchMemberInfo.do", { q: condition },
          function (data) {
              if (data.Status > 0) {
                  var cid = eval(data.Data);
                  window.location.href = "/Customer/View360.do?cid=" + cid;
              }
              else {
                  _showInfoMessage(data.Message, "error");
              }
          },
          "json"
         );
}