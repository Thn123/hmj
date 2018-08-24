; (function (window, undefined, $) {
    $(document).ready(function () {

        var gridopt = {
            url: options.queryRightUrl,
            colModel: [
                    { display: '编号', name: 'RIGHT_ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '名称', name: 'RIGHT_NAME', width: "15%", sortable: false, align: 'left' },
                    { display: '地址', name: 'URL_LINK_TO', width: "45%", sortable: false, align: 'left' },
                    { display: '编码', name: 'MENU_CODE', width: "10%", sortable: false, align: 'left' },
                    { display: '排序', name: 'RIGHT_DSC', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'RIGHT_ID', width: "20%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "RIGHT_ID",
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
            ops.push("&nbsp;<a title='编辑字典信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除字典信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
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
            $("#h4Dict").html(data.text);
            $("#PARENT_ID").val(data.id);
            $("#PARENT_NAME").html(data.text);
            SetRightDict(data.id);
            data.expand();
        }

        $("#btnAdd").click(function (e) {
            $('#frmSave')[0].reset();
            $("#RIGHT_ID").val("0")
            $("#DICT_CODE").removeAttr("readonly");
            if ($("#PARENT_ID").val() != "0") {
                $("#sqlDiv").hide();
            }
            else {
                $("#sqlDiv").show();
            }
            $('#EditModal').modal('show');
            $("#xz").html("请选择图标");
            $("#yx").removeClass();
        });

        util.Edit = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {

                         $("#DICT_CODE").attr("readonly", "readonly");

                         $("#RIGHT_ID").val(ret.data.RIGHT_ID);
                         $("#DICT_CODE").val(ret.data.DICT_CODE);
                         $("#RIGHT_NAME").val(ret.data.RIGHT_NAME);
                         $("#PARENT_NAME").html(ret.data.PARENT_NAME);
                         $("#RIGHT_DSC").val(ret.data.RIGHT_DSC);
                         $("#PARENT_ID").val(ret.data.PARENT_ID);
                         $("#URL_LINK_TO").val(ret.data.URL_LINK_TO);
                         //$("#IS_RIGHT").prop("checked", ret.data.IS_RIGHT=="true"?"checked":"");
                         //$("#IS_MENU").prop("checked", ret.data.IS_MENU == "true" ? "checked" : "");
                         $("#MENU_ICON").val(ret.data.MENU_ICON);
                         //$("#e4").val("<i class='fa fa-" + ret.data.MENU_ICON + "'></i>&nbsp;" + ret.data.MENU_ICON);
                         //$("#e4").find("option[text=" + ret.data.MENU_ICON + "]").attr("selected", true);
                         //$("#e4").val(ret.data.MENU_ICON);
                         $("#e4 option[text='group']").attr("selected", true);


                         $("#e4 option").each(function(){
                             if ($(this).text() === ret.data.MENU_ICON) {
                                 $(this).attr('selected', 'selected');
                                 //alert($(this).text());
                                // format($(this));
                             }
                             
                         });

                         //$("#e5 option").each(function () {
                         //    if ($(this).text() === 'dddd') {
                         //        $(this).attr('selected', 'selected');
                         //        alert($(this).text());
                         //    }

                         //});

                         //document.getElementById("e4").options[1].setAttribute("selected", true);
                         $("#MENU_CODE").val(ret.data.MENU_CODE);
                         if (ret.data.IS_RIGHT) {
                             $("#IS_RIGHT").prop("checked", true);
                         }
                         else {
                             $("#IS_RIGHT").prop("checked", false);
                         }
                         if (ret.data.IS_MENU) {
                             $("#IS_MENU").prop("checked", true);
                         }
                         else {
                             $("#IS_MENU").prop("checked", false);
                         }
                         //if (ret.data.IS_MENU == '1')
                         //    $("#IS_MENU1").attr("checked", true);
                         //if (ret.data.IS_RIGHT == '1')
                         //    $("#IS_RIGHT1").attr("checked", true);
                         if (ret.data.PARENT_ID != 0) {
                             $("#sqlDiv").hide();
                         }
                         else {
                             $("#sqlDiv").show();
                         }

                         $("input[type=radio][value=" + ret.data.MENU_ICON + "]").each(function () {
                             $(this).prop("checked", "checked");
                             $("#yx").removeClass().addClass("fa fa-" + ret.data.MENU_ICON);
                         });

                         //$("option[value=" + ret.data.MENU_ICON + "]").each(function () {
                         //    $(this).prop("selected", "true");
                         //    //$("#yx").removeClass().addClass("fa fa-" + ret.data.MENU_ICON);
                         //});

                         $("#xz").html("已选图标");
                     }
                     else {
                         _showInfoMessage("数据库中没有此权限菜单，请刷新重试！", 'error');
                     }
                 },
                 "json"
           );
            $('#EditModal').modal('show');
        };

        $(".radio").click(function (e) {
           // alert($(this).attr("value"));
            $("#MENU_ICON").val($(this).attr("value"));
            $("#yx").removeClass().addClass("fa fa-" + $(this).attr("value"));
            $("#xz").html("已选图标");
        });


        $(document).ready(function () {
            function format(state) {
                if (!state.id) return state.text; // optgroup
                //alert(state.text);
                return "<i class='fa fa-"+state.text+"'></i>&nbsp;" + state.text;
            }
            $("#e4").select2({
                formatResult: format,
                formatSelection: format,
                escapeMarkup: function (m) { return m; }
            });
           
        });

        $("#e4").change(function () {
            //alert(jQuery("#e4").val());
                $("#MENU_ICON").val(jQuery("#e4").val());
            });
       


        //$(".t").click(function (e) {
        //   $(".t").style.background-color='red';
        //});

        $("#btnSave").click(function (e) {
            //alert($("#IS_MENU1").val());
            //if ($("#IS_MENU1").val() == "true")
            //    $("#IS_MENU").val("1")
            //else
            //    $("#IS_MENU").val("0")

            //if ($("#IS_RIGHT1").val() == "true")
            //    $("#IS_RIGHT").val("1")
            //else
            //    $("#IS_RIGHT").val("0")
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
                //'#DICT_CODE': 'required',
                //'#DICT_VALUE': 'required',
                '#RIGHT_DSC': 'required',
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