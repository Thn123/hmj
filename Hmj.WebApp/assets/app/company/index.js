; (function (window, undefined, $) {

    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '公司编号', name: 'ORG_NO', width: "10%", sortable: true, hide: false, align: 'left', iskey: true },
                    { display: '公司名称', name: 'ORG_NAME', width: "25%", sortable: false, align: 'left' },
                    { display: '门店数', name: 'STORE_COUNT', width: "10%", sortable: false, align: 'left' },
                    { display: '城市', name: 'CITY', width: "15%", sortable: false, align: 'left' },
                    { display: '区域', name: 'REGION', width: "15%", sortable: false, align: 'left' },                    
                    { display: '短信剩余数', name: 'STORE_COUNT', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
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
            ops.push("&nbsp;<a title='编辑公司信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            //ops.push("&nbsp;&nbsp;<a title='删除公司信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }
         
        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            window.location.href = options.editUrl;
            //var url = options.editUrl;
            //window.xjDailog.Open(url, {
            //    width: 680,
            //    height: 570,
            //    caption: '新增公司信息',
            //    theme: "simple", //默认主题
            //    onclose: function (userstate) {
            //        xjgrid.Reload();

            //    } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            //});
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
        util.Edit = function (id) {
            var url = options.editUrl + "/" + id;
            window.location.href = url;
            //window.xjDailog.Open(url, {
            //    width: 680,
            //    height: 570,
            //    caption: '编辑公司信息',
            //    theme: "simple", //默认主题
            //    onclose: function (userstate) {
            //        xjgrid.Reload();
            //    } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            //});
        };
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };
    });

})(window, undefined, jQuery);