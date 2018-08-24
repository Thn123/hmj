; (function (window, undefined, $) {
    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: 'Id', name: 'ID', width: "0%", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '', name: 'IS_STAR', width: "5%", sortable: false, align: 'center', process: formateShowStar },
                    { display: '头像', name: 'IMAGE', width: "8%", sortable: false, align: 'center', process: processImage },
                    { display: '微信名', name: 'Fname', width: "10%", sortable: false, align: 'center' },
                    { display: '姓名', name: 'Name', width: "10%", sortable: false, align: 'center' },
                    { display: '信息', name: 'CONTENT', width: "35%", sortable: false, align: 'left' },
                    { display: '时间', name: 'CREATE_DATE', width: "10%", sortable: false, align: 'center' },
                    { display: '状态', name: 'IS_RETURN', width: "10%", sortable: false, align: 'center', process: formateIsReturn },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp },
            ],
            sortname: "CMH.CREATE_DATE",
            sortorder: "DESC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function formateShowStar(value, cell) {
            if (value.length > 0 && value.toLowerCase() == "false") {
                var ops = [];
                ops.push("<a title=\"收藏\" onclick=\"util.star('" + cell[0] + "')\"><em class=\"shoucang\"></em></a>");
                return ops.join("");
            }
            else if (value.toLowerCase() == "true") {
                var ops = [];
                ops.push("<a title=\"取消收藏\" onclick=\"util.cancelStar('" + cell[0] + "')\"><em class=\"yishoucang\"></em></a>");
                return ops.join("");
            }
        };
        function processImage(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a  href='javascript:;'><img src='", value, "' width='50px'  class='img-circle')\"></a>");
            return ops.join("");
        }
        function formateIsReturn(value, cell) {
            if (value.length > 0 && value.toLowerCase() == "false") {
                return "未回复";
            }
            else if (value.toLowerCase() == "true") {
                return "已回复";
            }
        };
        function processOp(value, cell) {
            var ops = [];
            if (cell[cell.length - 2] == "" || cell[cell.length - 2].toLowerCase() == "false") {
                ops.push("&nbsp;<a title='回复' class='abtn' href='javascript:;' onclick=\"util.reply('", value, "','", cell[3], "')\"><i class='fa fa-edit' ></i>回复</a>");
            }
            else if (cell[cell.length - 2].toLowerCase() == "true") {
                ops.push("&nbsp;<a title='查看回复' class='abtn' href='javascript:;' onclick=\"util.reply('", value, "','", cell[3], "')\"><i class='fa fa-edit' ></i>查看回复</a>");
            }
            return ops.join("");
        };
        util.star = function star(id) {
            $.post(options.setIsStarUrl + "/" + id, { id: id, IS_STAR: true },
                  function (res) {
                      if (res.status > 0) {
                          xjgrid.Reload();
                      }
                      else {
                          showErrorTip("操作失败！：" + res.message, { right: 400, top: 450 }, true, 5000);
                      }
                  },
                  "json"
            );
        }
        util.cancelStar = function cancelStar(id) {
            $.post(options.setIsStarUrl + "/" + id, { id: id, IS_STAR: false },
                  function (res) {
                      if (res.status > 0) {
                          xjgrid.Reload();
                      }
                      else {
                          showErrorTip("操作失败！：" + res.message, { right: 400, top: 450 }, true, 5000);
                      }
                  },
                  "json"
            );
        };

        util.reply = function star(id, wechat) {
            $('#replyModal').modal('show');
            $("#formReply input[type='text'][name='CONTENT']").val("");
            $("#replyH4").text("与" + wechat + "的实时消息");
            $.post(options.queryReplyMessagesUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          $("#replyModal #replyDiv").empty();
                          var messages = res.data;
                          var htmlStr = [];
                          var fanMessage = Enumerable.From(messages).First(function (x) { return x.State == 0 });
                          htmlStr.push("<div id=\"divFanMessage\" class=\"chat-message chat-primary\">");
                          htmlStr.push("<div class=\"chat-contact\"><img src=\"" + fanMessage.IMAGE + "\" alt=\"\"></div>");
                          htmlStr.push("<div class=\"chat-text\"><p>" + fanMessage.CONTENT + "</p></div></div>")
                          var replyMessage = Enumerable.From(messages).Where(function (x) { return x.State == 1 }).ToArray();
                          if (replyMessage.length > 0) {
                              for (var i = 0; i < replyMessage.length; i++) {
                                  htmlStr.push("<div class=\"chat-message me\"><div class=\"chat-contact\"></div>");
                                  htmlStr.push("<div class=\"chat-text\"><p>" + replyMessage[i].CONTENT + "</p></div></div>");
                              }
                          }
                          $("#replyModal #replyDiv").append(htmlStr.join(""));
                          $("#formReply input[type='hidden'][name='ReturnID']").val(id);
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
                CONTENT: "required",
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        $("#replyModal").modal('hide');
                        _showInfoMessage("回复成功", 'success');
                        xjgrid.Reload();
                    }
                    else {
                        showErrorTip("操作失败！：" + res.message, { right: 400, top: 450 }, true, 5000);
                    }
                })
            }
        });
        $("#ultab li").each(function () {
            $(this).click(function () {
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