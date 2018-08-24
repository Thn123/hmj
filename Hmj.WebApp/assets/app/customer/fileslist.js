; (function (window, undefined, $) {
    $(document).ready(function () {
        var gridopt = {
            url: options.linkUrl,
            colModel: [
                    { display: '编号', name: 'ID',  sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '文件名称', name: 'CONTENT_TYPE', sortable: false, align: 'left' },
                    { display: '后缀名', name: 'Zui', sortable: false, align: 'left' },
                    { display: '文件类型', name: 'FILES_NAME', sortable: false, align: 'left' },
                    { display: '文件大小', name: 'FILE_SIZE',  sortable: false, align: 'left' },
                    { display: '查看', name: 'FILES_ID', sortable: false, align: 'left', process: processOp },
                    { display: '删除', name: 'ID', sortable: false, align: 'left', process: processOp2 }
            ],
            sortname: "ID",
            sortorder: "desc",
            title: false,
            rp: 20,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='查看' class='abtn' href='javascript:;'  onclick=\"javascript:window.open('/Customer/ViewImage.do/", value, "')\"><i class='fa fa-search' ></i> 查看</a>");
            return ops.join("");
        }
        function processOp2(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='删除'  abbr='6' href='javascript:;'  onclick=\"util.Cancel(", value, ")\"><i class='fa fa-trash-o' ></i> 删除</a>");
            return ops.join("");
        }

        util.Cancel = function (item) {
            if (confirm("确认要删除这个附件吗？")) {
                $.post("/Customer/deletefile.do", { idx: item, type: 1 },
                    function (res) {
                        if (res.status == 0) {
                           // alert("sdfds");
                            gridlist.Reload();
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
