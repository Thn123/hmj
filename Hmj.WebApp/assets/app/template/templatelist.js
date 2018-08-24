; (function (window, undefined, $) {
    var gridopt = {
        url: options.listUrl,
        colModel: [
                { display: 'ID', name: 'ID', width: "14%", sortable: false, hide: true, align: 'left', iskey: true },
                { display: '模板ID', name: 'TEMPLATE_ID', width: "10%", sortable: true, align: 'left' },
                { display: '模板名称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                { display: '发送成功', name: 'SEND_SUCCESS_NUM', width: "10%", sortable: false, align: 'left' },
                { display: '发送失败', name: 'SEND_FAILED_NUM', width: "10%", sortable: false, align: 'left' },
                { display: '发送时间', name: 'CREATE_TIME', width: "10%", sortable: true, align: 'left' },
        ],
        sortname: "CREATE_TIME",
        sortorder: "desc",
        title: false,
        rp: 5,
        usepager: true,
        showcheckbox: false,

        autoload: true,
        onsuccess: function (data) {
            //console.log(data);
            if (data.page == 1) {
                $('#empQty').text(data.total);
            }
        }
    };
    var xjgrid = new xjGrid("gridlist", gridopt);

   

})(window, undefined, jQuery);

