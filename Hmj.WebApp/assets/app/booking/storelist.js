; (function (window, undefined, $) {
    $(document).ready(function () {

        var gridopt = {
            url: options.queryStoreListUrl,
            colModel: [
                    { display: 'Id', name: 'ID', width: "0%", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '店名', name: 'Name', width: "20%", sortable: false, align: 'center' },
                    { display: '预约短信通知', name: 'NotificationTel', width: "30%", sortable: false, align: 'center' },
                    { display: '创建时间', name: 'CeateTime', width: "20%", sortable: false, align: 'center' },
                    { display: '状态', name: 'IsDisplay', width: "10%", sortable: false, align: 'center', process: formateIsDisplay },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp },
            ],
            sortname: "ID",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: false,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);

        function formateIsDisplay(value, cell) {
            if (value.toLowerCase() == "false") {
                return "下架";
            }
            else if (value.toLowerCase() == "true") {
                return "正常";
            }
        };

        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑' class='abtn' href='javascript:;' onclick=\"util.edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            return ops.join("");
        }

        util.edit = function edit(id) {
            $('#EditModal').modal('show');
            $.post(options.getStoreUrl + "/" + id, { id: id },
                  function (res) {
                      $('input:radio[name=IsDisplay]').attr('checked', false);
                      if (res.status == 0) {
                          var store = res.data;
                          $("#frmEdit input[type='hidden'][name='ID']").val(store.ID);
                          $("#frmEdit").find("#Name").val(store.Name);
                          $("#frmEdit").find("#NotificationTel").val(store.NotificationTel);
                          $("#frmEdit").find("#Province").val(store.Province);
                          $("#frmEdit").find("#City").val(store.City);
                          $("#frmEdit").find("#Region").val(store.Region);
                          $("#frmEdit").find("#Address").val(store.Address);
                          $("#frmEdit").find("#Manager").val(store.Manager);
                          $("#frmEdit").find("#Telephone").val(store.Telephone);

                          if (store.IsDisplay) {
                              $('input:radio[name=IsDisplay]')[1].checked = true;
                          }
                          else {
                              $('input:radio[name=IsDisplay]')[0].checked = true;
                          }
                      }
                  },
                  "json"
            );
        }

        $("#btnSave").click(function () {
            $("#frmEdit").submit();
        });

        $('#frmEdit').validator({
            rules: {},
            fields: {
                Name: "required",
                //OrderNum: "integer[+]",
                NotificationTel: "mobile",
            },
            valid: function (form) {
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
                if (form["IsDisplay"].value == "1") {
                    form["IsDisplay"].value = true;
                }
                else if (form["IsDisplay"].value == "0") {
                    form["IsDisplay"].value = false;
                }
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        xjgrid.Reload();
                        $('#EditModal').modal('hide');
                    }
                    hideLoadingMsg();
                })
            }
        });

        $("#btnImport").click(function () {
            if (confirm("确认要从POS中同步门店信息吗？")) {
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
                $.post(options.importStoresUrl, null,
                         function (ret) {
                             if (ret.status == 0) {
                                 _showInfoMessage("同步成功<br />添加了" + ret.data.addcount + "个门店<br />更新了" + ret.data.updatecount +
                                     "个门店<br />删除了" + ret.data.deletecount + "个门店", 'success');
                                 xjgrid.Reload();
                             }
                             hideLoadingMsg();
                         }, "json");
            };
        });
    });
})(window, undefined, jQuery);