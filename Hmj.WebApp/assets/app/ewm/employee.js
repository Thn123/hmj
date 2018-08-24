; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '二维码编号', name: 'EwmId', width: "15%", sortable: false, align: 'left' },
                    { display: '姓名', name: 'NAME', width: "15%", sortable: false, align: 'left' },
                    //{ display: '关注人数', name: 'AddCount', width: "10%", sortable: false, align: 'left' },
                    //{ display: '取消人数', name: 'DeleteCount', width: "10%", sortable: false, align: 'left' },
                    { display: '手机', name: 'MOBILE', width: "15%", sortable: true, align: 'left' },
                    //{ display: '地址', name: 'ADDRESS', width: "15%", sortable: false, align: 'left' },
                    //{ display: '门店电话', name: 'TELEPHONE', width: "10%", sortable: false, align: 'left' },
                    //{ display: '导航', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp2 },
                    { display: '操作', name: 'EwmUrl', width: "20%", sortable: false, align: 'center', process: processOp }
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
            ops.push("&nbsp;<a title='发型师' class='abtn' href='javascript:;'  onclick=\"util.fx('", value, "')\"><i class='fa fa-edit' ></i>发型师</a>");
            ops.push("&nbsp;&nbsp;<a title='促销券' class='abtn' href='javascript:;'  onclick=\"util.cx('", value, "')\"><i class='fa fa-trash-o' ></i>促销券</a>");
            ops.push("&nbsp;&nbsp;<a title='配置' class='abtn' href='javascript:;'  onclick=\"util.pz('", value, "')\"><i class='fa fa-trash-o' ></i>预约配置</a>");
            return ops.join("");
        }

        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='二维码下载' class='abtn' href='", value, "' target='_blank'><i></i>二维码下载</a>");
            ops.push("&nbsp;&nbsp;<a title='查看详情' class='abtn' href='javascript:;'  onclick=\"util.Get('", cell[3], "')\"><i></i>查看绑定关系</a>");
            ops.push("&nbsp;<a title='删除' class='abtn' href='javascript:;' onclick=\"util.DeleteEmp('", cell[0], "')\" target='_blank'><i></i>删除</a>");
            return ops.join("");
        }
        function processOp3(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a  href='javascript:;' onmouseout='util.hidephoto()' onmouseover=\"util.showphoto('" + cell[0] + "')\"><img src='", value, "' width='50px'  class='img-circle')\"></a>");
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
            location = "EmpEdit.do";
        });

        $("#btntb").click(function (e) {


            $.post(options.editUrl, {

            },
                 function (ret) {
                     xjgrid.Query(this);
                     return false;
                 },
                 "json"
           );
        });

        util.fx = function (id) {
            location = "/WXEmployee/EmployeeList.do";
        }

        var xjgrid2 = null;
        util.Get = function (g) {
            var gridopt = {
                url: options.listBD + "?Mobile=" + g,
                colModel: [
                        { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                        { display: '关系编号', name: 'EcId', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                        { display: '头像', name: 'IMAGE', width: "8%", sortable: false, align: 'center', process: processOp3 },
                        { display: '昵称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                        { display: '姓名', name: 'XM', width: "10%", sortable: false, align: 'left' },
                        { display: '手机', name: 'Phone', width: "10%", sortable: false, align: 'left' },
                        //{ display: '关注时间', name: 'Create_date', width: "15%", sortable: false, align: 'left' },
                        //{ display: '取消时间', name: 'EndTime', width: "15%", sortable: false, align: 'left' },
                        { display: '性别', name: 'xb', width: "5%", sortable: false, align: 'left' },
                        //{ display: '省份', name: 'PROVINCE', width: "10%", sortable: false, align: 'left' },
                        //{ display: '城市', name: 'CITY', width: "10%", sortable: false, align: 'left' },
                        //{ display: '微信编号', name: 'FROMUSERNAME', width: "15%", sortable: false, align: 'left' },
                        { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp4 }
                ],
                sortname: "id",
                sortorder: "ASC",
                title: false,
                rp: 15,
                usepager: true,
                showcheckbox: false
            };
            xjgrid2 = new xjGrid("gridlistxq", gridopt);
        }
        function processOp4(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='解除绑定' class='abtn' href='javascript:;' onclick=\"util.Delete('", cell[1], "')\" target='_blank'><i></i>解除绑定</a>");
            ops.push("&nbsp;&nbsp;<a title='转移绑定' class='abtn' href='javascript:;'  onclick=\"util.Update('", cell[1], "')\"><i></i>转移绑定</a>");
            return ops.join("");
        }
        util.Delete = function (id) {
            //alert(id);
            $("#hdId").val(id);
            $('#confirmModal').modal('show');
        };
        util.DeleteEmp = function (id) {
            //alert(id);
            $("#hdEmpId").val(id);
            $('#confirmModalEmp').modal('show');
        };
        $("#btnEmpConfirm").click(function (e) {
            $('#confirmModalEmp').modal('hide');
            var id = $("#hdEmpId").val();
            $.post(options.deleteempUrl + "/" + id, { id: id },
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
        util.Update = function (id) {
            //alert(id);
            $("#hdIdByUpdate").val(id);
            $('#confirmModalByUpdate').modal('show');
        };
        $("#btnConfirmByUpdate").click(function (e) {
            $('#confirmModalByUpdate').modal('hide');
            var id = $("#hdIdByUpdate").val();
            var phone = $("#txtPhone").val();
            $.post(options.updateUrl + "/" + id, { id: id, phone: phone },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功！", 'success');
                          xjgrid2.Reload();
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
        })


        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdId").val();
            $.post(options.deleteUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功！", 'success');
                          xjgrid2.Reload();
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
        })
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



    });

})(window, undefined, jQuery);