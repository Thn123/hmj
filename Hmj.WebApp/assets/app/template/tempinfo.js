; (function (window, undefined, $) {
    var gridopt = {
        url: options.listUrl,
        colModel: [
                { display: 'ID', name: 'ID', width: "14%", sortable: false, hide: true, align: 'left', iskey: true },
                { display: '头 像', name: 'IMAGE', width: "10%", sortable: false, align: 'left', process: processAvatar },
                { display: '姓 名', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                { display: '微信id', name: 'FROMUSERNAME', width: "10%", sortable: false, align: 'left' },
        ],
        sortname: "ID",
        sortorder: "ASC",
        title: false,
        rp: 5,
        usepager: true,
        showcheckbox: false,

        autoload: false,
        onsuccess: function (data) {
            //console.log(data);
            if (data.page == 1) {
                $('#empQty').text(data.total);
            }
        }
    };
    var xjgrid = new xjGrid("gridlist", gridopt);
    function processOp(value, cell) {
        var ops = [];

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

    $("#tem").click(function () {
        $("#mainbody_content").hide();
        $("#mainbody4").hide();
        $("#mainbody_content1").removeClass("hide");
        $("#btnSave").removeClass("hide");
        $("#tem").addClass("hide");
        $('body,html').animate({
            scrollTop: 0
        }, 500);
        return false;
    })


    var multiSelectOption = {
        maxHeight: 250,
        numberDisplayed: 3,
        optionClass: function (element) {
            var value = $(element).parent().find($(element)).index();
            if (value % 2 == 0) {
                return 'even';
            }
            else {
                return 'odd';
            }
        },
        includeSelectAllOption: true,
        enableFiltering: false,
        selectAllJustVisible: true
    };
    $('#sel_search_status0').multiselect({
        maxHeight: 250,
        numberDisplayed: 3,
        optionClass: function (element) {
            var value = $(element).parent().find($(element)).index();
            if (value % 2 == 0) {
                return 'even';
            }
            else {
                return 'odd';
            }
        },
        includeSelectAllOption: false,
        enableFiltering: false,
        selectAllJustVisible: false,
        onSelectAll: function () {
            sel(0)
        },
        onDeselectAll: function () {
            //sel()
        },
        onChange: function (element, checked) {
            var selectedOptions = $('#sel_search_status0 option:selected');

            if (selectedOptions.length >= 5) {
                // Disable all other checkboxes.
                var nonSelectedOptions = $('#sel_search_status0 option').filter(function () {
                    return !$(this).is(':selected');
                });

                nonSelectedOptions.each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disabled');
                });
            }
            else {
                // Enable all checkboxes.
                $('#sel_search_status0 option').each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', false);
                    input.parent('li').addClass('disabled');
                });
            }
            if (checked === true) {
                //action taken here if true

                sel(0)


            }
            else if (checked === false) {
                sel(0)
            }
        }
    });
    $('#sel_search_status1').multiselect({
        maxHeight: 250,
        numberDisplayed: 3,
        optionClass: function (element) {
            var value = $(element).parent().find($(element)).index();
            if (value % 2 == 0) {
                return 'even';
            }
            else {
                return 'odd';
            }
        },
        includeSelectAllOption: true,
        enableFiltering: false,
        selectAllJustVisible: true,
        onSelectAll: function () {
            sel(1)
        },
        onDeselectAll: function () {
            //sel()
        },
        onChange: function (element, checked) {

            if (checked === true) {
                //action taken here if true

                sel(1)

            }
            else if (checked === false) {
                sel(1)
            }
        }
    });
    $('#sel_search_status2').multiselect({
        maxHeight: 250,
        numberDisplayed: 3,
        optionClass: function (element) {
            var value = $(element).parent().find($(element)).index();
            if (value % 2 == 0) {
                return 'even';
            }
            else {
                return 'odd';
            }
        },
        includeSelectAllOption: true,
        enableFiltering: false,
        selectAllJustVisible: true,
        onSelectAll: function () {
        },
        onDeselectAll: function () {
            //sel()
        },
        onChange: function (element, checked) {


        }
    });
    $('#sel_search_status3').multiselect({
        maxHeight: 250,
        numberDisplayed: 3,
        optionClass: function (element) {
            var value = $(element).parent().find($(element)).index();
            if (value % 2 == 0) {
                return 'even';
            }
            else {
                return 'odd';
            }
        },
        includeSelectAllOption: true,
        enableFiltering: false,
        selectAllJustVisible: true,
        onSelectAll: function () {

        },
        onDeselectAll: function () {
            //sel()
        },
        onChange: function (element, checked) {

            if (checked === true) {
                //action taken here if true



            }
            else if (checked === false) {
                if (confirm('Do you wish to deselect the element?')) {
                    //action taken here
                }
                else {
                    $("#sel_search_status2").multiselect('select', element.val());
                }
            }
        }
    });
    $('#sel_search_status4').multiselect({
        maxHeight: 250,
        numberDisplayed: 3,
        optionClass: function (element) {
            var value = $(element).parent().find($(element)).index();
            if (value % 2 == 0) {
                return 'even';
            }
            else {
                return 'odd';
            }
        },
        includeSelectAllOption: false,
        enableFiltering: false,
        selectAllJustVisible: true,
        onSelectAll: function () {
            sel()
        },
        onDeselectAll: function () {
            //sel()
        },
        onChange: function (element, checked) {
            $.ajax({
                url: options.templateUrl,
                data: { id: element.val() },
                type: "post",
                async: true,
                success: function (data) {
                    var d = jQuery.parseJSON(data);
                    $('#template_content').html(d.CONTENT);
                },
                error: function (data) {

                }
            })

        }
    });
    function sel(j) {
        var selected = [];
        $('#sel_search_status' + j + ' option:selected').each(function () {
            selected.push([$(this).val(), $(this).data('order')]);
        });

        selected.sort(function (a, b) {
            return a[1] - b[1];
        });

        var text = '';
        for (var i = 0; i < selected.length; i++) {
            text += selected[i][0] + ',';
        }
        text = text.substring(0, text.length - 1);

        $("#SelectGroupId").val(text);

        xjgrid.Query($("#formQuery")[0]);

        $.ajax({
            url: options.selstatuschange,
            data: { groupid: text },
            type: "post",
            async: true,
            success: function (data) {
                var d = jQuery.parseJSON(data);
                var data = [];
                $.each(d, function (i, item) {
                    data.push({ label: item.NAME, value: item.ID });
                })

                $('#sel_search_status' + (j + 1)).multiselect(multiSelectOption);
                $('#sel_search_status' + (j + 1)).multiselect('dataprovider', data);
            },
            error: function (data) {

            }
        })
    }


    $('#btnSave').on('click', function () {
        var selected = [];
        $('#sel_search_status2 option:selected').each(function () {
            selected.push([$(this).val(), $(this).data('order')]);
        });

        selected.sort(function (a, b) {
            return a[1] - b[1];
        });

        var text = '';
        for (var i = 0; i < selected.length; i++) {
            text += selected[i][0] + ', ';
        }
        text = text.substring(0, text.length - 2);
        var id = $('#sel_search_status4 option:checked').val();
        if (id==0) {
            alert("请选择模板")
            return false;
        }
        $.post(options.pushtemplate, { groupid: text, id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功！", 'success');
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
    });


})(window, undefined, jQuery);

