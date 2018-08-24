; (function (window, undefined, $) {

    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0%", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '用户名', name: 'USER_NO', width: "20%", sortable: false, align: 'left' },
                    { display: '姓名', name: 'Name', width: "20%", sortable: false, align: 'left' },
                    { display: '所属商户', name: 'EMPLOYEE_NAME', width: "20%", sortable: false, align: 'left' },
                    { display: '角色', name: 'ROLE_NAME', width: "20%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp },
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


        $('#frmDlzh').validator({
            rules: {
            },
            fields: {
                //'#USER_NO': 'required',
                //'#USER_PASS': 'required',
                //'#USER_PASS1': 'required',
                //'#ROLE_ID': 'required',
                //'#USER_TYPE': 'required',
                //'#STORE_ID': 'required',
                //'#EMPLOYEE_ID': 'required',
                //'#USER_STORES_OBJ': 'required',
            },
            valid: function (form) {
                //alert(3);
                if (_submiting1) {
                    showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                    return;
                }
                _submiting1 = true;
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
              //  alert(2);
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        if ($("#CUUSER_INFOID").val() == 0) {
                            $("#CUUSER_INFOID").val(res.data);
                        }
                        xjgrid.Query($("#formQuery")[0]);
                        $('#frmDlzh')[0].reset();
                        $("#STORE_ID").html("");
                        $("#EMPLOYEE_ID1").val("");
                        $('#EditModal').modal('hide');
                        _showInfoMessage("保存成功", 'success');
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                    }
                    hideLoadingMsg();
                    _submiting1 = false;
                })
            }
        });

        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑用户信息' class='abtn' href='javascript:;' onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除用户信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
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
            //$("#USER_TYPES").val(cuTDs[0].data.orgLevel);
            $("#STORE_IDS").val(cuTDs[0].value);
            $("#h4Dict").html(cuTDs[0].text);

            //加载修改添加的区域
          //  var ol = cuTDs[0].data.orgLevel == "0" ? "集团用户" : (cuTDs[0].data.orgLevel == "1" ? "区域用户" : "门店用户");

         //   $("#USER_TYPE_STR").html(ol);
            $("#STORE_ID_NAME").html(cuTDs[0].text);
           // $("#USER_TYPE").val(cuTDs[0].data.orgLevel);
            $("#STORE_ID").val(cuTDs[0].value);
        }

        function TreeNode_Click(data) {
            $("#h4Dict").html(data.text);
           // $("#USER_TYPES").val(data.data.orgLevel);
            $("#STORE_IDS").val(data.value);
           // var olr = data.data.orgLevel == "0" ? "集团用户" : (data.data.orgLevel == "1" ? "区域用户" : "门店用户");
           // $("#USER_TYPE_STR").html(olr);
            $("#STORE_ID_NAME").html(data.text);
           // $("#USER_TYPE").val(data.data.orgLevel);
            $("#STORE_ID").val(data.value);
            xjgrid.Query($("#formQuery")[0]);
        }

        xjgrid.Query($("#formQuery")[0]);

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            $("#ID").val(0);
            $('#frmDlzh')[0].reset();
            $("#STORE_ID").html("");
            $("#EMPLOYEE_ID1").val("");
            //$("#USER_STORES_OBJ").val("");
            //$("#USER_STORES_OBJ").select2({ width: "resolve", tags: dts });
            GetEmployeeByType();
            $('#EditModal').modal('show');
        });

        //$("#USER_TYPE").change(SelectUserType);
        //var _submiting2 = false;
        ////根据“用户类别”获取“区域”的下拉框数据
        //function SelectUserType() {          
        //    $.ajax({
        //        url: options.getUserREGION,
        //        type: "POST",
        //        data: { USER_TYPE: $("#USER_TYPE").val(), STORE_ID: $("#STORE_ID1").val() },
        //        success: function (result) {
        //            hideLoadingMsg();
        //            if (result.status == 0) {
        //                $("#STORE_ID").html(result.data);
        //                GetEmployeeByType("1");
        //                $("#STORE_ID").change(GetEmployeeByType); 
        //            }
        //            _submiting2 = false;
        //        },
        //        error: function (result) {
        //            hideLoadingMsg();
        //            _submiting2 = false;
        //            _showInfoMessage("数据查询失败：" + result, 'error');
        //        }
        //    });
        //} 
        var _submiting3 = false;
        //根据所属“分店区域 ”获取“所属员工”的下拉框数据
        function GetEmployeeByType() {
            $.ajax({
                url: options.getEmployeeByType,
                type: "POST",
                data: { USER_TYPE: $("#USER_TYPE").val(), STORE_ID: $("#STORE_ID").val(), EMPLOYEE_ID: $("#EMPLOYEE_ID1").val() },
                success: function (result) {
                    hideLoadingMsg();
                    if (result.status == 0) {
                        $("#EMPLOYEE_ID").html(result.data);
                        //GetStoreData(obj);
                    }
                    _submiting3 = false;
                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting3 = false;
                    _showInfoMessage("数据查询失败：" + result, 'error');
                }
            });
        }
        //var dts = new Array();
        //var _submiting4 = false;

        ////根据所属“分店区域 ”获取“所属员工”的下拉框数据
        //function GetStoreData(obj) {
        //    $.ajax({
        //        url: options.getStoreData,
        //        type: "POST",
        //        data: { USER_TYPE: $("#USER_TYPE").val(), REGION_ID: $("#STORE_ID").val() },
        //        success: function (result) {
        //            hideLoadingMsg();

        //            if (result.status == 0) {
        //                dts = new Array();
        //                for (var i = 0; i < result.data.length; i++) {
        //                    dts.push(result.data[i].NAME);
        //                } 
        //                if (obj != "1") {
        //                    //if ($("#ID").val() == "0") {
        //                        if ($("#USER_TYPE").val() == "2") {
        //                            $("#USER_STORES_OBJ").val(dts);
        //                            $("#USER_STORES_OBJ").select2({ width: "resolve", tags: dts });
        //                        }
        //                        else {
        //                            $("#USER_STORES_OBJ").val("");
        //                            $("#USER_STORES_OBJ").select2({ width: "resolve", tags: dts });
        //                        }
        //                    //}
        //                }

        //            }
        //            _submiting4 = false;
        //        },
        //        error: function (result) {
        //            hideLoadingMsg();
        //            _submiting4 = false;
        //            _showInfoMessage("数据查询失败：" + result, 'error');
        //        }
        //    });
        //}

        util.Edit = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                  function (ret) {
                      if (ret && ret.status == 0) {
                          $("#ID").val(ret.data.ID);
                          $("#USER_NO").val(ret.data.USER_NO);
                          $("#USER_PASS").val(ret.data.USER_PASS);
                          $("#USER_PASS1").val(ret.data.USER_PASS);
                          $("#USER_TYPE").val(ret.data.USER_TYPE);
                          //$("#USER_STORES_OBJ").val(ret.data.USER_STORES_OBJ);
                          //$("#USER_STORES_OBJ").select2({ width: "resolve", tags: dts });
                          $("#EMPLOYEE_ID1").val(ret.data.EMPLOYEE_ID);
                          $("#ROLE_ID").val(ret.data.ROLE_ID);
                          $("#Merchants_ID").val(ret.data.Merchants_ID);
                          $("#STORE_ID").val(ret.data.STORE_ID);
                          $("#Name").val(ret.data.Name);
                          GetEmployeeByType();
                          //SelectUserType();
                      }
                      else {
                          _showInfoMessage("数据库中没有此用户，请刷新重试！", 'error');
                      }
                  },
                  "json"
            );
            $('#EditModal').modal('show');
        };

        var _submiting1 = false;
        $("#btnSaveDlzh").click(function (e) {
           
            var up = $("#USER_PASS").val();
            var up1 = $("#USER_PASS1").val();
            if (up != up1) {
                _showInfoMessage("确认密码不一致，请重新填写！", 'error');
                return;
            }
           // alert(1);
            $("#frmDlzh").submit();
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
                          xjgrid.Query($("#formQuery")[0]);
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