; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '图文名称', name: 'Name', width: "20%", sortable: false, align: 'left' },
                    { display: '图文标题', name: 'Title', width: "25%", sortable: false, align: 'left' },
                    { display: '图文图片', name: 'Pic', width: "25%", sortable: false, align: 'left' },
                    //{ display: '操作', name: 'List_ID', width: "15%", sortable: false, align: 'center', process: processOp2 },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "d.id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='添加图文信息' class='abtn' href='javascript:;'  onclick=\"util.Add('", value, "')\"><i class='fa fa-edit' ></i>添加</a>");
            ops.push("&nbsp;<a title='编辑图文信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除图文信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        //function processOp2(value, cell) {
        //    var ops = [];
        //    ops.push("&nbsp;<a title='添加图文信息' class='abtn' href='javascript:;'  onclick=\"util.Add('", value, "')\"><i class='fa fa-edit' ></i>添加</a>");
        //    return ops.join("");
        //}

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            $("#ID").val(0);
            $("#List_ID").val(0);
            $("#Name").val("");
            $("#Title").val("");
            $("#Pic").val("");
            $("#Describe").val("");
            $("#Sorting").val(0);
            options.myeditor.setData("");
            $('#frmSave')[0].reset();
            $("#divnr").show();
            $("#divwl").hide();
            $('#EditModal').modal('show');
        });

        $("#isurl").click(function (e) {
            //alert($(this).prop("checked"));
            if ($(this).prop("checked")) {
                $("#divwl").show();
                $("#divnr").hide();
            }
            else {
                $("#divnr").show();
                $("#divwl").hide();
            }
            //$("#MENU_ICON").val($(this).attr("value"));
            //$("#yx").removeClass().addClass("fa fa-" + $(this).attr("value"));
            //$("#xz").html("已选图标");
        });

        util.Edit = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {
                         $("#ID").val(ret.data.ID);
                         $("#List_ID").val(ret.data.List_ID);
                         $("#Name").val(ret.data.Name);
                         $("#Title").val(ret.data.Title);
                         $("#Pic").val(ret.data.Pic);
                         $("#Describe").val(ret.data.Describe);
                         $("#Sorting").val(ret.data.Sorting);
                         $("#URL").val(ret.data.URL);
                        // alert(ret.data.IsURL);
                         if (ret.data.IsURL) {
                             $("#isurl").attr('checked', 'checked');
                             $("#divwl").show();
                             $("#divnr").hide();
                         }
                         else {
                             $("#isurl").attr('checked', false);
                             $("#divnr").show();
                             $("#divwl").hide();
                         }
                         //var myeditor = CKEDITOR.replace('ckeditor', { toolbar: 'Basic' });
                         // alert(myeditor);
                         //alert(options.listUrl);
                         options.myeditor.setData(ret.data.Body);
                        // $("#ckeditor").val(ret.data.Body);
                         //var editor = ckeditor.replace("content");
                         //alert(editor);
                         //editor.setData("输入文字");
                     }
                     else {
                         _showInfoMessage("数据库中没有此商户，请刷新重试！", 'error');
                     }
                 },
                 "json"
           );
            $('#EditModal').modal('show');
        };

        util.Add = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {
                         $("#ID").val(0);
                         $("#List_ID").val(ret.data.List_ID);
                         //alert(ret.data.List_ID);
                         $("#Name").val(ret.data.Name);
                         $("#Title").val("");
                         $("#Pic").val("");
                         $("#Describe").val("");
                         $("#Sorting").val(0);
                         options.myeditor.setData("");
                         $("#isurl").attr('checked', false);
                         $("#divnr").show();
                         $("#divwl").hide();
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

        //$("#btnSave").click(function (e) {
        //    $("#frmSave").submit();
        //});

        //$('#frmSave').validator({
        //    rules: {
        //    },
        //    fields: {

        //    },
        //    valid: function (form) {
        //        FormSubmit(form, function (res) {
        //            if (res.status == 0) {
        //                _showInfoMessage("操作成功！", 'success');
        //                $('#EditModal').modal('hide');
        //                xjgrid.Reload();
        //            }
        //            else {
        //                _showInfoMessage("操作失败：" + res.message, 'error');
        //                //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
        //            }
        //        })
        //    }
        //});

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