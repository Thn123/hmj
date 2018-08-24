// -------------------------------
// Demos
// -------------------------------
$(document).ready(
  function () {
      $("#Month").val(new Date().getMonth()+1);
      //alert($("#Month").val())
//      var count = new Array();
//var dates = new Array();
      search();
      //初始化日期时间
      var day = new Date().Format("yyyy-MM-dd");
      $("#BEGIN_DATE").val(day);
      $("#END_DATE").val(day);
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
              $("#BEGIN_DATE").val(start.format('YYYY-MM-DD'));
              $("#END_DATE").val(end.format('YYYY-MM-DD'));

            
          }
      );   
    
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

    

  }

  );


function search() {

    var count = new Array();
    var dates = new Array();
    var month = $("#Month").val();
  
    //alert(c);
    $.post("PassengerFlow2.do", {Month:month },
          function (result) {
              var cuAllPs = result.data;
            
                      var cuAllPsArray = new Array();
                      if (cuAllPs != "") {
                          cuAllPsArray = cuAllPs.split(",");

                      }

                      if (cuAllPs != "") {
                       
                              for (var i = 0; i < cuAllPsArray.length; i++) {
                                  var re = cuAllPsArray[i].split("_");
                                  count.push(re[0]);
                                  dates.push(re[1])

                              }
                          }
                        
                          getTJ(dates, count);
                    

          },
          "json"
    );
}





function getTJ(dates,count)
{
    var ndates = dates;
    var ncount = new Array();
    for(var  i=0;i<count.length;i++)
    {
        ncount.push(parseInt(count[i]));
    }
    //alert(ncount);
    var year = $("#Year").val();
    var month = $("#Month").val();
    //alert(year);
  
    $('#salesContainer').highcharts({
        chart: {
            type: 'spline'
        },
        title: {
            text: year+'年'+month+'月客流量分析'
        },
        xAxis: {
            title: {
                text: '日期'
            },
            categories:ndates
        },
        yAxis: {
            min: 0,
            title: {
                text: '人数'
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
            name: '人数',
            data: ncount
        }]
    });
}


$("#Month").change(function () {

    var count = new Array();
    var dates = new Array();
    var month = $("#Month").val();
    $.post("PassengerFlow2.do", { Month: month },
          function (result) {
              var cuAllPs = result.data;

              var cuAllPsArray = new Array();
              if (cuAllPs != "") {
                  cuAllPsArray = cuAllPs.split(",");

              }

              if (cuAllPs != "") {

                  for (var i = 0; i < cuAllPsArray.length; i++) {
                      var re = cuAllPsArray[i].split("_");
                      count.push(re[0]);
                      dates.push(re[1])

                  }
              }

              getTJ(dates, count);


          },
          "json"
    );
});




