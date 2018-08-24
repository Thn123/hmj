; (function (window, undefined, $) {

    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '内容', name: 'CONTENT', width: "15%", sortable: true, hide: false, align: 'left', iskey: true },
                    { display: '时间', name: 'SEND_TIME', width: "25%", sortable: false, align: 'left' },
                    { display: '生成员', name: 'CREATE_USER', width: "10%", sortable: false, align: 'left' }
            ],
            sortname: "ID",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑班次信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除班次信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[0], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }
        function formatColor(value, cell) {
            return "<a title=" + value + " style='float:left;width:60px;height:25px;;background-color:" + value + "'></a>&nbsp;[" + value + "]";
        }
        //颜色选择器
        $('.cpicker').colorpicker();
        $('#BEGINTIME').timepicker();
        $('#ENDTIME').timepicker();

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });
        $("#btnSave").click(function () {
            var id = $("#SCHID").val();
            var name = $("#NAME").val();
            var color = $("#COLOR").val();
            var btime = $("#BEGINTIME").val();
            var etime = $("#ENDTIME").val();
            $.post(options.editUrl, { ID: id, NAME: name, COLOR: color, BEGINTIME: btime, ENDTIME: etime },
                function (ret) {
                    if (ret.status > 0) {
                        _showInfoMessage('操作成功！', 'success');
                        $('#EditModal').modal('hide');
                        xjgrid.Reload();
                    }
                    else {
                        _showInfoMessage('操作失败！' + ret.message, 'error');
                        return false;
                    }
                },
                "json"
               );
        });
        $("#btnAdd").click(function (e) {
            $('#frmSave')[0].reset();
            $("#SCHID").val(0);
            $('#EditModal').modal('show');
        });
        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#SCHID").val();

            $.post(options.deleteUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage('操作成功！', 'success');
                          xjgrid.Reload();
                      }
                      else {
                          _showInfoMessage('操作失败！' + ret.message, 'error');
                          return false;
                      }
                  },
                  "json"
            );
        })
        util.Edit = function (id) {

            $.post(options.detailUrl, { id: id },
                function (ret) {
                    if (ret.status > 0) {
                        var data = eval(ret.data);
                        $("#SCHID").val(id);
                        $("#NAME").val(data.NAME);
                        $("#COLOR").val(data.COLOR);
                        $("#BEGINTIME").val(data.BEGINTIME);
                        $("#ENDTIME").val(data.ENDTIME);
                        //$("#divColor").attr("data-color", data.COLOR);
                    }
                },
                "json"
               );
            $('#EditModal').modal('show');

        };
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#SCHID").val(id);
            $('#confirmModal').modal('show');
        };
    });

})(window, undefined, jQuery);