; (function (window, undefined, $) {
    $(document).ready(function () {

        var gridopt = {
            url: options.queryRightUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '名称', name: 'CATE_NAME', width: "55%", sortable: false, align: 'left' },
                    { display: '编号', name: 'CATE_NO', width: "15%", sortable: false, align: 'left' },
                    { display: '排序', name: 'CATE_SEQ', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "ID",
            sortorder: "ASC",
            title: false,
            //rp: 15,
            usepager: false,
            showcheckbox: false,
            autoload: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);

        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑类别信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除类别信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        function SetRightDict(id) {
            xjgrid.QueryByFields([
                { name: "id", value: id }
            ]);
        }

        SetRightDict($("#PARENT_ID").val());

        $("#ptree").treeview({
            url: options.queryLeftUrl,
            showcheck: true,
            cascadecheck: true,
            onnodeclick: TreeNode_Click,
            //theme: "bbit-tree-lines" //bbit-tree-lines ,bbit-tree-no-lines,bbit-tree-arrows
        });

        function TreeNode_Click(data) {
            //if (data.pid == "00000000-0000-0000-0000-000000000000") {

            $("#h4Dict").html(data.text);
            $("#PARENT_ID").val(data.id);
            $("#PARENT_NAME").html(data.text);
            $("#PROD_TYPE").val(data.pid);
            SetRightDict(data.id);
            //data.expand();
            if (data.id == "00000000-0000-0000-0000-000000000000") {
                $("#btnAdd").hide();
            }
            else
                $("#btnAdd").show();
            //}
        }

        $("#btnAdd").click(function (e) {
            $('#frmSave')[0].reset();
            $("#ID").val("00000000-0000-0000-0000-000000000000")
            $("#CATE_NO").removeAttr("readonly");
            $('#EditModal').modal('show');
        });

        util.Edit = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {
                         $("#ID").val(ret.data.ID);
                         $("#CATE_NO").val(ret.data.CATE_NO);
                         $("#CATE_NAME").val(ret.data.CATE_NAME);
                         $("#PARENT_NAME").html(ret.data.PARENT_NAME);
                         $("#CATE_SEQ").val(ret.data.CATE_SEQ);
                         $("#PARENT_ID").val(ret.data.PARENT_ID);
                         $("#PROD_TYPE").val(ret.data.PROD_TYPE);
                         $("#CATE_NO").attr("readonly", "readonly");
                     }
                     else {
                         _showInfoMessage("数据库中没有此数据字典，请刷新重试！", 'error');
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
                '#CATE_NO': 'required',
                '#CATE_NAME': 'required',
                '#CATE_SEQ': 'required;number',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success');
                        $('#EditModal').modal('hide');
                        SetRightDict($("#PARENT_ID").val());

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
                          _showInfoMessage("操作成功！", 'success');
                          SetRightDict($("#PARENT_ID").val());
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