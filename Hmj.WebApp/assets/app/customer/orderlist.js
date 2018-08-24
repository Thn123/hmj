; (function (window, undefined, $) {
    $(document).ready(function () {
        var gridopt = {
            url: options.linkUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "5%", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '订单日期', name: 'CREATE_DATE', width: "9%", sortable: false, align: 'left', process: formatDate },
                    { display: '订单号', name: 'ORDER_NO', width: "15%", sortable: false, align: 'left' },
                    { display: '状态', name: 'ORDER_STATUSS', width: "8%", sortable: false, align: 'left' },
                    { display: '状态', name: 'ORDER_STATUS', width: "6%", sortable: false, hide: true, align: 'left' },
                    //{ display: '顾客姓名', name: 'CUST_NAME', width: "9%", sortable: false, align: 'left' },
                    { display: '订单项目', name: 'PRODUCT_NAME', width: "20%", sortable: false, align: 'left' },
                    { display: '应付金额', name: 'PAY_AMT', width: "9%", sortable: false, align: 'left', process: formatRMB },
                    { display: '付款方式', name: 'PAYMENT_TYPE', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "19%", sortable: false, hide: true, align: 'left', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='查看订单信息' class='abtn' href='javascript:;'  onclick=\"util.View('", value, "')\"><i class='fa fa-search' ></i> 查看</a>");
            if (cell[4]=="1")
            {
                ops.push("&nbsp;<a title='退款' class='abtn' href='javascript:;'  onclick=\"util.Refund('", value, "')\"><i class='fa fa-shopping-cart' ></i> 退款</a>");
                ops.push("&nbsp;<a title='作废' class='abtn' href='javascript:;'  onclick=\"util.Cancell('", value, "')\"><i class='fa fa-ban' ></i> 作废</a>");
            }
            return ops.join("");
        }
        function formatOrderStatus(value, cell) {
            var status = "未知";
            switch (value) {
                case "0":
                    status = "新建";
                    break;
                case "1":
                    status = "已支付";
                    break;
                case "8":
                    status = "已退款";
                    break;
                case "9":
                    status = "已取消";
                    break;
            }
            return status;
        }
        function formatPayStatus(value, cell) {
            var status = "未知";
            switch (value) {
                case "0":
                    status = "未支付";
                    break;
                case "1":
                    status = "已支付";
                    break;
                case "2":
                    status = "部分支付";
                    break;
                case "8":
                    status = "已退款";
                    break;
                case "9":
                    status = "已取消";
                    break;
            }
            return status;
        }
        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0]
            }
            else {
                return "-"
            }
        }

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
            return (((sign) ? '' : '-') + '<b>￥</b>' + value + '.' + cents);
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            var url = options.editUrl;
            window.xjDailog.Open(url, {
                width: 680,
                height: 570,
                caption: '新增客户信息',
                theme: "simple", //默认主题
                onclose: function (userstate) {
                    xjgrid.Reload();

                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        });
        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          showLoadingMsg("操作成功", { right: 100, top: 10 }, true, 5000);
                          xjgrid.Reload();
                      }
                      else {
                          showErrorTip("操作失败！：" + res.message, { right: 100, top: 10 }, true, 5000);
                      }
                  },
                  "json"
            );
        })
        util.Edit = function (id) {
            var url = options.editUrl + "/" + id;
            window.xjDailog.Open(url, {
                width: 680,
                height: 570,
                caption: '编辑客户信息',
                theme: "simple", //默认主题
                onclose: function (userstate) {
                    xjgrid.Reload();
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        };
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };

        util.View360 = function (id) {
            //TODO:
            //alert("跳转到客户360页面");
            location.href = "View360.do" + "?cid=" + id;
        };
    });

})(window, undefined, jQuery);
