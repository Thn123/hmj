; (function (window, undefined, $) {
    $(document).ready(function () {
        var gridopt = {
            url: options.bookingUrl,
            colModel: [
                    { display: '编号', name: 'ID',sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '预约时间', name: 'CREATE_DATE',  sortable: false, align: 'left', process: formatDate },
                    { display: '门店名称', name: 'STORE_NAME',sortable: false, align: 'left' },
                    //{ display: '服务项目', name: 'SERVICE_NAME', sortable: false, align: 'left' },
                    { display: 'CUST_ID', name: 'CUST_ID', width: 160, sortable: false, hide: true, align: 'left' },
                    { display: '床位', name: 'BED_ID', sortable: false, hide: true, align: 'left' },
                    { display: '护理师', name: 'EMP_NAME',  sortable: false, align: 'left' },
                    { display: '备注', name: 'REMARK',  sortable: false, align: 'left' },
                    { display: '状态', name: 'STATUS', width: 70, sortable: false, align: 'left', process: formatStatus },
                    { display: '操作', name: 'ID',  sortable: false, align: 'center', process: processOp }
            ],
            sortname: "ID",
            sortorder: "DESC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var jsondata = [];
            jsondata.push("{'ID':", cell[0], ",'STATUS':'", cell[7],"'}");
//            jsondata.push("','UserCard':'", cell[8], "','MemberId':'", cell[9]);
//            jsondata.push("','UserFullName':'", cell[10], "','IdCard':'", cell[11]);
//            jsondata.push("','MobilePhone':'", cell[12], "','BedId':'",cell[13],"'}");
         
            var status = cell[7];//状态
            var ops = []; 
            //if (opt.subId == cell[14]) {
                if (status == "0") {
                    ops.push("<a title='开单' abbr='2' href='javascript:void(0)' onclick=\"util.CheckIn(", jsondata.join(""), ")\"><i class='fa fa-shopping-cart'></i><span style='margin-left:1px'> 开单</span></a>");
                    ops.push("&nbsp;<a title='修改预约'  abbr='5'  href='javascript:void(0)'  onclick=\"util.Edit(", jsondata.join(""), ")\"><i class='fa fa-edit' ></i> 修改</a>");
                    ops.push("&nbsp;<a title='取消预约'  abbr='6' href='javascript:void(0)'  onclick=\"util.Cancel(", jsondata.join(""), ")\"><i class='fa fa-trash-o' ></i> 取消</a>");
                }
                var url = options.viewOrderDetailUrl + "?cid=" + cell[3];
                if(status == "1")
                {
                    ops.push("<a title='查看业务单' href='", url, "'><i class='fa fa-th-list' ></i> 查看业务单</a>");
                }
                if (status == "2")
                {
                    ops.push("&nbsp;<a title='结账' href='", url, "'><i class='fa fa-money' ></i> 结账</a>");
                }
            //}
            return ops.join("");
        }
        function formatGender(value, cell) {
            if (value == "1") {
                return "男";
            }
            else {
                return "女";
            }
        }
        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0]
            }
            else {
                return "-"
            }
        }
        function formatStatus(value, cell) {
            var status = "未知";
            switch (value) {
                case "0":
                    status = "新预约";
                    break;
                case "1":
                    status = "已开单";
                    break;
                case "2":
                    status = "已结账";
                    break;
                case "7":
                    status = "未约进";
                    break;
                case "8":
                    status = "已爽约";
                    break;
                case "9":
                    status = "已取消";
                    break;
            }
            return status;
        }
//        $("#formQuery").submit(function () {
//            xjgrid.Query(this);
//            return false;
//        });

//        $("#btnAdd").click(function (e) {
//            var url = options.editUrl;
//            window.xjDailog.Open(url, {
//                width: 680,
//                height: 570,
//                caption: '新增客户信息',
//                theme: "simple", //默认主题
//                onclose: function (userstate) {
//                    xjgrid.Reload();

//                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
//            });
//        });
//        $("#btnConfirm").click(function (e) {
//            $('#confirmModal').modal('hide');
//            var id = $("#hdCurrentId").val();
//            $.post(options.deleteUrl + "/" + id, { id: id },
//                  function (res) {
//                      if (res.status == 0) {
//                          showLoadingMsg("操作成功", { right: 100, top: 10 }, true, 5000);
//                          xjgrid.Reload();
//                      }
//                      else {
//                          showErrorTip("操作失败！：" + res.message, { right: 100, top: 10 }, true, 5000);
//                      }
//                  },
//                  "json"
//            );
//        })
        util.CheckIn = function (item) {
            var cardtype = item.UserCardType;
            if (cardtype == "1") { // 会员的话，登录
                location.href = opt.checkInUrl + "?memberid=" + item.MemberId + "&bid=" + item.Idx + "&cardno=" + item.UserCard + "&roomid=" + item.BedId;

                /*
                var url = opt.loginUrl + "?usercard=" + item.UserCard + "&memberid=" + item.MemberId + "&bid=" + item.Idx+"&roomid="+item.BedId;
                window.xjDailog.Open(url, {
                    width: 500,
                    height: 280,
                    caption: '会员登录',
                    theme: "simple" //默认主题
                });
                */
            }
            else { //非会员的话 填写一些信息
                location.href = opt.tempLoginUrl + StrFormat("?fullname={0}&idcard={1}&phone={2}&memberid={3}&bid={4}&cardno={5}&roomid={6}",
                    [item.UserFullName, item.IdCard, item.MobilePhone, item.MemberId, item.Idx,item.UserCard,item.BedId]);
            }
        }
        util.Edit = function (item) {
            var idx = item.Idx;
            var url = opt.editUrl + "/" + idx;
            window.xjDailog.Open(url, {
                width: 650,
                height: 480,
                caption: '修改预约',
                theme: "simple", //默认主题
                onclose: function (userstate) {
                    xjgrid1.Reload();
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        }
        util.Cancel = function (item) {
            if (confirm("确认要取消这条预约记录吗？")) {
                $.post(opt.setStatusUrl, { idx: item.Idx, type: 1 },
                    function (res) {
                        if (res.status == 0) {
                            xjgrid1.Reload();
                        }
                        else {
                            showErrorTip("操作失败！：" + res.message, { right: 100, top: 10 }, true, 5000);
                        }
                    },
                    "json"
                 );
            }
        }
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
