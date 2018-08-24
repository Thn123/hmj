; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '名称', name: 'Name', width: "15%", sortable: false, align: 'left' },
                    { display: '适用月龄', name: 'Age', width: "15%", sortable: false, align: 'left', process: processOp3},
                    { display: '类型', name: 'PType', width: "15%", sortable: false, align: 'left' },
                    { display: '价格', name: 'Price', width: "15%", sortable: false, align: 'left' },
                    { display: '状态', name: 'State', width: "10%", sortable: false, align: 'left', process: processOp2 },
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
            ops.push("&nbsp;<a title='编辑信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            //ops.push("&nbsp;&nbsp;<a title='删除信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        function processOp3(value, cell) {
            var ops = [];
            if (value == 0) {
                ops.push("&nbsp;0-6个月");
            } else if (value == 1) {
                ops.push("&nbsp;6-12个月");
            }
            else if (value == 2) {
                ops.push("&nbsp;12-18个月");
            }
            else if (value == 3) {
                ops.push("&nbsp;18个月以上");
            } else {
                ops.push("&nbsp;&nbsp;");
            }
            return ops.join("");
        }

        function processOp2(value, cell) {
            var ops = [];
            if (value == 1) {
                ops.push("&nbsp;上架");
            } else {
                ops.push("&nbsp;&nbsp;下架");
            }
            return ops.join("");
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#qb").click(function (e) {
            $("#replyType").val(0);
            $("#formQuery2").submit();
        });

        $("#gjz").click(function (e) {
            $("#replyType").val(1);
            $("#formQuery2").submit();
        });

        $("#bgz").click(function (e) {
            $("#replyType").val(2);
            $("#formQuery2").submit();
        });

        $("#zdhf").click(function (e) {
            $("#replyType").val(3);
            $("#formQuery2").submit();
        });


        $("#formQuery2").submit(function () {
            xjgrid.Query(this);
            return false;
        });
        $("#btnAdd").click(function (e) {
            location = "GoodEdit.do?id=0";

        });

        util.Edit = function (id) {
            location = "GoodEdit.do?id=" + id;
        };

        function TreeNode_Click(data) {
            data.expand();
        }

        $("#btnSave").click(function (e) {
            // $("#Describe").val(options.myeditor.getData());
            //alert(1);
            $("#frmSave").submit();
        });

        $("#btnClose").click(function (e) {
            location = "Ticket.do";
        });

        $("#Type").change(function (e) {
            if ($(this).val() == "抵扣券") {
                $("#divDiscount").show();
                $("#divoldprice").hide();
                $("#divnewprice").hide();
            }
            else if ($(this).val() == "现金券") {
                $("#divDiscount").hide();
                $("#divoldprice").show();
                $("#divnewprice").show();
            }
            else {
                $("#divDiscount").hide();
                $("#divoldprice").hide();
                $("#divnewprice").hide();
            }
        });

        if ($("#Type").val() == "抵扣券") {
            $("#divDiscount").show();
            $("#divoldprice").hide();
            $("#divnewprice").hide();
        }
        else if ($("#Type").val() == "现金券") {
            $("#divDiscount").hide();
            $("#divoldprice").show();
            $("#divnewprice").show();
        }
        else {
            $("#divDiscount").hide();
            $("#divoldprice").hide();
            $("#divnewprice").hide();
        }

        if ($("#IsCouponsReg").val() == "True") {
            $("#IsCouponsReg1").prop("checked", true);
        }

        $("#btnSave2").click(function (e) {
            $("#IsCouponsReg").val($("#IsCouponsReg1").prop("checked"));
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
                        //xjgrid.Reload();
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                })
            }
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
                        //_showInfoMessage("操作成功！", 'success');
                        //$('#EditModal').modal('hide');
                        //xjgrid.Reload();
                        location = "Ticket.do";
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                })
            }
        });

        util.Delete = function (id) {
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