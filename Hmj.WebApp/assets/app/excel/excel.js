; (function (window, undefined, $) {

    $(document).ready(function () {
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'STORE_ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '门店名称', name: 'STORE_NAME', width: "8%", sortable: false, align: 'center' },
                    { display: '门店编码', name: 'STORE_CODE', width: "10%", sortable: false, align: 'left'},
                    { display: '绑定会员数量', name: 'MEMBER_COUNT', width: "20%", sortable: false, align: 'left' },
                    { display: '粉丝数量', name: 'FANS_COUNT', width: "10%", sortable: false, align: 'left' },


                    { display: '操作', name: 'STORE_ID', width: "10%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "ID",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);


        $('#BeginTime').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        }).on('hide', function (ev) {
            var date = this.value;
            $(this).val(date);
        }).val(function () {
            var date = this.value;
            return date;
        });
        $('#EndTime').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        }).on('hide', function (ev) {
            var date = this.value;
            $(this).val(date);
        }).val(function () {
            var date = this.value;
            return date;
        });

        function processOp(value, cell) {
            var ops = [];

            ops.push("&nbsp;<a title='查看列表' class='abtn' href='javascript:;'  onclick=\"util.Edit('",
                value, "')\"><i class='fa fa-edit' ></i>查看</a>");

            return ops.join("");
        }

        util.Edit = function (url) {
            $('#confirmModal').modal('show');

            var gridopt = {
                url: options.fansUrl,
                colModel: [
                        { display: '编号', name: 'STORE_ID', width: "0", sortable: true, hide: true, align: 'left', iskey: true },
                        { display: '门店名称', name: 'STORE_NAME', width: "8%", sortable: false, align: 'center' },
                        { display: '门店编码', name: 'STORE_CODE', width: "10%", sortable: false, align: 'left' },
                        { display: '绑定会员数量', name: 'MEMBER_COUNT', width: "20%", sortable: false, align: 'left' },
                        { display: '粉丝数量', name: 'FANS_COUNT', width: "10%", sortable: false, align: 'left' },

                        { display: '操作', name: 'STORE_ID', width: "10%", sortable: false, align: 'center', process: processOp }
                ],
                sortname: "ID",
                sortorder: "ASC",
                title: false,
                rp: 15,
                usepager: true,
                showcheckbox: false
            };
            var xjgrid = new xjGrid("gridlisttwo", gridopt);
        }

    });

})(window, undefined, jQuery);