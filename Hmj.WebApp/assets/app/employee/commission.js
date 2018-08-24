; (function (window, undefined, $) {

    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0%", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '名称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '状态', name: 'STATUS', width: "10%", sortable: false, align: 'left', process: formatSTATUS },
                    { display: '说明', name: 'REMARK', width: "25%", sortable: false, align: 'left' },
                    { display: '开始日期', name: 'BEGIN_DATE', width: "15%", sortable: false, align: 'left', process: formatDate },
                    { display: '结束日期', name: 'END_DATE', width: "15%", sortable: false, align: 'left', process: formatDate },
                    { display: '类别', name: 'CMODE', width: "10%", sortable: false, hide: false, align: 'left', process: formatType },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp },
                    
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
            ops.push("&nbsp;<a title='编辑佣金方案信息' class='abtn' href='javascript:;' onclick=\"util.Edit('", value, "',", cell[6], ")\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除佣金方案信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");

            return ops.join("");
        }
        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0].replace("/", "-").replace("/", "-");
            }
            else {
                return "-";
            }
        }

        function formatSTATUS(value, cell) {
            if (value == 0) {
                return "关闭";
            }
            else if (value == 1) {
                return "开启";
            }
        }

        function formatType(value, cell) {
            if (value == 1) {
                return "项目";
            }
            else if (value == 2) {
                return "业绩";
            }
         
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功", 'success');
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

    util.Edit = function (id, type) {         
        if (type == 1)
            window.location.href = options.editUrl + "/" + id;
        else if (type == 2)
            window.location.href = options.editPFMCUrl + "/" + id;
    }
    util.Delete = function (id, name) {
        $("#lbuserName").html(name);
        $("#hdCurrentId").val(id);
        $('#confirmModal').modal('show');
    };

    $("#btnAdd").click(function (e) {
        $('#EditModal').modal('show');
        //window.location.href = options.editUrl;
    });

    $("#btnProj").click(function (e) {
        window.location.href = options.editUrl;
    });

    $("#btnYj").click(function (e) {
        window.location.href = options.editPFMCUrl;
    });

})(window, undefined, jQuery);