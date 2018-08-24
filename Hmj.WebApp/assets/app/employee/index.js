; (function (window, undefined, $) {
    $(document).ready(function () {

        $("#USER_TYPE").val("");

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '工号', name: 'EMPLOYEE_NO', width: "10%", sortable: true, hide: false, align: 'left', iskey: true },
                    { display: '姓名', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    //{ display: '所属区域', name: 'STORE_NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '状态', name: 'STATUS', width: "8%", sortable: false, align: 'left', process: formatSTATUS },
                    //{ display: '类型', name: 'EMP_TYPE', width: "7%", sortable: false, align: 'left', process: formatTYPE },
                    { display: '员工级别', name: 'POSITIONNAME', width: "15%", sortable: false, align: 'left' },
                    { display: '电话', name: 'MOBILE', width: "20%", sortable: false, align: 'left' },
                    { display: '创建日期', name: 'ENTRY_DATE', width: "12%", sortable: false, align: 'left', process: formatDate },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id", 
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false,
            autoload: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑员工信息' class='abtn' href='javascript:;' onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除员工信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
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

        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0].replace("/", "-").replace("/", "-");
            }
            else {
                return "-"
            }
        }

        function formatSTATUS(value, cell) {
            if (value == "1") {
                return "在职";
            }
            else if (value == "0") {
                return "离职";
            }
        }

        function formatTYPE(value, cell) {
            if (value == "1") {
                return "全职员工";
            }
            else if (value == "2") {
                return "兼职员工";
            }
        }
        $("#ptree").treeview({
            url: options.queryLeftUrl,
            showcheck: true,
            cascadecheck: true,
            onnodeclick: TreeNode_Click,
        });

        var cuTDs = $("#ptree").getTreeData();
        if (cuTDs != undefined && cuTDs[0] != undefined) {
            //默认加载第一个菜单数据
            $("#USER_TYPE").val(cuTDs[0].data.orgLevel);
            $("#STORE_ID").val(cuTDs[0].value);
            $("#h4Dict").html(cuTDs[0].text);
        }

        function TreeNode_Click(data) {
            $("#h4Dict").html(data.text);
            $("#USER_TYPE").val(data.data.orgLevel);
            $("#STORE_ID").val(data.value);
            xjgrid.Query($("#formQuery")[0]);
        }

        xjgrid.Query($("#formQuery")[0]);
        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            window.location.href = options.editUrl + "?userType=" + $("#USER_TYPE").val() + "&storeId=" + $("#STORE_ID").val();
        });

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl, { id: id },
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

        util.Edit = function (id) {
            window.location.href = options.editUrl + "/" + id;
        }
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };
         
    });

})(window, undefined, jQuery);