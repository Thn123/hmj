; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl + "?state=" + (getQueryString("state") || -1),
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '分组名称', name: 'GROUP_NAME', width: "8%", sortable: false, align: 'center' },
                    { display: '状态（仅有一组可用）', name: 'STATUS', width: "10%", sortable: false, align: 'left', process: StatusName },
                    { display: '备注', name: 'GROUP_RULE', width: "20%", sortable: false, align: 'left', process: Remrk },
                    { display: '创建时间', name: 'CRETE_TIME', width: "10%", sortable: false, align: 'left' },


                    { display: '操作', name: 'ID', width: "10%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        //使用状态
        function StatusName(value, cell) {
            if (value == "0") {
                value = "不可用分组";
            }

            if (value == "1") {
                value = "正在使用";
            }

            if (value == "3") {
                value = "测试用分组";
            }

            return value;
        }

        //截取字符串
        function Remrk(value, cell) {
            if (value.length <= 10) {
                return value.substring(0, 10);
            } else {
                return value.substring(0, 10) + ".....";
            }
        }


        function processOp(value, cell) {
            var ops = [];

            alert(cell)
            ops.push("&nbsp;<a title='编辑信息' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>修改</a>");

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
            $('#confirmModal').modal('show');
        });

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