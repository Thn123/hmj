; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'Id', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '区域', name: 'Name', width: "15%", sortable: false, align: 'left' },
                    { display: '登陆账号', name: 'User_No', width: "10%", sortable: false, align: 'left' },
                    { display: '登陆密码', name: 'User_Pass', width: "10%", sortable: false, align: 'left' },
                    { display: '是否显示', name: 'IsShow', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "10%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);

        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑' class='abtn' href='AreaEdit.do?Id=" + cell[0] + "' target='_blank'><i></i>编辑</a>");
            if (cell[4] == "是") {
                ops.push("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a title='关闭' class='abtn' href='javascript:;'  onclick=\"util.UpdateShow('", cell[0], "','关闭')\"><i></i>关闭</a>");
            } else {
                ops.push("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a title='开启' class='abtn' href='javascript:;'  onclick=\"util.UpdateShow('", cell[0], "','开启')\"><i></i>开启</a>");
            }

            return ops.join("");
        }

        $("#btnAdd").click(function (e) {
            location = "AreaEdit.do?Id=0";
        });
        util.UpdateShow = function (id, IsShow) {
            $("#hdAreaId").val(id);
            $("#hdIsShow").val(IsShow);
            $('#confirmModal').modal('show');
        };

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdAreaId").val();
            var IsShow = $("#hdIsShow").val();
            $.post(options.updateshowUrl + "/" + id, { id: id, IsShow: IsShow },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功！", 'success');
                          xjgrid.Reload();
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
        })
    });

})(window, undefined, jQuery);