; (function (window, undefined, $) {

    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '名称', name: 'NAME', sortable: true, hide: false, align: 'left', iskey: true },
                    { display: '项目折扣', name: 'DISCOUNT_RATE', sortable: false, align: 'left' },
                     { display: '产品折扣', name: 'DISCOUNT_RATE2', sortable: false, align: 'left' },
                    { display: '类型', name: 'TYPE', sortable: false, align: 'left',process:formatType },
                    { display: '有效期', name: 'PERIOD', sortable: false, align: 'left' },
                    { display: '说明', name: 'REMARK', sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', sortable: false, align: 'center', process: processOp }
                 
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑会员卡信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除会员卡信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[0], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
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
                return "会员卡";
            }
            else if (value == "3") {
                return "折扣卡";
            }
            else if (value == "2") {
                return "疗程卡";
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
        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            var url = options.editUrl;
            location.href = url;
        });
        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCardId").val();
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
            location.href = url;
        };
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCardId").val(id);
            $('#confirmModal').modal('show');
        };
    });

})(window, undefined, jQuery);