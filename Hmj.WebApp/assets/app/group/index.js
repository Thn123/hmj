; (function (window, undefined, $) {
    var _ACTION = '';
    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: 'ID', name: 'ID', width: "14%", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '头 像', name: 'AVATAR_URL', width: "10%", sortable: true, align: 'left', process: processAvatar },
                    { display: '姓 名', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    //{ display: '性 别', name: 'GENDER', width: "9%", sortable: true, align: 'left', process: processGender },
                    { display: '账 号', name: 'USERID', width: "10%", sortable: false, align: 'left' },
                    { display: '手机号', name: 'MOBILE', width: "10%", sortable: false, align: 'left' },
                    { display: '邮 箱', name: 'EMAIL', width: "15%", sortable: false, align: 'left' },
                    //{ display: '微信号', name: 'WECHAT_ID', width: "8%", sortable: false, align: 'left' },
                    //{ display: '状态', name: 'STATUS', width: "10%", sortable: false, align: 'left' },
                    { display: '操 作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "ID",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
            //localpage: true,
            autoload: false,
            onsuccess: function (data) {
                //console.log(data);
                if (data.page == 1) {
                    $('#empQty').text(data.total);
                }
            }
        };
        var xjgrid = new xjGrid("gridlist", gridopt);


        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        function processOp(value, cell) {
            var ops = [];
            //ops.push('<button type="button" class="btn btn-primary dropdown-toggle btn-xs" data-toggle="dropdown"> 操 作 <span class="caret"></span></button>');
            //ops.push('<ul class="dropdown-menu" role="menu">');
            //ops.push('<li><a href="javascript:;" class="abtn"  onclick="util.Edit('+ value+')"><i class="fa fa-edit"></i>修改</a></li>');
            //ops.push('<li><a href="@Url.Action("Edit", "Voucher")">新增券</a></li>');
            //ops.push('</ul>');

            ops.push("&nbsp;<a title='修改员工信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit'></i>修改</a>");
            ops.push("&nbsp;<a title='删除员工信息' class='abtn' href='javascript:;'  onclick=\"util.Delete ('", value, "')\"><i class='fa fa-trash-o'></i>删除</a>");
            ops.push("&nbsp;<a title='查看二维码' class='abtn' href='javascript:;'  onclick=\"util.viewEWM('", value, "')\"><i class='fa fa-qrcode'></i>二维码</a>");
            ops.push("&nbsp;<a title='查看绑定' class='abtn' href='javascript:;'  onclick=\"util.viewBind('", cell[3], "')\"><i class='fa fa-user'></i>查看绑定</a>");
            ops.push("&nbsp;<a title='修改组织架构' class='abtn' href='javascript:;'  onclick=\"util.modifyStore('", value, "')\"><i class='fa fa-exchange'></i>修改组织架构</a>");
            return ops.join("");
        }
        function processAvatar(value) {

            return '<img src="' + value + '" class="emp-pic" />';
        }
        function processGender(value) {

            if (value == 1) {
                return '男';
            } else if (value == 2) {
                return '女';
            }
            return '';
        }

        $("#btnAddEmp").click(function (e) {

            var inst = myTree.jstree(true);
            var sel = inst.get_selected(true);
            if (sel.length == 0) {
                _showInfoMessage("请选择一个组织部门。", 'info');
                return;
            } else if (sel[0].data.type == '0') {
                _showInfoMessage("不能选择根目录。", 'info');
                return;
            }
            //console.log(sel);
            util.Edit(0);
        });
        $("#btnExportEmp").click(function (e) {

            var inst = myTree.jstree(true);
            var sel = inst.get_selected(true);
            if (sel.length == 0) {
                _showInfoMessage("请选择一个组织部门。", 'info');
                return;
            }
            var deptId = sel[0].id;
            var deptName = sel[0].text;

            window.location.href = options.exportEmpUrl + '/' + deptId + '?deptName=' + deptName;
        });
        $("#btnSaveEmp").click(function (e) {
            $("#formEmployee").submit();
            return false;
        });
        $('#formEmployee').validator({
            rules: {
            },
            fields: {
                '#NAME': 'required',
                '#USERID': 'required',
                '#WECHAT_ID': 'required',
                '#MOBILE': 'required;mobile',
                '#EMAIL': 'email'
            },
            valid: function (form) {
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                //var inst = myTree.jstree(true);
                //var sel = inst.get_selected(true);
                //var selectedOne = sel[0];

                FormSubmit(form, function (response) {
                    if (response.status > 0) {

                        xjgrid.Reload();
                        _showInfoMessage("保存成功", 'success');

                        $('#editEmpModal').modal('hide');
                    } else {
                        _showInfoMessage("操作失败：" + response.message, 'error');
                    }
                    hideLoadingMsg();
                });
            }
        });

        var myTree = $('#ptree');
        myTree.jstree({
            "core": {
                'multiple': false,
                "animation": 0,
                "check_callback": true,
                "themes": { "stripes": true },
                'data': {
                    'url': function (node) {
                        //return node.id === '#' ? 'ajax_demo_roots.json' : 'ajax_demo_children.json';
                        return options.queryGroupTree;
                    },
                    'data': function (node) {
                        return { 'pid': node.id == '#' ? 0 : node.id, storeId: -1 }
                    }
                }
            },
            "types": {
                "#": {
                    "max_children": 1,
                    //"max_depth": 4,
                    "valid_children": ["root"]
                },
                "root": {
                    "icon": "../../assets/plugins/jquery-jstree/images/tree_icon.png",
                    "valid_children": ["default"]
                },
                "default": {
                    "valid_children": ["default", "file"]
                },
                "file": {
                    "icon": "fa fa-file",
                    "valid_children": []
                }
            },
            "plugins": ["contextmenu", "state", "types", "wholerow"],
            "contextmenu": {
                "items": {
                    "create": null,
                    "rename": null,
                    "remove": null,
                    "ccp": null,
                    "新建": {
                        "label": "新建",
                        'icon': "fa fa-plus",
                        'visible': function (NODE, TREE_OBJ) {
                            if (NODE.length != 1) return 0;
                            return TREE_OBJ.check("creatable", NODE);
                        },
                        "action": function (data) {
                            _ACTION = 'create';

                            var inst = $.jstree.reference(data.reference);
                            var obj = inst.get_node(data.reference);
                            if (obj.type == 'file') {
                                _showInfoMessage("门店不能添加子部门", 'info');

                            } else {
                                var _modal = $('#editGroupModal');
                                if (obj.data.type == 2) { //区域，创建门店
                                    _modal = $('#editStoreModal');
                                }
                                _modal.find('#ID').val('');
                                _modal.find('#ParentID').val(obj.id);
                                showDialog("新建“" + obj.text + "”的子部门", _modal, obj);
                            }
                        }
                    },
                    "修改": {
                        "label": "修改",
                        'icon': "fa fa-edit",
                        'visible': function (NODE, TREE_OBJ) {
                            if (NODE.length != 1) return 0;
                            return TREE_OBJ.check("creatable", NODE);
                        },
                        "action": function (data) {
                            _ACTION = 'modify';

                            var inst = $.jstree.reference(data.reference),
                                obj = inst.get_node(data.reference);
                            if (obj.type == 'root') {

                                _showInfoMessage("顶级目录不能修改。", 'info');
                                return;
                            }

                            var _modal = $('#editGroupModal');
                            if (obj.type == 'file') { //门店
                                _modal = $('#editStoreModal');
                            }

                            _modal.find('#ID').val(obj.id);
                            _modal.find('#ParentID').val('');
                            showDialog("修改“" + obj.text + "”", _modal, obj);

                        }
                    },
                    "调整部门": {
                        "label": "调整部门",
                        'icon': "fa fa-edit",
                        'visible': function (NODE, TREE_OBJ) {
                            if (NODE.length != 1) return 0;
                            return TREE_OBJ.check("creatable", NODE);
                        },
                        "action": function (data) {
                            _ACTION = 'updateorg';

                            var inst = $.jstree.reference(data.reference),
                                obj = inst.get_node(data.reference);
                            if (obj.type == 'root') {

                                _showInfoMessage("顶级目录不能修改。", 'info');
                                return;
                            }
                            var type = obj.data.type;
                            if (type > 1) {
                                var _modal = $('#modifyStoreOrg');

                                util.modifyStoreOrg(obj.id);
                            } else {
                                _showInfoMessage("该组织不能修改。", 'error');
                            }
                            //_modal.find('#ID').val(obj.id);
                            //_modal.find('#ParentID').val('');
                            //showDialog("修改“" + obj.text + "”", _modal, obj);

                        }
                    },
                    "删除": {
                        "label": "删除",
                        'icon': "fa fa-trash-o",
                        'visible': function (NODE, TREE_OBJ) {
                            if (NODE.length != 1) return 0;
                            return TREE_OBJ.check("creatable", NODE);
                        },
                        "action": function (data) {
                            _ACTION = 'delete';

                            var inst = $.jstree.reference(data.reference),
                                obj = inst.get_node(data.reference);
                            if (obj.type == 'root') {

                                _showInfoMessage("顶级目录不能删除。", 'info');
                                return;
                            }

                            if (confirm("确定要删除此项？删除后不可恢复。")) {
                                $.post(options.deleteDeptUrl, { id: obj.id, type: obj.data.type }, function (response) {
                                    if (response.status > 0) {
                                        inst.delete_node(obj);
                                    } else {
                                        _showInfoMessage(response.message, 'info');
                                    }
                                });
                            }
                        }
                    }
                }
            }
        })
        .on("select_node.jstree", function (e, data) {

            $("#SelectGroupId").val(data.node.id);
            $("#EmpGroupId").val(data.node.id);
            $("#EmpGroupType").val(data.node.data.type);
            $('#h4Dict').text(data.node.text);

            xjgrid.Query($("#formQuery")[0]);
        });

        //右键创建事件
        //.on("create_node.jstree", function (e, data) {

        //})
        //.on("rename_node.jstree", function (e, data) {

        //})
        //.on("delete_node.jstree", function (e, data) {

        //});

        //保存大区/区域
        $('#btnSaveGroup').click(function () {

            $('#formGroup').submit();
            return false;

        });
        var _submiting1 = false;
        $('#formGroup').validator({
            rules: {
            },
            fields: {
                '#DeptName': 'required',
                '#DeptCode': 'required',
                '#Order': 'required;integer[+]'
            },
            valid: function (form) {
                if (_submiting1) {
                    showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                    return;
                }
                _submiting1 = true;
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                var inst = myTree.jstree(true);
                var sel = inst.get_selected(true);
                var selectedOne = sel[0];

                FormSubmit(form, function (response) {
                    if (response.status > 0) {
                        if (_ACTION == 'create') {
                            var newNode = {
                                id: response.data.ID,
                                text: response.data.DeptName,
                                data: { type: response.data.Type }
                            };

                            if (response.data.Type == 3) {   //如果是门店
                                newNode.type = 'file';
                            }
                            inst.create_node(selectedOne, newNode, 'last');
                        } else if (_ACTION == 'modify') {
                            inst.rename_node(selectedOne, response.data.DeptName);

                        }
                        _showInfoMessage("保存成功", 'success');

                        $('#editGroupModal').modal('hide');
                    } else {
                        _showInfoMessage("操作失败：" + response.message, 'error');
                    }
                    hideLoadingMsg();
                    _submiting1 = false;
                });
            }
        });


        //保存门店
        $('#btnSaveStore').click(function () {

            $('#formStore').submit();
            return false;

        });
        var _submiting2 = false;
        $('#formStore').validator({
            rules: {
            },
            fields: {
                '#DeptName': 'required',
                '#DeptCode': 'required',
                '#StoreType': 'required',
                '#Order': 'required;integer[+]'
            },
            valid: function (form) {
                if (_submiting2) {
                    showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                    return;
                }
                _submiting2 = true;
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                var inst = myTree.jstree(true);
                var sel = inst.get_selected(true);
                var selectedOne = sel[0];


                FormSubmit(form, function (response) {
                    if (response.status > 0) {
                        if (_ACTION == 'create') {
                            var newNode = {
                                id: response.data.ID,
                                text: response.data.DeptName,
                                type: 'file',
                                data: { type: response.data.Type }
                            };

                            inst.create_node(selectedOne, newNode, 'last');
                        } else if (_ACTION == 'modify') {
                            inst.rename_node(selectedOne, response.data.DeptName);

                        }
                        _showInfoMessage("保存成功", 'success');

                        $('#editStoreModal').modal('hide');
                    } else {
                        _showInfoMessage("操作失败：" + response.message, 'error');
                    }
                    hideLoadingMsg();
                    _submiting2 = false;
                });
            }
        });

        function showDialog(title, _modal, node) {
            var deptName = '',
                deptCode = '',
                storeType = '',
                isPickUp = false,
                province = '',
                city = '',
                area = '',
                addr = '',
                phone = '',
                lat = '',
                lng = '',
                brand = '',
                order = '',
                magentoId = '';

            if (_ACTION == 'modify') {
                $.ajax({
                    url: options.getDeptUrl,
                    type: 'post',
                    data: { id: node.id, type: node.data.type },
                    async: false,
                    success: function (response) {
                        if (response.status > 0) {
                            var dept = response.data;
                            if (dept != null) {
                                deptName = dept.DeptName;
                                deptCode = dept.DeptCode;
                                storeType = dept.StoreType;
                                isPickUp = dept.IsPickUp;
                                order = dept.Order;

                                magentoId = dept.MagentoGroupID;

                                province = dept.Province;
                                city = dept.City;
                                area = dept.Area;
                                addr = dept.Address;
                                phone = dept.Telephone;
                                lat = dept.Latitude;
                                lng = dept.Longitude;
                                brand = dept.Brand;
                            }
                        }
                    }
                });
            }
            _modal.find('#DeptName').val(deptName);
            _modal.find('#DeptCode').val(deptCode);
            _modal.find('#Order').val(order);

            _modal.find('#MagentoGroupID').val(magentoId);

            _modal.find('#StoreType').val(storeType);
            _modal.find('input[name=IsPickUp][value=' + (isPickUp == false ? '0' : '1') + ']').prop('checked', true);
            _modal.find('#Province').val(province);
            _modal.find('#City').val(city);
            _modal.find('#Area').val(area);
            _modal.find('#Address').val(addr);
            _modal.find('#Telephone').val(phone);
            _modal.find('#Latitude').val(lat);
            _modal.find('#Longitude').val(lng);
            _modal.find('#Brand').val(brand);


            _modal.modal('show').find('.modal-title').text(title);
        }
        util.viewEWM = function (id) {
            $.ajax({
                url: options.getEmpUrl,
                type: 'post',
                data: { id: id },
                async: false,
                success: function (response) {
                    if (response.status > 0) {
                        var dept = response.data;
                        if (dept != null) {
                            var ewmUrl = dept.EwmUrl;

                            var _modal = $('#showEwmModal');
                            _modal.find("#EWMUrl").attr('src', ewmUrl);
                            _modal.find('#showEmpName').text(dept.NAME);
                            _modal.find('#showEwmUrl').text(ewmUrl);
                            _modal.modal('show');
                        }
                    }
                }
            });
        }
        var xjGridCust = null;
        util.viewBind = function (mobile) {

            var gridopt2 = {
                url: options.queryBindListUrl + "?Mobile=" + mobile,
                colModel: [
                        { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                        { display: '关系编号', name: 'EcId', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                        { display: '头像', name: 'IMAGE', width: "8%", sortable: false, align: 'center', process: processPic },
                        { display: '昵称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                        { display: '姓名', name: 'XM', width: "10%", sortable: false, align: 'left' },
                        { display: '手机', name: 'Phone', width: "10%", sortable: false, align: 'left' },
                        { display: '性别', name: 'xb', width: "5%", sortable: false, align: 'left' },
                        { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processBind }
                ],
                sortname: "ID",
                sortorder: "ASC",
                title: false,
                rp: 10,
                usepager: true,
                showcheckbox: false,
                autoload: true
            };
            //if (xjGridCust == null) {
            xjGridCust = new xjGrid("gridListCust", gridopt2);
            //} else {
            //    xjGridCust.Reload();
            //}

            var _modal = $('#showBindModal');
            _modal.modal('show');
        }
        function processPic(value) {
            var ops = [];
            ops.push("&nbsp;<a  href='javascript:;' onmouseout='util.hidePhoto()' onmouseover=\"util.showPhoto('" + value + "')\"><img src='", value, "' width='50px'  class='img-circle')\"></a>");
            return ops.join("");
        }
        function processBind(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='解除绑定' class='abtn' href='javascript:;' onclick=\"util.unBind('", cell[1], "')\" target='_blank'><i class='fa fa-times'></i>解除绑定</a>");
            ops.push("&nbsp;&nbsp;<a title='转移绑定' class='abtn' href='javascript:;'  onclick=\"util.transferBind('", cell[1], "')\"><i class='fa fa-retweet'></i>转移绑定</a>");
            return ops.join("");
        }
        util.showPhoto = function (id) {
        }
        util.hidePhoto = function (id) {
        }
        util.unBind = function (id) {
            $("#hdId").val(id);
            $('#confirmUnBindModal').modal('show');
        }
        $("#btnConfirmUnBind").click(function (e) {
            var id = $("#hdId").val();
            $.post(options.unBindUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功！", 'success');
                          $('#confirmUnBindModal').modal('hide');
                          xjGridCust.Reload();
                      } else {
                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
        })
        util.transferBind = function (id) {
            $("#hdIdByUpdate").val(id);
            $('#confirmTransferModal').modal('show');
        }
        $("#btnConfirmTransfer").click(function (e) {
            var id = $("#hdIdByUpdate").val();
            var phone = $("#txtPhone").val();
            $.post(options.transferBindUrl + "/" + id, { id: id, phone: phone },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功！", 'success');
                          $('#confirmTransferModal').modal('hide');
                          xjGridCust.Reload();
                      } else {
                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
        })
        util.Edit = function (id) {
            var empName = '',
                empNO = '',
                gender = '',
                mobile = '',
                userId = '',
                wechatId = '',
                email = '',
                position = '',
                brand = '',
                intro = '',
                status = '',
                fullDeptName = '';

            if (id > 0) {

                $.ajax({
                    url: options.getEmpUrl,
                    type: 'post',
                    data: { id: id },
                    async: false,
                    success: function (response) {
                        if (response.status > 0) {
                            var dept = response.data;
                            if (dept != null) {
                                empName = dept.NAME;
                                empNO = dept.EMP_NO;
                                gender = dept.GENDER;
                                mobile = dept.MOBILE;
                                userId = dept.USERID;
                                wechatId = dept.WECHAT_ID;
                                email = dept.EMAIL;
                                position = dept.POSITION;
                                brand = dept.BRAND;
                                intro = dept.INTRO;
                                status = dept.STATUS;
                                fullDeptName = dept.FullDeptName;
                            }
                        }
                    }
                });
            }

            var _modal = $('#editEmpModal');
            _modal.find("#ID").val(id);
            _modal.find('#NAME').val(empName);
            _modal.find('#EMP_NO').val(empNO);
            _modal.find('#GENDER').val(gender);
            _modal.find('#MOBILE').val(mobile);
            var _readonly = false;
            if (id > 0) {
                _readonly = true;
            }
            _modal.find('#USERID').val(userId).prop('readonly', _readonly);
            _modal.find('#WECHAT_ID').val(wechatId);
            _modal.find('#EMAIL').val(email);
            _modal.find('#POSITION').val(position);
            _modal.find('#BRAND').val(brand);
            _modal.find('#INTRO').val(intro);
            if (status != '') {
                if (status == 1) {
                    status = '已关注';
                } else if (status == 2) {
                    status = '已冻结';
                } else if (status == 4) {
                    status = '未关注';
                }
            }
            _modal.find('#EmpStatus').text(status);
            _modal.find('#spnFullDeptName').text(fullDeptName);

            _modal.modal('show');
        }

        util.Delete = function (id) {
            if (!confirm('你确定要删除该员工吗？')) {
                return;
            }

            $.ajax({
                url: options.deleteEmpUrl,
                type: 'post',
                data: { id: id },
                async: true,
                success: function (response) {
                    if (response.status > 0) {
                        xjgrid.Reload();
                        _showInfoMessage("删除成功", 'success');
                    } else {
                        _showInfoMessage("删除失败", 'error');
                    }
                }
            });
        }



        var deptTree = null;
        util.modifyStore = function (id) {
            var _modal = $('#showModifyStoreModal');

            if (deptTree == null) {
                deptTree = $('#deptTree');
                deptTree.jstree({
                    "core": {
                        'multiple': false,
                        "animation": 0,
                        "check_callback": true,
                        "themes": { "stripes": true },
                        'data': {
                            'url': function (node) {
                                //return node.id === '#' ? 'ajax_demo_roots.json' : 'ajax_demo_children.json';
                                return options.queryGroupTree;
                            },
                            'data': function (node) {
                                return { 'pid': node.id == '#' ? 0 : node.id }
                            }
                        }
                    },
                    "types": {
                        "#": {
                            "max_children": 1,
                            //"max_depth": 4,
                            "valid_children": ["root"]
                        },
                        "root": {
                            "icon": "../../assets/plugins/jquery-jstree/images/tree_icon.png",
                            "valid_children": ["default"]
                        },
                        "default": {
                            "valid_children": ["default", "file"]
                        },
                        "file": {
                            "icon": "fa fa-file",
                            "valid_children": []
                        }
                    },
                    "plugins": ["state", "types"],
                    "contextmenu": null
                })
                .on("select_node.jstree", function (e, data) {

                    _modal.find('#EmpStoreID').val(data.node.id);
                    //console.log(data);

                });
            }

            _modal.find('#EmpID').val(id);
            _modal.find('#EmpStoreID').val('0');
            _modal.modal('show');

        }
        var deptTreeOrg = null;
        util.modifyStoreOrg = function (id) {
            var _modal = $('#showModifyStoreOrgModal');

            if (deptTreeOrg == null) {
                deptTreeOrg = $('#deptTreeOrg');
                deptTreeOrg.jstree({
                    "core": {
                        'multiple': false,
                        "animation": 0,
                        "check_callback": true,
                        "themes": { "stripes": true },
                        'data': {
                            'url': function (node) {
                                return options.queryGroupTree;
                            },
                            'data': function (node) {
                                return { 'pid': (node.id == '#' ? 0 : node.id), storeId: id }
                            }
                        }
                    },
                    "types": {
                        "#": {
                            "max_children": 1,
                            //"max_depth": 4,
                            "valid_children": ["root"]
                        },
                        "root": {
                            "icon": "../../assets/plugins/jquery-jstree/images/tree_icon.png",
                            "valid_children": ["default"]
                        },
                        "default": {
                            "valid_children": ["default", "file"]
                        },
                        "file": {
                            "icon": "fa fa-file",
                            "valid_children": []
                        }
                    },
                    "plugins": ["state", "types"],
                    "contextmenu": null
                })
                .on("select_node.jstree", function (e, data) {

                    _modal.find('#ParentStoreID').val(data.node.id);

                });
            } else {
                //deptTreeOrg.data('jstree', false).empty();
                deptTreeOrg.jstree(true).settings.core.data.data = { pid: 0, storeId: id };
                deptTreeOrg.jstree('refresh');
            }

            _modal.find('#StoreID').val(id);
            _modal.find('#ParentStoreID').val('0');
            _modal.modal('show');

        }
        //调整部门架构
        $('#btnModifyStoreOrg').click(function () {
            var inst = deptTreeOrg.jstree(true);
            var sel = inst.get_selected(true);
            var selectedOne = sel[0];
            var deptId = selectedOne.id;

            var _modal = $('#showModifyStoreOrgModal');
            var storeId = _modal.find('#StoreID').val();
            $.ajax({
                url: options.updateDeptOrgUrl + '/' + storeId,
                type: 'post',
                data: { parentId: deptId },
                async: true,
                success: function (response) {
                    if (response.status > 0) {
                        myTree.jstree('refresh');
                        _modal.modal('hide');
                        _showInfoMessage("修改成功", 'success');
                    } else {
                        _showInfoMessage("修改组织架构失败。", 'error');
                    }
                }
            });
        });
        //修改员工门店
        $('#btnModifyStore').click(function () {

            var inst = deptTree.jstree(true);
            var sel = inst.get_selected(true);
            var selectedOne = sel[0];
            //console.log(selectedOne.id); return;
            var deptId = selectedOne.id;
            if (deptId == '0') {
                _showInfoMessage("组织结构不能为顶级目录。", 'info');
                return;
            }
            var _modal = $('#showModifyStoreModal');
            var empId = _modal.find('#EmpID').val();
            $.ajax({
                url: options.modifyEmpStoreUrl + '/' + empId,
                type: 'post',
                data: { deptId: selectedOne.id },
                async: true,
                success: function (response) {
                    if (response.status > 0) {
                        xjgrid.Reload();
                        _modal.modal('hide');
                        _showInfoMessage("修改成功", 'success');
                    } else {
                        _showInfoMessage("修改组织架构失败。", 'error');
                    }
                }
            });

        });






    });

})(window, undefined, jQuery);