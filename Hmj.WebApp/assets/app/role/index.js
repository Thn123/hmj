; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ROLE_ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '名称', name: 'ROLE_NAME', width: "30%", sortable: false, align: 'left' },
                    { display: '说明', name: 'REMARK', width: "55%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ROLE_ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "ROLE_ID",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑角色信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除角色信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            $("#ROLE_ID").val(0);
            $("#usertree").treeview(
             {
                 url: options.getTreeList + "?orgId=" + $("#ORG_ID").val() + "&roleId=0" + $("#ROLE_ID").val(),
                 showcheck: true,
                 cascadecheck: true,
                 onnodeclick: TreeNode_Click,
                 theme: "bbit-tree-no-lines" //bbit-tree-lines ,bbit-tree-no-lines,bbit-tree-arrows
             }
    );
       
            $('#frmSave')[0].reset();
            $('#EditModal').modal('show');
        });

        util.Edit = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {
                         $("#ROLE_ID").val(ret.data.ROLE_ID);
                         $("#ROLE_NAME").val(ret.data.ROLE_NAME);
                         $("#REMARK").val(ret.data.REMARK);

                         $("#usertree").treeview(
                {
                    
                    url: options.getTreeList + "?orgId=" + $("#ORG_ID").val() + "&roleId=" + $("#ROLE_ID").val(),
                    showcheck: true,
                    cascadecheck: true,
                    onnodeclick: TreeNode_Click,
                    theme: "bbit-tree-no-lines" //bbit-tree-lines ,bbit-tree-no-lines,bbit-tree-arrows
                }
       );
                     }
                     else {
                         _showInfoMessage("数据库中没有此角色，请刷新重试！", 'error'); 
                     }
                 },
                 "json"
           );
            $('#EditModal').modal('show');
        };         

        function TreeNode_Click(data) {
            data.expand();
        }

        $("#btnSave").click(function (e) {

            var teeSd = $("#usertree").getTSVs();
            if (teeSd == "") {
                _showInfoMessage("请选择权限", 'error'); 
                return;
            }
            $("#AddIDS").val(teeSd);
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#ROLE_NAME': 'required',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success'); 
                        $('#EditModal').modal('hide');
                        xjgrid.Reload();
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

    });

})(window, undefined, jQuery);