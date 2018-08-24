; (function (window, undefined, $) {
    $(document).ready(function () {

        //金额格式化
        function formatRMB(value, cell) {
            value = value.toString().replace(/\$|\,/g, '');
            if (isNaN(value))
                value = "0";
            sign = (value == (value = Math.abs(value)));
            value = Math.floor(value * 100 + 0.50000000001);
            cents = value % 100;
            value = Math.floor(value / 100).toString();
            if(cents<10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3) ; i++)
                value = value.substring(0, value.length - (4 * i + 3)) + ',' +
                value.substring(value.length - (4 * i + 3));
            return (((sign) ? '' : '-') + '<span class="badge"><b>￥</b>' + value + '.' + cents + '</span>');
        }
        //产品名称格式化
        function formatName(value, cell)
        {
            if (value.length > 14)
            {
                value=value.substring(0,14)+".."
            }
            return value;
        }

        //根据姓名/电话检索
        $("#CUST_NAME").select2({
            placeholder: "输入姓名、电话查询",
            minimumInputLength: 2,
            width: 'resolve',
            ajax: {
                url: options.queryCustomerUrl,
                dataType: 'json',
                quietMillis: 100,
                data: function (term, page) {
                    return {
                        q: term, //search term
                        page_limit: 10, // page size
                        page: page // page number
                    };
                },
                results: function (data, page) {
                    var strJson = eval("(" + data.data + ")");
                    return { results: strJson, more: false };
                }
            },
            formatResult: custFormatResult,
            formatSelection: custFormatSelection,
            dropdownCssClass: "bigdrop",
            formatNoMatches: function (m) {
                return "<span style='color:#7B7B7B;font-size:12px;' >未查询到相关信息</span>&nbsp;&nbsp;<a href='javascript:;' id='newCust' onclick='return showAddCustName();'><i class='fa fa-plus'></i> 新增</a>";
            },
            escapeMarkup: function (m) { return m; }
        });

        function custFormatResult(obj) {
            var markup = "<table class='movie-result'><tr>";
            markup += "<td class='movie-info'><div class='movie-title'>" + obj.title + "</div>";
            if (obj.mobile !== undefined) {
                markup += "<div class='movie-synopsis'>" + obj.mobile + "</div>";
            }
            markup += "</td></tr></table>"
            return markup;
        }
        //选择顾客
        function custFormatSelection(obj) {
            window.location.href = window.location.href + "?cid=" + obj.id;
            return false;
            //return obj.title;
        }

        //根据项目名称、价格检索
        $("#PROD_NAME_PRICE").select2({
            placeholder: "输入项目名、价格查询",
            minimumInputLength: 2,
            width: 'resolve',
            ajax: {
                url: options.queryGodSerCardUrl,
                dataType: 'json',
                quietMillis: 100,
                data: function (term, page) {
                    return {
                        q: term, //search term
                        page_limit: 10, // page size
                        page: page // page number
                    };
                },
                results: function (data, page) {
                    var strJson = eval("(" + data.data + ")");
                    return { results: strJson, more: false };
                }
            },
            formatResult: goodsFormatResult,
            formatSelection: goodsFormatSelection,
            dropdownCssClass: "bigdrop", //
            escapeMarkup: function (m) { return m; } // 
        });

        function goodsFormatResult(obj) {
            var markup = "<table class='movie-result'><tr>";
            markup += "<td class='movie-info'><div class='movie-title'>" + obj.title + "</div>";
            if (obj.price !== undefined) {
                markup += "<div class='movie-synopsis'>￥：" + obj.price + "</div>";
            }
            markup += "</td></tr></table>"
            return markup;
        }

        //选择项目添加到购物车
        function goodsFormatSelection(obj) {
            //getProAjax(obj.id, obj.table);
            return obj.title;
        }


    });



    //提交表单
    $(function () {
        var submiting = false;
        $('#formSave').validator({
            rules: {
            },
            fields: {
                '#CUST_NO': 'required',
                '#NAME': 'required',
                '#IDCARD': 'required;idcard',
                '#EMAIL': 'email',
            },
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", "success");
                        //保存订单返回列表 2秒跳转
                        setInterval(returnList, 2000);
                        return false;
                    }
                    else {
                        showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                    submiting = false;
                })
            }
        });


    });

    //保存订单备注信息
    $("#btnSaveRemark").click(function () {
        var oid=$("#hide_OrderId").val();
        var remark=$.trim($("#txtOrderRemark").val());
        if (remark == "") {
            _showInfoMessage('请输入订单备注信息！', 'info');
            return false;
        }
        $.post(options.saveOrderRemarkUrl, { orderid: oid,remark:remark },
                  function (data) {
                      if (data.status == 1) {
                          _showInfoMessage('订单备注保存成功！', 'success');
                      }
                      else {
                          _showInfoMessage('订单备注保存失败！', 'error');
                      }
                  },
                  "json"
                 );

    });

    //作废订单
    $("#btnInvalid").click(function () {
        if (!confirm("您确认要作废该订单吗？"))
        {
            return false;
        }
        var oid = $("#hide_OrderId").val();

        $.post(options.UpdateStatusUrl, { orderid: oid, order_status: 1 },
                 function (data) {
                     if (data.status == 1) {
                         _showInfoMessage('作废订单成功！', 'success');
                         window.location.href = "/Order/OrderList.do";
                     }
                     else {
                         _showInfoMessage('作废订单失败！', 'error');
                     }
                 },
                 "json"
                );
    });




  
    




    //window.xjDailog.Open("/Order/OrderPrint.do", {
    //    width: 500,
    //    height: 620,
    //    caption: '小票打印',
    //    theme: "simple", //默认主题
    //    onclose: function (userstate) {
    //        xjgrid.Reload();

    //    } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
    //});
    var paystatus = $("#payStatus").val();
    //新生成的订单 打印小票
    if (paystatus == "new") {
        var left = (screen.width - 400) / 2;
        var top = (screen.height - 500) / 2;
        window.open("/Order/OrderPrint.do?cid=" + $("#hide_Cid").val() + "&oid=" + $("#hide_OrderId").val(), "_blank", "height=500,width=400,left=" + left + ",top=" + top + ",scrollbars=yes,location=no,resizable=yes");
    }
})(window, undefined, jQuery);


//价格格式化
function formatPrice(value) {
    value = value.toString().replace(/\$|\,/g, '');
    if (isNaN(value))
        value = "0";
    sign = (value == (value = Math.abs(value)));
    value = Math.floor(value * 100 + 0.50000000001);
    cents = value % 100;
    value = Math.floor(value / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3) ; i++)
        value = value.substring(0, value.length - (4 * i + 3)) + ',' +
        value.substring(value.length - (4 * i + 3));
    return (((sign) ? '' : '-') + '' + value + '.' + cents);
}
//金额转数字
function fromatMoneyNum(value) {
    var num = value.trim();
    var ss = value.toString();
    if (ss.length == 0) {
        return "0";
    }
    return ss.replace(/,/g, "");

}


function WindowPrint() {

    var left = (screen.width - 400) / 2;
    var top = (screen.height - 500) / 2;
    window.open("/Order/OrderPrint.do?cid=" + $("#hide_Cid").val() + "&oid=" + $("#hide_OrderId").val(), "_blank", "height=500,width=400,left=" + left + ",top=" + top + ",scrollbars=yes,location=no,resizable=yes");

}



