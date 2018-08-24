; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl+"?id="+getQueryString("id"),
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '微信号', name: 'FromUserName', width: "15%", sortable: false, align: 'left' },
                    { display: '昵称', name: 'nc', width: "15%", sortable: false, align: 'left' },
                     { display: '头像', name: 'tx', width: "15%", sortable: false, align: 'left', process: processOp2 },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "s.id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp2(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a  href='javascript:;'  onclick=\"util.See('" + cell[0] + "')\" onmouseout='util.hidephoto()' onmouseover=\"util.showphoto('" + cell[0] + "')\"><img src='", value, "' width='50px'  class='img-circle')\"></a>");
            return ops.join("");
        }
        function processOp(value, cell) {
            var ops = [];
           ops.push("&nbsp;&nbsp;<a title='删除信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "')\"><i class='fa fa-trash-o' ></i>删除</a>");
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
            //location = "StoreEdit.do?id=0";
            //$("#ID").val(0);
            //$("#divkeys").show();
            //$("#divtype").show();
            //$("#divnews").show();
            //$("#divtext").hide();
            //$('#frmSave')[0].reset();
            //$('#EditModal').modal('show');
            if ($("#yzm").val() == "") {
                _showInfoMessage("请输入验证码！", 'error');
            }
            else {
                $.post(options.SaveSet + "/" + $("#yzm").val(), { yzm: $("#yzm").val(),sid:getQueryString("id") },
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
            }
        });

        $.ajax({
            url: 'LSQrcode.do',

            type: 'POST',

            data: {},

            dataType: 'json',

            timeout: 1000,

            error: function (e) { alert(e.error); },

            success: function (result) {
                $("#yzmtp").attr("src", result.message);
            }

        });

        util.fx = function (id) {
            location = "/WXEmployee/EmployeeList.do";
        }

        util.cx = function (id) {
            location = "/wechat/Ticket.do";
        }

        util.pz = function (id) {
            location = "StoreQrcodeSet.do?id=" + id;
        }

        util.Edit = function (id) {
            location = "StoreEdit.do?id=" + id;
            // $.post(options.editUrl, {
            //     id: id
            // },
            //      function (ret) {
            //          if (ret && ret.status == 0) {
            //              $("#ID").val(ret.data.ID);
            //              $("#replyType").val(ret.data.replyType);
            //              $("#KeyWords").val(ret.data.KeyWords);
            //              $("#MatchingType").val(ret.data.MatchingType);
            //              $("#MsgType").val(ret.data.MsgType);
            //              $("#Graphic_ID").val(ret.data.Graphic_ID);
            //              $("#Content").val(ret.data.Content);
            //              if (ret.data.replyType == 1) {
            //                  $("#divkeys").show();
            //                  $("#divtype").show();
            //              }
            //              else {
            //                  $("#divkeys").hide();
            //                  $("#divtype").hide();
            //              }

            //              if (ret.data.MsgType == "text") {
            //                  $("#divnews").hide();
            //                  $("#divtext").show();
            //              }
            //              else {
            //                  $("#divnews").show();
            //                  $("#divtext").hide();
            //              }
            //          }
            //          else {
            //              _showInfoMessage("数据库中没有此商户，请刷新重试！", 'error');
            //          }
            //      },
            //      "json"
            //);
            // $('#EditModal').modal('show');
        };

        function TreeNode_Click(data) {
            data.expand();
        }

        $("#btnSave").click(function (e) {
            $("#frmSave").submit();
        });

        $("#btnClose").click(function (e) {
            location = "Informations.do";
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
                        location = "Informations.do";
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

        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return r[2]; return null;
        }

    });

})(window, undefined, jQuery);