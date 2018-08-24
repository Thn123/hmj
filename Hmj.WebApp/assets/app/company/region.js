; (function (window, undefined, $) {

    $(document).ready(function () {

        $("#ptree").treeview({
            url: options.queryLeftUrl,
            showcheck: true,
            cascadecheck: true,
            onnodeclick: TreeNode_Click,
        });

        var cuTDs = $("#ptree").getTreeData();
        if (cuTDs != undefined && cuTDs[0] != undefined) { 
            //默认加载第一个菜单数据
            $("#PARENT_ID").val(cuTDs[0].id);
            $("#PARENT_NAME").html(cuTDs[0].text);
            $("#h4Dict").html(cuTDs[0].text);
        }
        function TreeNode_Click(data) {
            $("#PARENT_ID").val(data.id);
            $("#PARENT_NAME").html(data.text);
            $("#h4Dict").html(data.text);
          
            SetRightDict();
        }

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '区域编号', name: 'ORG_NO', width: "15%", sortable: true, hide: false, align: 'left', iskey: true },
                    { display: '区域名称', name: 'ORG_NAME', width: "35%", sortable: false, align: 'left' },
                    { display: '门店数', name: 'STORE_COUNT', width: "10%", sortable: false, align: 'left' },
                    { display: '等级', name: 'ORG_LEVEL_STR', width: "10%", sortable: false, align: 'left' },
                    { display: '联系人', name: 'CONTACT', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "ID",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
            autoload: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);

        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑区域信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除区域信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        function SetRightDict() {
            xjgrid.QueryByFields([
                { name: "REGION_NAME1", value: $("#REGION_NAME1").val() },
                { name: "ORG_ID", value: $("#PARENT_ID").val() }
            ]);
        }
        SetRightDict();

        $("#formQuery").submit(function () {
            SetRightDict();
            return false;
        });

        $("#btnAdd").click(function (e) {
            //$("#REGION_NO").removeAttr("readonly");
            $("#ID").val(0);
            $('#frmSave')[0].reset();
            $('#EditModal').modal('show');
        });
        util.Edit = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                    function (ret) {
                        if (ret && ret.status == 0) {
                            //$("#REGION_NO").attr("readonly", "readonly");

                            $("#ID").val(ret.data.ID);
                            $("#ORG_NO").val(ret.data.ORG_NO);
                            $("#ORG_NAME").val(ret.data.ORG_NAME);
                            $("#CONTACT").val(ret.data.CONTACT);
                            $("#TELEPHONE").val(ret.data.TELEPHONE);
                            $("#FAX").val(ret.data.FAX);
                            $("#ADDRESS").val(ret.data.ADDRESS);
                            $("#REMARK").val(ret.data.REMARK);
                            $("#PARENT_NAME").html(ret.data.PARENT_NAME);
                            $("#ORG_LEVEL").val(ret.data.ORG_LEVEL);
                            $("#PARENT_ID").val(ret.data.PARENT_ID);
                        }
                        else {
                            _showInfoMessage("数据库中没有此区域，请刷新重试！", 'error');
                        }
                    },
                    "json"
              );
            $('#EditModal').modal('show');
        };

        $("#btnSave").click(function (e) {
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#ORG_NO': 'required',
                '#ORG_NAME': 'required',
                '#ORG_LEVEL': 'required',
                '#CONTACTS': 'required',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success');
                        $('#EditModal').modal('hide');

                        SetRightDict();

                        $("#ptree").treeview({
                            url: options.queryLeftUrl,
                            showcheck: false,
                            cascadecheck: true,
                            onnodeclick: TreeNode_Click,
                            theme: "bbit-tree-no-lines" //bbit-tree-lines ,bbit-tree-no-lines,bbit-tree-arrows
                        });
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                })
            }
        });
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功", 'success');
                          SetRightDict();
                          $("#ptree").treeview({
                              url: options.queryLeftUrl,
                              showcheck: false,
                              cascadecheck: true,
                              onnodeclick: TreeNode_Click,
                              theme: "bbit-tree-no-lines" //bbit-tree-lines ,bbit-tree-no-lines,bbit-tree-arrows
                          });
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