; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '头像', name: 'IMAGE', width: "5%", sortable: false, align: 'left', process: processOp2 },
                    { display: '昵称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '电话', name: 'Phone', width: "15%", sortable: false, align: 'left' },
                    { display: '年龄', name: 'Age', width: "5%", sortable: false, align: 'left' },
                    { display: '类型', name: 'OPType', width: "15%", sortable: false, align: 'left', process: processOp3 },
                    { display: '留言时间', name: 'CreateTime', width: "10%", sortable: false, align: 'left' },
                     { display: '留言内容', name: 'OPContent', width: "25%", sortable: false, align: 'left' }
                    //{ display: '地址', name: 'ADDRESS', width: "10%", sortable: false, align: 'left' },
                    //{ display: '门店电话', name: 'TELEPHONE', width: "10%", sortable: false, align: 'left' },
                    //{ display: '导航', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp2 },
                    //{ display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "o.id",
            sortorder: "DESC",
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

        function processOp3(value, cell) {
            var ops = [];
            if (value == 1) {
                ops.push("门店服务建议");
            }
            else {
                ops.push("微信平台建议");
            }
            return ops.join("");
        }

        function processOp(value, cell) {
            var ops = [];
            // ops.push("&nbsp;<a title='编辑信息' class='abtn' href='javascript:;'  onclick=\"util.Detail('", value, "')\"><i class='fa fa-edit' ></i>编辑明细</a>");
            ops.push("&nbsp;<a title='查看信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>查看</a>");
            // ops.push("&nbsp;&nbsp;<a title='删除信息' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "')\"><i class='fa fa-trash-o' ></i>删除</a>");
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
            $("#ID").val(0);
            //$("#divkeys").show();
            //$("#divtype").show();
            //$("#divnews").show();
            //$("#divtext").hide();
            $('#frmSave')[0].reset();
            $('#EditModal').modal('show');
        });

        util.Edit = function (id) {
            //location = "StoreEdit.do?id=" + id; 
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {

                     if (ret && ret.status == 0) {
                         $("#Nickname").html(ret.data.Nickname);
                         $("#headimgurl").html("<img src='" + ret.data.headimgurl + "' width='50px' />");
                         $("#ZongTi").html(processOp3(ret.data.ZongTi, null));
                         $("#JiShu").html(processOp3(ret.data.JiShu, null));
                         $("#HuangJing").html(processOp3(ret.data.HuangJing, null));
                         $("#FuWu").html(processOp3(ret.data.FuWu, null));
                         $("#OrderID").html(ret.data.OrderID);
                         $("#CreateTime").html(ret.data.CreateTime);
                         $("#LiuYan").html(ret.data.LiuYan);
                     }
                     else {
                         _showInfoMessage("数据库中没有此商户，请刷新重试！", 'error');
                     }
                 },
                 "json"
           );
            $('#EditModal').modal('show');
        };



        util.Detail = function (id) {
            location = "PriceDetail.do?id=" + id;

        };

        function TreeNode_Click(data) {
            data.expand();
        }

        $("#btnSave").click(function (e) {
            $("#frmSave").submit();
        });

        $("#btnClose").click(function (e) {
            // location = "Informations.do";
            $('#EditModal').modal('hide');
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
                        //location = "Informations.do";
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