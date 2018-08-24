; (function (window, undefined, $) {
    var submiting = false;
    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "5%", sortable: true, hide: false, align: 'left', iskey: true },
                    { display: '名称', name: 'NAME', width: "28%", sortable: false, align: 'left' },
                    { display: '所属大类', name: 'BCATE', width: "12%", sortable: false, align: 'left' },
                    { display: '所属小类', name: 'SCATE', width: "10%", sortable: false, align: 'left'},
                    { display: '持续时长(分钟)', name: 'TIME_LEN', width: "15%", sortable: false, align: 'left' },
                    { display: '价格', name: 'PRICE', width: "10%", sortable: false, align: 'left' },
                    { display: '状态', name: 'STATUS', width: "7%", sortable: false, align: 'left', process: formatStatus },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            localpage: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑产品信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除产品信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");

            return ops.join("");
        }
        function formatCategory(value, cell) {
            var category = "未知";
            switch (value) {
                case "1":
                    category = "按摩";
                    break;
                case "2":
                    category = "面部护理";
                    break;
                case "3":
                    category = "身体服务";
                    break;
                case "4":
                    category = "水疗法";
                    break;
                case "5":
                    category = "纤体";
                    break;
            }
            return category;
        }
        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0]
            }
            else {
                return "-"
            }
        }
        function formatStatus(value, cell) {
            if (value=="1") {
                return "正常"
            }
            else if (value == "0") {
                return "停止"
            }
        }
        function clear() {
            $("#ID").val("");
            $("#NAME").val("");
            $("#MOBILE").val("");
            $("#REMARK").val("");
            $('input:radio:first').attr('checked', 'checked');
            $('#EditModal').modal('show');
        }
        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            window.location.href = options.editUrl;
        });

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
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
            window.location.href = options.editUrl + "/" + id;

        };
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };

        util.View360 = function (id) {
            //TODO:
            alert("跳转到客户360页面")
        };
    });

    //验证
    $('#frmSave').validator({
        rules: {
        },
        fields: {
            '#NAME': 'required',
            '#MOBILE': 'required',
            '#vEMP_TYPE': 'required',
            '#vSERVICE_LVL': 'required',
            '#vPOSITION': 'required'
            //'#Store_ID': 'required;idcard',

        },
        valid: function (form) {
            //if (submiting) {
            //    return;
            //}
            //submiting = true;
            FormSubmit(form, function (res) {
                if (res.status == 0) {
                    alert("操作成功！");
                    $("#btnClose").click();
                    $('#frmSave')[0].reset()
                    $("#formQuery").submit();
                    //if (window.opener) {
                    //    window.opener.location.reload();
                    //    window.close();
                    //}
                    //else {
                    //    if (parent && parent.xjDailog) {
                    //        parent.xjDailog.Close(false, true, null);
                    //    }
                    //    else {
                    //        window.close();
                    //    }
                    //}
                }
                else {
                    showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                }
                submiting = false;
            })
        }
    });
    $("#btnSave").click(function (e) {
        $("#frmSave").submit();
    });


    $("#TYPE").change(function () {
        PostGetProdTypes();
    })

    var _submiting2 = false;
    function PostGetProdTypes() {
        if (_submiting2) {
            //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
            return;
        }
        _submiting2 = true;
        //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
        var pid = $('#TYPE').val();
        $.ajax({
            url: options.proTypeUrl,
            type: "POST",
            data: { "PID": pid },
            success: function (result) {
               
                hideLoadingMsg();
                //大类
                var cuAllPs = result.data;              
                var cuAllPsArray = new Array();
                if (cuAllPs != "") {
                    cuAllPsArray = cuAllPs.split(",");
                   
                }            
                var hl = "<select class=\"form-control\" id=\"PRO_TYPE\" name=\"PRO_TYPE\" ><option value=''>请选择项目所属小类</option>";

                if (cuAllPs != "") {
                    for (var i = 0; i < cuAllPsArray.length; i++) {
                        var re = cuAllPsArray[i].split("_");                                        
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                    }
                }
                hl += "<select>";
                $("#PRO_TYPE").html(hl);            
                _submiting2 = false;
            }
            ,
            error: function (result) {
                hideLoadingMsg();
                _submiting2 = false;
                _showInfoMessage("页面有异常：" + result, 'error');

            }
        });
    }
   

})(window, undefined, jQuery);