; (function (window, undefined, $) {
    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: 'Id', name: 'ID', width: "0%", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '', name: 'IS_STAR', width: "5%", sortable: false, align: 'center', process: formateShowStar },
                    { display: '微信名', name: 'NICKNAME', width: "10%", sortable: false, align: 'center' },
                    { display: '姓名', name: 'Name', width: "10%", sortable: false, align: 'center' },
                    { display: '信息', name: 'CONTENT', width: "35%", sortable: false, align: 'left' },
                    { display: '时间', name: 'CREATE_DATE', width: "10%", sortable: false, align: 'center' },
                    { display: '状态', name: 'IS_RETURN', width: "10%", sortable: false, align: 'center', process: formateIsReturn },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp },
                    { display: 'IS_STAR', name: 'IS_STAR', width: "0%", sortable: false, hide: true, align: 'center' },
            ],
            sortname: "cmr.CREATE_DATE",
            sortorder: "DESC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };

        var xjgrid = new xjGrid("gridlist", gridopt);

        function formateShowStar(value, cell) {
            if (value.toLowerCase() == "false") {
                return "";
            }
            else if (value.toLowerCase() == "true") {
                return "星星";
            }
        };

        function formateIsReturn(value, cell) {
            if (value == 0) {
                return "未回复";
            }
            else if (value = 1) {
                return "已回复";
            }
        };

        function processOp(value, cell) {
            var ops = [];
            if (cell[cell.length - 1].toLowerCase() == "false" || cell[cell.length - 1] == "") {
                ops.push("&nbsp;<a title='加星' class='abtn' href='javascript:;' onclick=\"util.star('", value, "','", cell[2], "')\"><i class='fa fa-edit' ></i>加星</a>");
            }
            else if (cell[cell.length - 1].toLowerCase() == "true") {
                ops.push("&nbsp;<a title='取消加星' class='abtn' href='javascript:;' onclick=\"util.cancelStar('", value, "','", cell[2], "')\"><i class='fa fa-edit' ></i>取消加星</a>");
            }

            if (cell[6] == "" || cell[6].toLowerCase() == "false") {
                ops.push("&nbsp;<a title='回复' class='abtn' href='javascript:;' onclick=\"util.reply('", value, "','", cell[2], "')\"><i class='fa fa-edit' ></i>回复</a>");
            }
            else if (cell[6].toLowerCase() == "true") {
                ops.push("&nbsp;<a title='查看回复' class='abtn' href='javascript:;' onclick=\"util.reply('", value, "','", cell[2], "')\"><i class='fa fa-edit' ></i>查看回复</a>");
            }
            return ops.join("");
        };

        util.star = function star(id, wechat) {
            $("#lbuserName").html("加星[" + wechat + "]的这条实时消息？");
            $("#hdCurrentId").val(id);
            $("#btnStarConfirm").show();
            $("#btnCancelStarConfirm").hide();
            $('#confirmModal').modal('show');
        }

        util.reply = function star(id, wechat) {
            //默认
            $("#formReply input[type='text']").val("");
            $('#replyModal').modal('show');
            $("#divChatMessage img").attr("src", "");
            $("#divChatMessage p:eq(0)").text("");
            $("#divChatRely").hide();
            $("#formReply").show();

            $.post(options.getMessageUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          $("#formReply input[type='hidden'][name='ID']").val(res.data.ID);
                          $("#formReply input[type='hidden'][name='FROMUSERNAME']").val(res.data.FROMUSERNAME);
                          $("#divChatMessage img").attr("src", res.data.IMAGE);
                          $("#divChatMessage p:eq(0)").text(res.data.CONTENT);
                          if (res.data.IS_RETURN && res.data.IS_RETURN == true) {
                              $("#divChatRely").show();
                              $("#divChatRely p:eq(0)").text(res.data.Return_Con);
                              $("#formReply").hide();
                          }
                          else {

                          };
                      }
                  },
                  "json"
            );
        }

        $("#btnReply").click(function () {
            $("#formReply").submit();
        });

        $('#formReply').validator({
            rules: {

            },
            fields: {
                Return_Con: "required",
                //OrderNum: "integer[+]",
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        $("#replyModal").modal('hide');
                        //showLoadingMsg("操作成功", { right: 100, top: 10 }, true, 5000);
                        _showInfoMessage("回复成功", 'success');
                        xjgrid.Reload();
                    }
                    else {
                        //_showInfoMessage("操作失败：" + res.message, 'error');
                        showErrorTip("操作失败！：" + res.message, { right: 400, top: 450 }, true, 5000);
                    }
                })
            }
        });

        $("#btnStarConfirm").click(function () {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.setIsStarUrl + "/" + id, { id: id, IS_STAR: true },
                  function (res) {
                      if (res.status > 0) {
                          //showLoadingMsg("操作成功", { right: 100, top: 10 }, true, 5000);
                          //_showInfoMessage("操作成功", 'success');
                          xjgrid.Reload();
                      }
                      else {
                          showErrorTip("操作失败！：" + res.message, { right: 400, top: 450 }, true, 5000);
                      }
                  },
                  "json"
            );
        });

        util.cancelStar = function cancelStar(id, wechat) {
            $("#lbuserName").html("取消加星[" + wechat + "]的这条实时消息？");
            $("#hdCurrentId").val(id);
            $("#btnStarConfirm").hide();
            $("#btnCancelStarConfirm").show();
            $('#confirmModal').modal('show');
        };

        $("#btnCancelStarConfirm").click(function () {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.setIsStarUrl + "/" + id, { id: id, IS_STAR: false },
                  function (res) {
                      if (res.status > 0) {
                          //showLoadingMsg("操作成功", { right: 100, top: 10 }, true, 5000);
                          xjgrid.Reload();
                      }
                      else {
                          showErrorTip("操作失败！：" + res.message, { right: 400, top: 450 }, true, 5000);
                      }
                  },
                  "json"
            );
        });

        $("#ultab li").each(function () {
            $(this).click(function () {
                //alert());
                $("#SEARCHTYPE").val($(this).index());
                xjgrid.Query($("#formQuery"));
            });
        });

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });
    });
})(window, undefined, jQuery);