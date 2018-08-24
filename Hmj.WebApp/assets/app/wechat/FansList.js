; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl + "?state=" + (getQueryString("state") || -1),
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '头像', name: 'IMAGE', width: "8%", sortable: false, align: 'center', process: processOp2 },
                    { display: '昵称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                     { display: '姓名', name: 'XM', width: "10%", sortable: false, align: 'left' },
                      { display: '手机', name: 'Phone', width: "15%", sortable: false, align: 'left' },
                    { display: '性别', name: 'xb', width: "10%", sortable: false, align: 'left' },

                    { display: '省份', name: 'PROVINCE', width: "10%", sortable: false, align: 'left' },
                    { display: '城市', name: 'CITY', width: "10%", sortable: false, align: 'left' },

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
            ops.push("&nbsp;<a  href='javascript:;'  onclick=\"util.See('" + cell[0] + "')\" onmouseout='util.hidephoto()' onmouseover=\"util.showphoto('" + cell[0] + "')\"><img src='", value, "' width='50px'  class='img-circle')\"></a>");
            return ops.join("");
        }
        //function processOp3(value, cell) {
        //    var ops = [];
        //    ops.push("&nbsp;<a  href='javascript:;'  onmouseover=\"javascript:alert('", cell[0], "')\">" + value + "</a>");
        //    return ops.join("");
        //}
        function processOp(value, cell) {
            var ops = [];
            //ops.push("&nbsp;<a title='编辑信息' class='abtn' href='javascript:;'  onclick=\"util.See('", value, "')\"><i class='fa fa-edit' ></i>查看</a>");
            //ops.push("&nbsp;<a title='编辑信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>分组</a>");
            ops.push("&nbsp;&nbsp;<a title='发送信息' class='abtn' href='javascript:;'  onclick=\"util.Message('", value, "')\"><i class='fa fa-edit' ></i>发消息</a>");
            return ops.join("");
        }

        util.showphoto = function (url) {
            //alert(url);

            //-document.body.clientTop
            ev = window.event;
            var mousePos = mouseCoords(ev);
            //alert(mousePos.y);
            var div3 = document.getElementById('div3'); //将要弹出的层    
            div3.style.display = "block"; //div3初始状态是不可见的，设置可为可见       
            div3.style.left = (mousePos.x + 30) + "px"; //鼠标目前在X轴上的位置，加10是为了向右边移动10个px方便看到内容 
            if (event.clientY < 400) {     //鼠标在页面的高度小于200时
                div3.style.top = (mousePos.y) + "px";  //div高度为滚动条的高度
            }
            else if (event.clientY < 600) {
                div3.style.top = (mousePos.y - 200) + "px";
            }
            else {
                div3.style.top = (mousePos.y - 400) + "px";
            }
            div3.style.position = "absolute";
            $("#imgphoto").attr("src", url);
            // alert(div3.style.left);

            $.post(options.editUrl, {
                id: url
            },
                  function (ret) {
                      if (ret && ret.status == 0) {
                          $("#div3").html(ret.data.HTML);
                      }
                      else {
                          _showInfoMessage("请刷新重试！", 'error');
                      }
                  },
                  "json"
            );
        }

        function mouseCoords(ev) {
            if (ev.pageX || ev.pageY) {
                return { x: ev.pageX, y: ev.pageY };
            }
            return {
                x: ev.clientX + document.body.scrollLeft - document.body.clientLeft,
                y: ev.clientY + document.body.scrollTop - document.body.clientTop
            };
        }

        util.hidephoto = function (url) {
            var div3 = document.getElementById('div3'); //将要弹出的层    
            div3.style.display = "none";
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        var num = 0;
        $("#btnAdd").click(function (e) {
            //num = 0;
            _showInfoMessage("开始拉取数据，请稍等...", 'success');
            GetFans("", 0, 0);
            xjgrid.Query(this);
        });

        function GetFans(next_openid, nextcount, ycount) {
            $.post(options.GetFansUrl, {
                openid: next_openid,
                nextcount: nextcount,
                ycount: ycount
            },
                function (ret) {
                    if (ret && ret.status == 0) {
                        if (ret.message == "")
                            _showInfoMessage("拉取粉丝成功", 'success');
                        else {
                            _showInfoMessage("已拉取<span style='color:red;'>" + ret.data.ycount + "/" + ret.data.total + "</span>条数据<br>&nbsp;&nbsp;&nbsp;&nbsp;已完成<span style='color:black;'>" + ((ret.data.ycount / ret.data.total).toFixed(4) * 100).toFixed(2) + "%</span><br>&nbsp;&nbsp;&nbsp;&nbsp;预计剩余<span style='color:black;'>" + ((ret.data.total - ret.data.ycount) / 20 / 60).toFixed(2) + "</span>分钟", 'success');
                            GetFans(ret.message, ret.data.nextcount, ret.data.ycount);
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
            _showInfoMessage("开发中！", 'error');
            return;
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {
                         $("#ID").val(ret.data.ID);
                         $("#replyType").val(ret.data.replyType);
                         $("#KeyWords").val(ret.data.KeyWords);
                         $("#MatchingType").val(ret.data.MatchingType);
                         $("#MsgType").val(ret.data.MsgType);
                         $("#Graphic_ID").val(ret.data.Graphic_ID);
                         $("#Content").val(ret.data.Content);
                         if (ret.data.replyType == 1) {
                             $("#divkeys").show();
                             $("#divtype").show();
                         }
                         else {
                             $("#divkeys").hide();
                             $("#divtype").hide();
                         }

                         if (ret.data.MsgType == "text") {
                             $("#divnews").hide();
                             $("#divtext").show();
                         }
                         else {
                             $("#divnews").show();
                             $("#divtext").hide();
                         }
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
                        alert(ret.message);
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
            location = "MsgRecord.do?id=" + id;
            //$("#hdCurrentId").val(id);
            //$('#EditModal2').modal('show');
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