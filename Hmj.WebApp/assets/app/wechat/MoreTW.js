; (function (window, undefined, $) {

    $(document).ready(function () {

        function showinput(id) {
            $("#pid" + id).hide();
            $("#txtid" + id).fadeIn();
        }
        function titledd(id) {
            var title = $("#title" + id).val();
            var tit = "#title" + $("#TWID").val();
            //alert($("#TWID").val() == 1);
            if ($("#TWID").val() == 1) {
                $(".duoTitle").find("a").html(title);
            }
            else {
                //alert(title);
                $(tit).html(title);
            }
        }

        var opp = document.getElementById('addduotu').getElementsByTagName("ol");
        function addtuwen() {
            if (opp.length > 6) {
                alert("您最多只能添加8个菜单");
            } else {
                document.getElementById('addduotu').innerHTML += '<ol>  <img id="img' + (parseInt(opp.length) + 2) + '" class="morenxiaotu" src="" /><span id="title' + (parseInt(opp.length) + 2) + '">标题</span><div class="imgEditbg"><em><a href="javascript:yidong3(' + opp.length + ');" class="editda"></a>  &nbsp; <i><a href="javascript:void(0);" class="delda" onmouseover="deltuwen()"></i></em></a></div></ol>'
                // alert(document.getElementById('addduotu').innerHTML);
            }
        }

        function deltuwen() {
            savetw();
            var eol = document.getElementById('addduotu').getElementsByTagName("ol");
            var eii = document.getElementById('addduotu').getElementsByTagName("i");
            for (var i = 0; i < eii.length; i++) {
                eii[i].index = i;
                eii[i].onclick = function () {
                    deltw(parseInt(this.index + 3));
                    document.getElementById('addduotu').removeChild(eol[this.index + 1]);
                    //yidong2();
                }
            }
        }

        function yidong() {
            savetw();
            $("#TWID").val(2);
            $(".inputbox").animate({ marginTop: "175px" }, 500);
            showtw(2);
        }
        function yidong2() {
            savetw();
            $("#TWID").val(1);
            $(".inputbox").animate({ marginTop: "0" }, 500); $(".inputsanjiao").animate({ top: "40px" }, 500);
            showtw(1);
        }
        function yidong3(id) {
            savetw();
            $("#TWID").val(id + 2);
            var changdu = (id - 1) * 101 + 258 - (id - 1) * 30 + "px";
            var mardu = (id - 1) * 30 + 58 + "px";
            $(".inputbox").animate({ marginTop: mardu }, 500);
            $(".inputsanjiao").animate({ top: changdu }, 500);

            showtw((id + 2));
        }


        function deltw(id) {
            $.post("DelGraphicByRowID.do", {
                rowid: id,
                listid: $("#List_ID").val()
            },
                      function (ret) {
                          if (ret && ret.status == 0) {

                              location = "MoreTW.do?id=" + $("#List_ID").val();
                          }
                          else {
                              _showInfoMessage("加载失败！", 'error');
                          }
                      },
                      "json"
                );
        }

        function showtw(id) {
            //alert(id);
            $.post("GetGraphicByRowID.do", {
                rowid: id,
                listid: $("#List_ID").val()
            },
                      function (ret) {
                          if (ret && ret.status == 0) {
                              $("#title1").val(ret.data.Title);
                              options.myeditor.setData(ret.data.Body);
                              $("#ID").val(ret.data.ID);
                              $("#FILE_URL").val(ret.data.Pic);
                              $("#Describe").val(ret.data.Describe);
                              $("#URL").val(ret.data.URL);

                          }
                          else {
                              _showInfoMessage("加载失败！", 'error');
                          }
                      },
                      "json"
                );
        }

        function FileUpload_onselect() {
            ajaxFileUpload();
        }



        $(function () {
            $(".greenbtn").click(function () {
                //alert($("#FILE_URL").val());
                if ($("#FILE_URL").val() == "") {
                    alert("请上传图片");
                    return;
                }
                $("#Body").val(options.myeditor.getData());
                if ($("#Body").val() == "" && $("#URL").val() == "") {
                    alert("请输入正文或添加原文链接");
                    return;
                }
                $("#ISFB").val(1);
                savetw();
            })
        })

        function savetw() {
            $("#Body").val(options.myeditor.getData());
            //alert(options.myeditor.getData());
            //alert($("#Body").val());
            $("#frmSave").submit();
        }

        $(document).ready(function () {
            $('#frmSave').validator({
                rules: {
                },
                fields: {

                },
                valid: function (form) {
                    FormSubmit(form, function (res) {
                        if (res.status == 0) {
                            $("#List_ID").val(res.data.List_ID);

                            //var tit = "#title" + $("#TWID").val();
                            ////alert($("#TWID").val() == 1);
                            //if ($("#TWID").val() == 1) {
                            //    $(".duoTitle").find("a").html(res.data.Title);
                            //}
                            //else {
                            //    $(tit).html(res.data.Title);
                            //}

                            _showInfoMessage("保存成功！", 'success');
                            if ($("#ISFB").val() == 1) {
                                location = "GraphicShow.do";
                            }
                            //$('#EditModal').modal('hide');
                            //xjgrid.Reload();
                        }
                        else {
                            _showInfoMessage("保存失败：" + res.message, 'error');
                            //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                        }
                    })
                }
            });
        });

        function ajaxFileUpload() {
            $.ajaxFileUpload
            (
                {
                    url: 'UploadImage.do', //用于文件上传的服务器端请求地址
                    type: 'post',
                    data: { Id: '123', name: 'lunis' }, //此参数非常严谨，写错一个引号都不行
                    secureuri: false, //一般设置为false
                    fileElementId: 'file1', //文件上传空间的id属性  <input type="file" id="file" name="file" />
                    dataType: 'json', //返回值类型 一般设置为json
                    success: function (data, status, Message)  //服务器成功响应处理函数
                    {
                        //alert(data.data.FILE_URL);
                        var objimg = "#img" + $("#TWID").val();
                        //alert(objimg);
                        $(objimg).attr("src", data.data.FILE_NAME);
                        $("#FILE_URL").val(data.data.FILE_URL);
                        if (typeof (data.error) != 'undefined') {
                            if (data.error != '') {
                                alert(data.error);
                            } else {
                                alert(data.msg);
                            }
                        }
                    },
                    error: function (data, status, e)//服务器响应失败处理函数
                    {
                        alert(e);
                    }
                }
            )
            return false;
        }


    });

})(window, undefined, jQuery);