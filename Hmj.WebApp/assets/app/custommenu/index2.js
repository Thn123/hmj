; (function (window, undefined, $) {

    var _submiting = false;

    $(document).ready(function () {
        showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

        pageDefault();

        $("#btnSaveMenu").click(function () {
            $("#frmSaveMenu").submit();
        });

        $("#btnRefresh").click(function () {
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            pageDefault();
            hideLoadingMsg();
            _showInfoMessage("刷新排序成功", 'success');
        });

        $("#btnPublish").click(function () {
            if (confirm("公众平台限制了每天发布的次数，确认如此做吗？")) {
                $.post(options.publishToWechatUrl,
                 null,
                 function (ret) {
                     if (ret.status == 0) {
                         alert("发布成功,创建自定义菜单后,由于微信客户端缓存，需要24小时微信客户端才会展现出来。");
                     }
                     else {
                         alert("发布失败，errcode:" + ret.status + ",errmsg:" + ret.message);
                     }
                 },
                 "json");
            }
        })

        $('#frmSaveMenu').validator({
            rules: {

            },
            fields: {
                //Name: "required;length[~10, true]",
                //OrderNum: "integer[+]",
            },
            valid: function (form) {
                if (_submiting) {
                    showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                    return;
                }
                _submiting = true;
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                if ($.trim(form["Name"].value).length == 0) {
                    showErrorTip("菜单名称不得为空。", { left: 400, top: 450 }, true, 5000);
                    stopAjax();
                    return;
                }

                //if (getByteLen(form["Name"].value) > 10) {
                //    showErrorTip("菜单名称可以最多输入10个字符，中文占2个。", { left: 400, top: 450 }, true, 5000);
                //    stopAjax();
                //    return;
                //}

                if ($("#divContent").css("display") != "none" && $.trim(form["Content"].value).length == 0) {
                    showErrorTip($("#lblContent").text().replace("*", "") + "不得为空。", { left: 400, top: 450 }, true, 5000);
                    stopAjax();
                    return;
                }

                if ($("#divGraphicID").css("display") != "none" && $.trim(form["Graphic_ID"].value).length == 0) {
                    showErrorTip("请选择一个图文资源。", { left: 400, top: 450 }, true, 5000);
                    stopAjax();
                    return;
                }

                if ($("#divUrl").css("display") != "none" && $.trim(form["Url"].value).length == 0) {
                    showErrorTip("链接地址不得为空。", { left: 400, top: 450 }, true, 5000);
                    stopAjax();
                    return;
                }

                if ($("#divUrl").css("display") != "none" && !isURL($.trim(form["Url"].value))) {
                    showErrorTip("请输入正确的链接地址。", { left: 400, top: 450 }, true, 5000);
                    stopAjax();
                    return;
                }

                if ($.trim(form["OrderNum"].value).length != 0 && !(isPositiveInteger($.trim(form["OrderNum"].value)))) {
                    showErrorTip("请输入为正整数的有效排序数。", { left: 400, top: 450 }, true, 5000);
                    stopAjax();
                    return;
                }

                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("保存成功", 'success');
                        updateZTreeNode(res.data.ID, res.data.Name);
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                    }
                    hideLoadingMsg();
                    _submiting = false;
                })
            }
        });

        hideLoadingMsg();
    });

    function stopAjax() {
        _submiting = false;
        hideLoadingMsg();
    }

    ////算长度，全角2个字符
    //function getByteLen(val) {
    //    var len = 0;
    //    for (var i = 0; i < val.length; i++) {
    //        if (val[i].match(/[^\x00-\xff]/ig) != null) //全角
    //            len += 2;
    //        else
    //            len += 1;
    //    }
    //    return len;
    //}

    //判断是否为正整数
    function isPositiveInteger(value) {
        if (!(/^(\+|-)?\d+$/.test(value)) || value < 0) {
            return false;
        }
        return true;
    }

    function isURL(url) {
        var strRegex = "^((https|http|ftp|rtsp|mms)://)?[a-z0-9A-Z]{3}\.[a-z0-9A-Z][a-z0-9A-Z]{0,61}?[a-z0-9A-Z]\.com|net|cn|cc (:s[0-9]{1-4})?/$";
        var re = new RegExp(strRegex);
        if (re.test(url)) {
            return true;
        }
        else {
            return false;
        }
    }

    //默认加载页面
    function pageDefault() {
        getZnodes();
        var treeObj = $.fn.zTree.init($("#treeDemo"), setting, zNodes);
        var nodes = treeObj.getNodes();
        if (nodes.length > 0) {
            treeObj.selectNode(nodes[0]);  //默认选中一级菜单
            showMenuInfo(nodes[0]);
        }
        $("#divType").hide();
        $("#divContent").hide();
        $("#divGraphicID").hide();
        $("#divUrl").hide();
        bindTypeSelect();
    };

    function bindTypeSelect() {
        $("#Type").change(function () {
            if ($(this).val() == "0") {  //文本
                $("#lblContent").html("<span style=\"color: red\">*</span>文本内容");
                $("#divContent").show();
                $("#divGraphicID").hide();
                $("#divUrl").hide();
            }
            if ($(this).val() == "1") {  //图文
                $("#divContent").hide();
                $("#divGraphicID").show();
                $("#divUrl").hide();
            }
            if ($(this).val() == "2") {  //语音
                $("#lblContent").html("<span style=\"color: red\">*</span>语音内容");
                $("#divContent").show();
                $("#divGraphicID").hide();
                $("#divUrl").hide();
            }
            if ($(this).val() == "3") {  //外链
                $("#divContent").hide();
                $("#divGraphicID").hide();
                $("#divUrl").show();
            }
            if ($(this).val() == "4") {  //活动
                $("#lblContent").html("<span style=\"color: red\">*</span>活动内容");
                $("#divContent").show();
                $("#divGraphicID").hide();
                $("#divUrl").hide();
            }
            if ($(this).val() == "5") {  //业务模块
                $("#lblContent").html("<span style=\"color: red\">*</span>业务模块内容");
                $("#divContent").show();
                $("#divGraphicID").hide();
                $("#divUrl").hide();
            }
            if ($(this).val() == "6") {  //应用商店
                $("#lblContent").html("<span style=\"color: red\">*</span>应用商店内容");
                $("#divContent").show();
                $("#divGraphicID").hide();
                $("#divUrl").hide();
            }
            if ($(this).val() == "7") {  //应用商店
                $("#divContent").hide();
                $("#divGraphicID").hide();
                $("#divUrl").hide();
            }
        });
    };

    //同步获取树内容
    function getZnodes() {
        $.ajax({
            type: "post",
            async: false,
            url: options.treeNodesUrl,
            success: function (nodes) {
                zNodes.length = 0;
                for (var i = 0; i < nodes.length; i++) {
                    zNodes.push(nodes[i]);
                }
            }
        });
    };

    //点击菜单
    function beforeClick(treeId, treeNode, clickFlag) {
        return (treeNode.click != false);
    }

    //点击菜单
    function onClick(event, treeId, treeNode, clickFlag) {
        showMenuInfo(treeNode);
    }

    //点击菜单 显示菜单详细
    function showMenuInfo(treeNode) {
        showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
        $.post(options.showMenuInfoUrl, {
            id: treeNode.id
        },
                  function (ret) {
                      if (ret && ret.status == 0) {
                          if (ret.data.Depth == 0) {
                              $("#divType").hide();
                              $("#divKdivContenteys").hide();
                              $("#divGraphicID").hide();
                              $("#divUrl").hide();
                          }
                          else if (ret.data.Depth == 1 || ret.data.Depth == 2) {
                              $("#divType").show();
                              if (ret.data.Type == 0) {  //文本
                                  $("#lblContent").html("<span style=\"color: red\">*</span>文本内容");
                                  $("#divContent").show();
                                  $("#divGraphicID").hide();
                                  $("#divUrl").hide();
                              }
                              if (ret.data.Type == 1) {  //图文
                                  $("#divContent").hide();
                                  $("#divGraphicID").show();
                                  $("#divUrl").hide();
                              }
                              if (ret.data.Type == 2) {  //语音
                                  $("#lblContent").html("<span style=\"color: red\">*</span>语音内容");
                                  $("#divContent").show();
                                  $("#divGraphicID").hide();
                                  $("#divUrl").hide();
                              }
                              if (ret.data.Type == 3) {  //外链
                                  $("#divContent").hide();
                                  $("#divGraphicID").hide();
                                  $("#divUrl").show();
                              }
                              if (ret.data.Type == 4) {  //活动
                                  $("#lblContent").html("<span style=\"color: red\">*</span>活动内容");
                                  $("#divContent").show();
                                  $("#divGraphicID").hide();
                                  $("#divUrl").hide();
                              }
                              if (ret.data.Type == 5) {  //业务模块
                                  $("#lblContent").html("<span style=\"color: red\">*</span>业务模块内容");
                                  $("#divContent").show();
                                  $("#divGraphicID").hide();
                                  $("#divUrl").hide();
                              }
                              if (ret.data.Type == 6) {  //应用商店
                                  $("#lblContent").html("<span style=\"color: red\">*</span>应用商店内容");
                                  $("#divContent").show();
                                  $("#divGraphicID").hide();
                                  $("#divUrl").hide();
                              }
                              if (ret.data.Type == 7) {  //应用商店
                                  $("#divContent").hide();
                                  $("#divGraphicID").hide();
                                  $("#divUrl").hide();
                              }
                          }
                          $("#ID").val(ret.data.ID);
                          $("#ParentID").val(ret.data.ParentID);
                          $("#Depth").val(ret.data.Depth);
                          $("#Name").val(ret.data.Name);
                          $("#Type").val(ret.data.Type);
                          $("#Content").val(ret.data.Content);
                          $("#Graphic_ID").val(ret.data.Graphic_ID);
                          $("#Url").val(ret.data.Url);
                          $("#OrderNum").val(ret.data.OrderNum);
                      }
                      else {
                          _showInfoMessage(res.data.message, 'error');
                      }
                      hideLoadingMsg();
                  },
                  "json"
            );
    };

    //点击保存后更新左边DOM树
    function updateZTreeNode(id, name) {
        var zTree = $.fn.zTree.getZTreeObj("treeDemo");
        var node = zTree.getNodeByParam("id", id);
        var nodeList = [];
        if (node === null) {
            nodeList = [];
        } else {
            nodeList = [node];
        }
        for (var i = 0, l = nodeList.length; i < l; i++) {
            nodeList[i].name = name;
            zTree.updateNode(nodeList[i]);
        }
    }

    //添加按钮
    function addHoverDom(treeId, treeNode) {
        if (treeNode.level < 2) {

            if ((treeNode.level == 0 && treeNode.children != null && treeNode.children.length == 3) ||
                (treeNode.level == 1 && treeNode.children != null && treeNode.children.length == 5)) {

            }
            else {
                var sObj = $("#" + treeNode.tId + "_span");
                if (treeNode.editNameFlag || $("#addBtn_" + treeNode.tId).length > 0) return;
                var addStr = "<span class='button add' id='addBtn_" + treeNode.tId
                    + "' title='add node' onfocus='this.blur();'></span>";
                sObj.after(addStr);
                var btn = $("#addBtn_" + treeNode.tId);
                if (btn) btn.bind("click", function () {
                    if (treeNode.level == "0" && treeNode.children != null && treeNode.children.length >= 3) {
                        showErrorTip("一级菜单最多添加三个。", { left: 400, top: 450 }, true, 5000);
                    }
                    else if (treeNode.level == "1" && treeNode.children != null && treeNode.children.length >= 5) {
                        showErrorTip("二级菜单最多添加五个", { left: 400, top: 450 }, true, 5000);
                    }
                    else {
                        addChild(treeNode);
                    }
                });
            }
        }
        return false;
    };

    //添加子菜单事件
    var newCount = 1;
    function addChild(treeNode) {
        showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
        var name = "";
        if (treeNode.level == 0) {
            name = "一级菜单";
        }
        else if (treeNode.level == 1) {
            name = "二级菜单";
        }
        $.post(options.addChildUrl, { Name: name + (newCount++), ParentID: treeNode.id },
                 function (childNode) {
                     var zTree = $.fn.zTree.getZTreeObj("treeDemo");
                     zTree.addNodes(treeNode, { id: childNode.id, pId: childNode.pId, name: childNode.name });
                     hideLoadingMsg();
                 },
                 "json"
           );
    };

    //是否显示移除按钮
    function showRemoveBtn(treeId, treeNode) {
        if (treeNode.level == 0) {
            return false
        }
        return true;
    }

    //移除之前的事件
    var deleteFlag = false;
    function beforeRemove(treeId, treeNode) {
        var zTree = $.fn.zTree.getZTreeObj("treeDemo");
        zTree.selectNode(treeNode);
        if (treeNode.children != null && treeNode.children.length > 0) {
            showErrorTip("该菜单有" + treeNode.children.length + "个子菜单，请先删除所有子菜单后重试。", { left: 400, top: 450 }, true, 5000);
            return false;
        }
        else if (treeNode.level == 0) {
            showErrorTip("自定义菜单不能删除。", { left: 400, top: 450 }, true, 5000);
            return false;
        }
        //return confirm("确认删除 节点 -- " + treeNode.name + " 吗？");
        if (confirm("确认删除 菜单 -- " + treeNode.name + " 吗？")) {
            deleteFlag = false;
            removeCustomMenu(treeNode);
            if (deleteFlag) {
                _showInfoMessage("删除菜单成功", 'success');
                zTree.selectNode(treeNode.getParentNode());
                showMenuInfo(treeNode.getParentNode());
                return deleteFlag;
            }
            else {
                _showInfoMessage("操作失败：" + res.message, 'error');
            }
        }
        zTree.selectNode(treeNode);
        showMenuInfo(treeNode);
        return false;
    }

    //移除事件
    function onRemove(e, treeId, treeNode) {
    }

    function removeCustomMenu(treeNode) {
        showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
        $.ajax({
            type: "post",
            async: false,
            url: options.deleteMenuUrl,
            data: { id: treeNode.id },
            success: function (ret) {
                deleteFlag = true;
                hideLoadingMsg();
            }
        });
    }

    function removeHoverDom(treeId, treeNode) {
        $("#addBtn_" + treeNode.tId).unbind().remove();
    };

    //是否显示重命名按钮
    function showRenameBtn(treeId, treeNode) {
        //if (treeNode.level == 0) {
        //    return false;
        //}
        //return true;
        return false;
    }

    //重命名之前的事件
    function beforeRename(treeId, treeNode, newName, isCancel) {
        if (newName.length == 0) {
            alert("菜单名称不能为空.");
            var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            setTimeout(function () { zTree.editName(treeNode) }, 10);
            return false;
        }
        return true;
    }

    //重命名事件
    function onRename(e, treeId, treeNode, isCancel) {

    }

    function beforeDrag(treeId, treeNodes) {
        return false;
    }

    function beforeEditName(treeId, treeNode) {
        var zTree = $.fn.zTree.getZTreeObj("treeDemo");
        zTree.selectNode(treeNode);
        return confirm("进入节点 -- " + treeNode.name + " 的编辑状态吗？");
    }

    var setting = {
        view: {
            addHoverDom: addHoverDom,
            removeHoverDom: removeHoverDom,
            selectedMulti: false
        },
        edit: {
            enable: true,
            editNameSelectAll: true,
            showRemoveBtn: showRemoveBtn,
            showRenameBtn: showRenameBtn
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        callback: {
            beforeDrag: beforeDrag,
            beforeEditName: beforeEditName,
            beforeRemove: beforeRemove,
            beforeRename: beforeRename,
            onRemove: onRemove,
            onRename: onRename,
            beforeClick: beforeClick,
            onClick: onClick
        }
    };

    var zNodes = [
         { id: 1, pId: 0, name: "父节点 1", open: true },
         { id: 11, pId: 1, name: "叶子节点 1-1" },
         { id: 12, pId: 1, name: "叶子节点 1-2" },
         { id: 13, pId: 1, name: "叶子节点 1-3" },
         { id: 2, pId: 0, name: "父节点 2", open: true },
         { id: 21, pId: 2, name: "叶子节点 2-1" },
         { id: 22, pId: 2, name: "叶子节点 2-2" },
         { id: 23, pId: 2, name: "叶子节点 2-3" },
         { id: 3, pId: 0, name: "父节点 3", open: true },
         { id: 31, pId: 3, name: "叶子节点 3-1" },
         { id: 32, pId: 3, name: "叶子节点 3-2" },
         { id: 33, pId: 3, name: "叶子节点 3-3" }
    ];

})(window, undefined, jQuery);