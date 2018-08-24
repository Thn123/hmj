; (function (window, undefined, $) {

    $(document).ready(function () {

        if ($("#ISADMIN").val() == "True") {
            $("#storeDiv").show();
        }
        else {
            $("#storeDiv").hide();
        }

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "10%", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '策略名称', name: 'ROOM_NO', width: "10%", sortable: false, align: 'left' },
                    { display: '版本号', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '策略类型', name: 'STORE_NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '策略描述', name: 'REMARK', width: "20%", sortable: false, align: 'left' },
                    { display: '创建人', name: 'REMARK', width: "20%", sortable: false, align: 'left' },
                    { display: '创建日期', name: 'REMARK', width: "20%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
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
            ops.push("&nbsp;<a title='编辑' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            $("#ID").val(0);
            $('#frmSave')[0].reset();
            $('#EditModal').modal('show');
        });

        $("#x1").click(function (e) {         
            $('#EditModal').modal('show');
        });

        $("#x2").click(function (e) {
            $('#EditModal').modal('show');
        });

        $("#x3").click(function (e) {
            $('#EditModal').modal('show');
        });

        $("#x4").click(function (e) {
            $('#EditModal').modal('show');
        });

        $("#x5").click(function (e) {
            $('#EditModal').modal('show');
        });

        $("#d1").click(function (e) {
            $('#confirmModal').modal('show');
        });
        $("#d2").click(function (e) {
            $('#confirmModal').modal('show');
        });
        $("#d3").click(function (e) {
            $('#confirmModal').modal('show');
        });
        $("#d4").click(function (e) {
            $('#confirmModal').modal('show');
        });
        $("#d5").click(function (e) {
            $('#confirmModal').modal('show');
        });



       
        util.Edit = function (id) {
           // $.post(options.editUrl, {
           //     id: id
           // },
           //      function (ret) {
           //          if (ret && ret.status == 0) { 
           //              $("#ID").val(ret.data.ID);
           //              $("#ROOM_NO").val(ret.data.ROOM_NO);
           //              $("#STORE_ID").val(ret.data.STORE_ID);
           //              $("#STORE_ID1").val(ret.data.STORE_ID);
           //              $("#NAME").val(ret.data.NAME);
           //              $("#BED_NUM").val(ret.data.BED_NUM);
           //              $("#REMARK").val(ret.data.REMARK);
           //          }
           //          else {
           //              _showInfoMessage("数据库中没有此房间信息，请刷新重试！" , 'error'); 
           //          }
           //      },
           //      "json"
           //);
            $('#EditModal').modal('show');
        };

        $("#btnSave").click(function (e) {
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#ROOM_NO': 'required',
                '#NAME': 'required',
                '#BED_NUM': 'required;digits',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success'); 
                        $('#EditModal').modal('hide');
                        xjgrid.Reload();
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error'); 
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                })
            }
        });

        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };
        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl + "/" + id, { id: id },
                  function (res) {
                      showErrorTip(res.message, { right: 100, top: 10 }, true, 5000);
                      if (res.status == 0) {
                          _showInfoMessage("删除成功！", 'success'); 
                          xjgrid.Reload();
                      }
                      else {
                          _showInfoMessage("删除失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
        })

    });
})(window, undefined, jQuery);