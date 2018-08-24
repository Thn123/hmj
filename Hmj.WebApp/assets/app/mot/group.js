; (function (window, undefined, $) {

    $(document).ready(function () {

        if ($("#ISADMIN").val() == "True") {
            $("#storeDiv").show();
        }
        else {
            $("#storeDiv").hide();
        }

        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "10%", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '策略名称', name: 'ROOM_NO', width: "10%", sortable: false, align: 'left' },
                    { display: '版本号', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '策略类型', name: 'STORE_NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '策略描述', name: 'REMARK', width: "20%", sortable: false, align: 'left' },
                    { display: '创建人', name: 'REMARK', width: "20%", sortable: false, align: 'left' },
                    { display: '创建日期', name: 'REMARK', width: "20%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='编辑' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");
            ops.push("&nbsp;&nbsp;<a title='删除' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
        
            $('#EditModal').modal('show');
        });

        util.Edit = function (id) {
            $.post(options.editUrl, {
                id: id
            },
                 function (ret) {
                     if (ret && ret.status == 0) {
                         $("#ID").val(ret.data.ID);
                         $("#ROOM_NO").val(ret.data.ROOM_NO);
                         $("#STORE_ID").val(ret.data.STORE_ID);
                         $("#STORE_ID1").val(ret.data.STORE_ID);
                         $("#NAME").val(ret.data.NAME);
                         $("#BED_NUM").val(ret.data.BED_NUM);
                         $("#REMARK").val(ret.data.REMARK);
                     }
                     else {
                         _showInfoMessage("数据库中没有此房间信息，请刷新重试！", 'error');
                     }
                 },
                 "json"
           );
            $('#EditModal').modal('show');
        };

        $("#btnSave").click(function (e) {
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#ROOM_NO': 'required',
                '#NAME': 'required',
                '#BED_NUM': 'required;digits',
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

        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };
        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl + "/" + id, { id: id },
                  function (res) {
                      showErrorTip(res.message, { right: 100, top: 10 }, true, 5000);
                      if (res.status == 0) {
                          _showInfoMessage("删除成功！", 'success');
                          xjgrid.Reload();
                      }
                      else {
                          _showInfoMessage("删除失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
        })

        
     
        $("#tr1").click(function ()
        {
           
            var t= "";
                         t+=" <tr ><th  >姓名</th><th>账号</th><th>电话</th></tr>"
                         t += " <tr><td  ><a href='../customer/View360.do?cid=57739&mot=1'>赵洁</a></td><td>8391</td><td>15921990617</td></tr>";
                         t += " <tr><td  ><a href='../customer/View360.do?cid=57740&mot=1'>钱晴</a></td><td>8390</td><td>13916537228</td></tr>";
                         t += "<tr><td  ><a href='../customer/View360.do?cid=57741&mot=1'>王天杭</a></td><td>8389</td><td>13661558451</td></tr>";
                         t += "<tr><td  ><a href='../customer/View360.do?cid=57742&mot=1'>CHOW SILAS周</a></td><td>8388</td><td>13817817581</td></tr>";
                         t += "</table>";

                         $("#table").html(t);
        })


        $("#tr2").click(function () {

            var t = "";
            t += " <tr ><th  >姓名</th><th>账号</th><th>电话</th></tr>"
            t += " <tr><td  ><a href='../customer/View360.do?cid=57743&mot=1'>杨慧</a></td><td>8387</td><td>15921161337</td></tr>";
            t += "</table>";

            $("#table").html(t);
        })



        $("#tr3").click(function () {

            var t = "";
            t += " <tr ><th  >姓名</th><th>账号</th><th>电话</th></tr>"
            t += " <tr><td  ><a href='../customer/View360.do?cid=57744&mot=1'>徐娜佳</a></td><td>8391</td><td>15921990617</td></tr>";
            t += " <tr><td  ><a href='../customer/View360.do?cid=57745&mot=1'>丁志雄</a></td><td>8390</td><td>13916537228</td></tr>";
            t += "<tr><td  ><a href='../customer/View360.do?cid=57746&mot=1'>Mever-Galow Phihpp</a></td><td>8389</td><td>13661558451</td></tr>";
           
            t += "</table>";

            $("#table").html(t);
        })



        $("#tr4").click(function () {

            var t = "";
            t += " <tr ><th  >姓名</th><th>账号</th><th>电话</th></tr>"
            t += " <tr><td  ><a href='../customer/View360.do?cid=57747&mot=1'>马秀梅</a></td><td>8323</td><td>15921990617</td></tr>";
            t += " <tr><td  ><a href='../customer/View360.do?cid=57748&mot=1'>Voiona 柯婉慧</a></td><td>8324</td><td>13916537228</td></tr>";
            t += "<tr><td  ><a href='../customer/View360.do?cid=57749&mot=1'>cheung audrey</a></td><td>8325</td><td>13661558451</td></tr>";
           
            t += "</table>";

            $("#table").html(t);
        })


        $("#tr5").click(function () {

            var t = "";
            t += " <tr ><th  >姓名</th><th>账号</th><th>电话</th></tr>"
            t += " <tr><td  ><a href='../customer/View360.do?cid=57750&mot=1'>IVEY DEIDRA/CRAIG</a></td><td>8332</td><td>15921990617</td></tr>";
            t += " <tr><td  ><a href='../customer/View360.do?cid=57751&mot=1'>郝信星</a></td><td>8395</td><td>13916537228</td></tr>";
          
            t += "</table>";

            $("#table").html(t);
        })

    


    });
})(window, undefined, jQuery);