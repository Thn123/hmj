; (function (window, undefined, $) {
    var submiting = false;
    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', sortable: true, hide: false, align: 'left', iskey: true },
                    { display: '名称', name: 'NAME', sortable: false, align: 'left' },
                    { display: '类别', name: 'TYPE', sortable: false, align: 'left' },
                    { display: '剩余数量', name: 'ID', sortable: false, align: 'left' },
                    { display: '库存水平', name: 'ID', sortable: false, align: 'left' },
                    { display: '状态', name: 'STATUS', sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', sortable: false, align: 'center', process: processOp }
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
            ops.push("&nbsp;<a title='编辑产品信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除产品信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");

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
        function formatType(value, cell) {
            if (value == "1") {
                return "正装产品";
            }
            else if (value == "2") {
                return "赠品";
            }
            else if (value == "3") {
                return "耗材";
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
        function clear() {
            $("#ID").val("");
            $("#NAME").val("");
            $("#MOBILE").val("");
            $("#REMARK").val("");
            $('input:radio:first').attr('checked', 'checked');
            $('#EditModal').modal('show');
        }
        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            var url = options.editUrl;
            window.xjDailog.Open(url, {
                width: 500,
                height: 500,
                caption: '新增产品信息',
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
                width: 480,
                height: 420,
                caption: '编辑产品信息',
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
            alert("跳转到客户360页面")
        };
    });

    //验证
    $('#frmSave').validator({
        rules: {
        },
        fields: {
            '#NAME': 'required',
            '#MOBILE': 'required',
            '#vEMP_TYPE': 'required',
            '#vSERVICE_LVL': 'required',
            '#vPOSITION': 'required'
            //'#Store_ID': 'required;idcard',

        },
        valid: function (form) {
            //if (submiting) {
            //    return;
            //}
            //submiting = true;
            FormSubmit(form, function (res) {
                if (res.status == 0) {
                    alert("操作成功！");
                    $("#btnClose").click();
                    $('#frmSave')[0].reset()
                    $("#formQuery").submit();
                    //if (window.opener) {
                    //    window.opener.location.reload();
                    //    window.close();
                    //}
                    //else {
                    //    if (parent && parent.xjDailog) {
                    //        parent.xjDailog.Close(false, true, null);
                    //    }
                    //    else {
                    //        window.close();
                    //    }
                    //}
                }
                else {
                    showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                }
                submiting = false;
            })
        }
    });
    $("#btnSave").click(function (e) {
        $("#frmSave").submit();
    });
    //function addGoods(a1,a2,a3,a4)
    //{
    //    alert(a1);

    //}
})(window, undefined, jQuery);