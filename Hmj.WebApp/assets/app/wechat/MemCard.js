; (function (window, undefined, $) {

    $(document).ready(function () {

        $.post(options.GetMerSeting, {
            //openid: next_openid
        },
                function (ret) {
                    if (ret.status == 0) {
                        $("#SetingID").val(ret.data.ID);
                        $("#IsMemCardReg").val(ret.data.IsMemCardReg);
                        $("#IsShowMemCardPrivilege").val(ret.data.IsShowMemCardPrivilege);
                        $("#IsShowCoupons").val(ret.data.IsShowCoupons);
                        $("#IsShowActivity").val(ret.data.IsShowActivity);
                        $("#IsShowMemCardAmount").val(ret.data.IsShowMemCardAmount);
                        $("#IsShowMemInfo").val(ret.data.IsShowMemInfo);
                    }
                    else {
                        _showInfoMessage(ret.message, 'error');
                    }
                },
                "json"
          );

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '标题', name: 'Title', width: "8%", sortable: false, align: 'center' },
                     { display: '所属门店', name: 'Explain', width: "8%", sortable: false, align: 'center' },
                    { display: '修改时间', name: 'CreateDate', width: "10%", sortable: false, align: 'left' },
                    //{ display: '性别', name: 'xb', width: "10%", sortable: false, align: 'left' },
                    //{ display: '国籍', name: 'COUNTRY', width: "10%", sortable: false, align: 'left' },
                    //{ display: '省份', name: 'PROVINCE', width: "10%", sortable: false, align: 'left' },
                    //{ display: '城市', name: 'CITY', width: "10%", sortable: false, align: 'left' },
                    //{ display: '是否已取消关注', name: 'qx', width: "15%", sortable: false, align: 'left' },
                    //{ display: '微信编号', name: 'FROMUSERNAME', width: "15%", sortable: false, align: 'left' },

                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp2(value, cell) {
            var ops = [];
            ops.push("&nbsp;<img src='", value, "' width='50px'  class='img-circle')\">");
            return ops.join("");
        }
        function processOp(value, cell) {
            var ops = [];
            //ops.push("&nbsp;<a title='编辑信息' class='abtn' href='javascript:;'  onclick=\"util.See('", value, "')\"><i class='fa fa-edit' ></i>查看</a>");
            ops.push("&nbsp;<a title='编辑信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            //ops.push("&nbsp;&nbsp;<a title='发送信息' class='abtn' href='javascript:;'  onclick=\"util.Message('", value, "')\"><i class='fa fa-edit' ></i>发消息</a>");
            return ops.join("");
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        var num = 0;
        $("#btnAdd").click(function (e) {
            $('#frmSave')[0].reset();
            $("#ID").val("0")
            $('#EditModal').modal('show');
        });

        function GetFans(next_openid) {
            $.post(options.GetFansUrl, {
                openid: next_openid
            },
                function (ret) {
                    if (ret && ret.status == 0) {
                        //num = num + 1;
                        //alert(ret.data==17);

                        if (ret.data < 10000 || ret.message == "")
                            _showInfoMessage("拉取粉丝成功。", 'success');
                        else {
                            _showInfoMessage("已成功拉取一万条数据，正在继续拉取剩余数据，请稍等...", 'success');
                            GetFans(ret.message);
                        }
                    }
                    else {
                        _showInfoMessage(ret.message, 'error');
                    }
                },
                "json"
          );
        }

        util.Edit = function (id) {
            //_showInfoMessage("开发中！", 'error');
            //return;
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {
                         $("#ID").val(ret.data.ID);
                         $("#Title").val(ret.data.Title);
                         $("#SID").val(ret.data.SID);
                         $("#Explain").val(ret.data.Explain);
                         //  alert($("#Explain").val());
                         //options.myeditor.setData(ret.data.Explain);
                     }
                     else {
                         _showInfoMessage("数据库中没有此商户，请刷新重试！", 'error');
                     }
                 },
                 "json"
           );
            $('#EditModal').modal('show');
        };

        util.See = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                  function (ret) {
                      if (ret && ret.status == 0) {
                          //alert(ret.data.ID);
                          $("#ID").val(ret.data.ID);
                          $("#NAME").val(ret.data.NAME);
                          $("#xb").val(ret.data.xb);
                          $("#CITY").val(ret.data.CITY);
                          $("#FROMUSERNAME").val(ret.data.FROMUSERNAME);
                          $("#qx").val(ret.data.qx);
                          $("#image1").attr("src", ret.data.IMAGE);
                          // alert(ret.data.NAME);

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
            //alert($("#ckeditor").html);

            // $("#Explain").val(options.myeditor.getData());
            //alert($("#Explain").val());
            $("#frmSave").submit();
        });

        $("#MsgType").change(function (e) {
            if ($(this).val() == "text") {
                $("#divnews").hide();
                $("#divtext").show();
            }
            else {
                $("#divnews").show();
                $("#divtext").hide();
            }
        });

        $("#replyType").change(function (e) {
            if ($(this).val() == "1") {
                $("#divkeys").show();
                $("#divtype").show();
            }
            else {
                $("#divkeys").hide();
                $("#divtype").hide();
            }
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

        $("#btnSave2").click(function (e) {
            //if ($("#IsMemCardReg1").prop("checked") == "true") {

            //}
            $("#IsMemCardReg").val($("#IsMemCardReg1").prop("checked"));
            $("#IsShowMemCardPrivilege").val($("#IsShowMemCardPrivilege1").prop("checked"));
            $("#IsShowCoupons").val($("#IsShowCoupons1").prop("checked"));
            $("#IsShowActivity").val($("#IsShowActivity1").prop("checked"));
            $("#IsShowMemCardAmount").val($("#IsShowMemCardAmount1").prop("checked"));
            $("#IsShowMemInfo").val($("#IsShowMemInfo1").prop("checked"));
            $("#frmSave2").submit();
        });

        $('#frmSave2').validator({
            rules: {
            },
            fields: {

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

        util.Message = function (id) {
            //_showInfoMessage("开发中！", 'error');
            //return;
            $("#hdCurrentId").val(id);
            $('#EditModal2').modal('show');
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