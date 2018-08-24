; (function (window, undefined, $) {

    $(document).ready(function () {

        function showinput(id) {
            $("#pid" + id).hide();
            $("#txtid" + id).fadeIn();
        }

        function titledd(id) {
            var title = $("#Title").val();
            $(".tuwenTitle").find("a").html(title);
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
                $("#frmSave").submit();
            })
        })
        $(document).ready(function () {
            $('#frmSave').validator({
                rules: {
                },
                fields: {

                },
                valid: function (form) {
                    FormSubmit(form, function (res) {
                        if (res.status == 0) {
                            _showInfoMessage("操作成功！", 'success');
                            location = "GraphicShow.do";
                            //$('#EditModal').modal('hide');
                            //xjgrid.Reload();
                        }
                        else {
                            _showInfoMessage("操作失败：" + res.message, 'error');
                            //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                        }
                    })
                }
            });
        });

       


    });

})(window, undefined, jQuery);