; (function (window, undefined, $) {
    $(document).ready(function () {

        $('#FirstTime').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        $('#EndTime').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '姓名', name: 'NAME', width: "15%", sortable: false, align: 'left' },
                    { display: '手机', name: 'MOBILE', width: "15%", sortable: true, align: 'left' },
                    { display: '大区', name: 'AreaName', width: "15%", sortable: true, align: 'left' },
                    { display: '门店', name: 'StoreName', width: "15%", sortable: true, align: 'left' },
                    { display: '绑定人数', name: 'FansNum', width: "15%", sortable: true, align: 'left' },
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            localpage: true,
            showcheckbox: false,
            autoload: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑员工信息' class='abtn' href='javascript:;' onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            // ops.push("&nbsp;&nbsp;<a title='删除员工信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            ops.push("&nbsp;<a title='调迁及日志' class='abtn' href='javascript:;' onclick=\"util.ChangeHis('", value, "')\"><i class='fa fa-edit' ></i>调迁及日志</a>");
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
            onnodeclick: nodeClick,
        });

        var cuTDs = $("#ptree").getTreeData();
        if (cuTDs != undefined && cuTDs[0] != undefined) {
            //默认加载第一个菜单数据
            $("#h4Dict").html(cuTDs[0].text);
        }

        function TreeNode_Click(data) {
            $("#h4Dict").html(data.text);
            $("#USER_TYPE").val(data.data.orgLevel);
            $("#STORE_ID").val(data.value);
            xjgrid.Query($("#formQuery")[0]);
        }

        function nodeClick(data) {
            $("#h4Dict").html(data.text);
            $("#StoreName").val(data.text);
            $("#DeptId").val(data.value);
            xjgrid.Query($("#formQuery")[0]);

        }

        xjgrid.Query($("#formQuery")[0]);
        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            window.location.href = options.editUrl + "?StoreName=" + $("#StoreName").val() + "&FirstTime=" + $("#FirstTime").val() + "&FirstTime=" + $("#FirstTime").val();
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

        util.ChangeHis = function (id) {
            window.location.href = options.changeHisUrl + "/" + id;
        }

        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };

        $("#btnUpload").click(function () {
            $.ajax({
                url: options.uploadAllEmployeesUrl,
                //data: { filePath: $(this).val() },
                type: "POST",
                success: function (result) {
                    alert("a");
                },
                error: function (result) {
                    alert("b");
                }
            });
            $("#formUpload").submit();
        });

     //   $('input[type=file]').bootstrapFileInput();
        $("#btnOpenUploadModal").click(function () {
            $('#uploadModal').modal('show');
        });

        $("#btnStartUpload").click(function () {
            var flag = confirm("导入会占用大量时间，门店忙时请谨慎执行此操作，确认如此做吗?")
            if (flag) {
                var excel = $("input[type='file'][name='excel']");
                if (excel.val() == "" || excel.length == 0) {
                    _showInfoMessage("请选择文件", 'error');
                    return;
                }
                $('#uploadModal').modal('hide');
                $("#btnOpenUploadModal").attr("disabled", true)
                $("#btnOpenUploadModal").text("");
                $("#btnOpenUploadModal").append("<i class=\"fa fa-plus\"></i>导入中");

                $.ajaxFileUpload({
                    type: 'post',
                    fileElementId: 'excel',
                    url: options.uploadAllEmployeesUrl,
                    dataType: 'json',
                    success: function (data, status) {
                        if (data.status > 0) {
                            alert(data.message);
                            xjgrid.Reload();
                        }
                        else {
                            alert("操作失败：" + data.message);
                        }
                        $("#btnOpenUploadModal").attr("disabled", false)
                        $("#btnOpenUploadModal").text("");
                        $("#btnOpenUploadModal").append("<i class=\"fa fa-plus\"></i>导 入");
                    },
                    error: function (data, status, e) {
                        alert(e);
                        $("#btnOpenUploadModal").attr("disabled", false)
                        $("#btnOpenUploadModal").text("");
                        $("#btnOpenUploadModal").append("<i class=\"fa fa-plus\"></i>导 入");
                    }
                });
            }
        });

        $("#btnExportFile").click(function () {

            window.location.href = "/EmplReport/ExportEmployeeList.do?StoreName=" + $("#StoreName").val() + "&FirstTime=" + $("#FirstTime").val() + "&EndTime=" + $("#EndTime").val() ;

        });


    });

})(window, undefined, jQuery);