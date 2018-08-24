; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '门店编号', name: 'Code', width: "5%", sortable: false, align: 'left' },
                    { display: '门店名', name: 'Name', width: "15%", sortable: false, align: 'left' },
                    { display: '省份', name: 'Province', width: "5%", sortable: false, align: 'left' },
                    { display: '城市', name: 'City', width: "5%", sortable: false, align: 'left' },
                    { display: '地区', name: 'Area', width: "5%", sortable: false, align: 'left' },
                    //{ display: '坐标X', name: 'X', width: "10%", sortable: false, align: 'left' },
                    //{ display: '坐标Y', name: 'Y', width: "10%", sortable: false, align: 'left' },
                    { display: '号码', name: 'Phone', width: "10%", sortable: false, align: 'left' },
                    { display: '门店类别', name: 'StoreType', width: "5%", sortable: false, align: 'left', process: processE },
                    { display: '所属大区', name: 'BelongsAreaName', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);


        function processE(value, cell) {
            var ops = [];
            if (value == 1) {
                ops.push("<p>销售门店</p>");
            } else {
                ops.push("<p>维修门店</p>");
            }
            return ops.join("");
        }

        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑' class='abtn' href='StoreEdit.do?Id=" + value + "' target='_blank'><i class='fa fa-edit'></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }
        util.Delete = function (id) {
            $("#hdId").val(id);
            $('#confirmModal').modal('show');
        };

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdId").val();
            $.post(options.deleteUrl + "/" + id, { id: id },
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
        $("#btnAdd").click(function (e) {
            location = "StoreEdit.do?Id=0";
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