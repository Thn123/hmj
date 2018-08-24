; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '名称', name: 'ORG_NAME', width: "30%", sortable: false, align: 'left' },
                    //{ display: '说明', name: 'Abstract', width: "55%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑商户信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            //ops.push("&nbsp;&nbsp;<a title='删除商户信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
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
                         $("#ID").val(ret.data.ID);
                         $("#Name").val(ret.data.Name);
                         $("#Abstract").val(ret.data.Abstract);
                         $("#ToUserName").val(ret.data.ToUserName);
                         $("#AppID").val(ret.data.AppID);
                         $("#Appsecret").val(ret.data.Appsecret);
                         $("#OneOpenID").val(ret.data.OneOpenID);
                         $("#DOMAIN_PREFIX").val(ret.data.DOMAIN_PREFIX);
                         $("#TemplateID1").val(ret.data.TemplateID1);
                         $("#TemplateID2").val(ret.data.TemplateID2);
                         $("#TemplateID3").val(ret.data.TemplateID3);
                         $("#TemplateID4").val(ret.data.TemplateID4);
                         $("#img1").attr("src", ret.data.MEM_PIC);
                     }
                     else {
                         _showInfoMessage("数据库中没有此商户，请刷新重试！", 'error');
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
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
              
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success');
                        if (confirm("商户信息保存后必须需重新登录\n系统后才能正常使用其他功能,\n是否重新登录？")) {
                            location = "/Home/Login.do";
                        }
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