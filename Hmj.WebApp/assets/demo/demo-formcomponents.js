// -------------------------------
// Demos: Form Components
// -------------------------------

$(function() {

    //FSEditor
    $(".fullscreen").fseditor({maxHeight: 500});

     // iPhone like button Toggle (uncommented because already activated in demo.js)
     // $('.toggle').toggles({on:true});

    // Autogrow Textarea
    $('textarea.autosize').autosize({append: "\n"});

    //Typeahead for Autocomplete
    $('.example-countries.typeahead').typeahead({
      name: 'countries',
      prefetch: 'assets/demo/countries.json',
      limit: 10
    });

    //Color Picker
    $('.cpicker').colorpicker();


    //Bootstrap Date Picker
    $('#datepicker,#datepicker2,#datepicker3').datepicker();
    $('#datepicker-pastdisabled').datepicker({startDate: "today"});
    $('#datepicker-startview1').datepicker({startView: 1});
    //http://eternicode.github.io/bootstrap-datepicker/


    //jQueryUI Time Picker
    $('#timepicker1,#timepicker3').timepicker();
    $("#timepicker2").timepicker({
        showPeriod: true,
        showLeadingZero: true
    });
    $('#timepickerbtn2').click(function () {
        $('#timepicker2').timepicker("show");
    });
    $("#timepicker4").timepicker({
       hours: { starts: 6, ends: 19 },
       minutes: { interval: 15 },
       rows: 3,
       showPeriodLabels: true,
       minuteText: 'Min'
    });


    // Date Range Picker
    $(document).ready(function() {
        $('#daterangepicker1').daterangepicker();
    });

    $('#daterangepicker2').daterangepicker(
        {
          ranges: {
             'Today': [moment(), moment()],
             'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
             'Last 7 Days': [moment().subtract('days', 6), moment()],
             'Last 30 Days': [moment().subtract('days', 29), moment()],
             'This Month': [moment().startOf('month'), moment().endOf('month')],
             'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
          },
          opens: 'left',
          startDate: moment().subtract('days', 29),
          endDate: moment()
        },
        function(start, end) {
            $('#daterangepicker2 span').html(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));
        }
    );

    $('#daterangepicker3').daterangepicker({ timePicker: true, timePickerIncrement: 30, format: 'MM/DD/YYYY h:mm A' });



    //Tokenfield

    $('#tokenfield-jQUI').tokenfield({
      autocomplete: {
        source: ['red','blue','green','yellow','violet','brown','purple','black','white'],
        delay: 100
      },
      showAutocompleteOnFocus: true
    });

    $('#tokenfield-typeahead').tokenfield({
      typeahead: {
        name: 'tags',
        local: ['red','blue','green','yellow','violet','brown','purple','black','white'],
      }
    });

    $('#tokenfield-email')
      .on('beforeCreateToken', function (e) {
        var token = e.token.value.split('|')
        e.token.value = token[1] || token[0]
        e.token.label = token[1] ? token[0] + ' (' + token[1] + ')' : token[0]
      })
      .on('afterCreateToken', function (e) {
        // Über-simplistic e-mail validation
        var re = /\S+@\S+\.\S+/
        var valid = re.test(e.token.value)
        if (!valid) {
          $(e.relatedTarget).addClass('invalid')
        }
      })
      .on('beforeEditToken', function (e) {
        if (e.token.label !== e.token.value) {
          var label = e.token.label.split(' (')
          e.token.value = label[0] + '|' + e.token.value
        }
      })
      .on('removeToken', function (e) {
        alert('Token removed! Token value was: ' + e.token.value)
      })
      .on('preventDuplicateToken', function (e) {
        alert('Duplicate detected! Token value is: ' + e.token.value)
      })
      .tokenfield();

    //SELECT2

    //For detailed documentation, see: http://ivaynberg.github.io/select2/index.html

    //Populate all select boxes with from select#source
    var opts=$("#source").html(), opts2="<option></option>"+opts;
    $("select.populate").each(function() { var e=$(this); e.html(e.hasClass("placeholder")?opts2:opts); });

    //select2
    $("#e1,#e2").select2({ width: 'resolve' });

    $("#e3").select2({
            minimumInputLength: 2,
            width: 'resolve'
        });

    $("#e5").select2({
        minimumInputLength: 1,
        width: 'resolve',
        query: function (query) {
            var data = {results: []}, i, j, s;
            for (i = 1; i < 5; i++) {
                s = "";
                for (j = 0; j < i; j++) {s = s + query.term;}
                data.results.push({id: query.term + i, text: s});
            }
            query.callback(data);
        }
    });

    $("#e12").select2({width: "resolve", tags:["red", "white", "purple", "orange", "yellow"]});


    $("#e9").select2({width: 'resolve'});


    //Rotten Tomatoes Infinite Scroll + Remote Data example
    var cusurl = "/Customer/QueryCustomer.do?type=name";
    $("#CUST_NAME").select2({
        placeholder: "姓名/电话",
        minimumInputLength: 2,
        width: 'resolve',
        ajax: {
            url: cusurl,
            dataType: 'json',
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    q: term, //search term
                    page_limit: 10, // page size
                    page: page // page number
                    //apikey: "q7jnbsc56ysdyvvbeanghegk" // please do not use so this example keeps working
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available
                // notice we return the value of more so Select2 knows if more results can be loaded
                var str = eval("(" + data.data + ")");
                return { results: str, more: more };
            }
        },
        formatResult: movieFormatResult,
        formatSelection: movieFormatSelection,
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
    });
    var cusurls = "/Order/QueryCustomer.do";
    $("#PROD_NAME_PRICE").select2({
        placeholder: "输入项目名、价格进行查询",
        minimumInputLength: 2,
        width: 'resolve',
        ajax: {
            url: cusurls,
            dataType: 'json',
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    q: term, //search term
                    page_limit: 10, // page size
                    page: page // page number
                    //apikey: "q7jnbsc56ysdyvvbeanghegk" // please do not use so this example keeps working
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available
                // notice we return the value of more so Select2 knows if more results can be loaded
                var str = eval("(" + data.data + ")");
                //var sd = [{ "title": "12升圆形脚踏静音垃圾桶", "id": "12升圆形脚踏静音垃圾桶", "cust_no": "1328", "critics_consensus": "0.00" }, { "title": "12月会刊", "id": "12月会刊", "cust_no": "1473", "critics_consensus": "0.00" }, { "title": "2012年情人节特价", "id": "2012年情人节特价", "cust_no": "175", "critics_consensus": "0.00" }, { "title": "佰草集07年12月会刊", "id": "佰草集07年12月会刊", "cust_no": "1533", "critics_consensus": "0.00" }, { "title": "佰草集汉方SPA昊氲沐浴乳(250ML)", "id": "佰草集汉方SPA昊氲沐浴乳(250ML)", "cust_no": "1054", "critics_consensus": "120.00" }, { "title": "佰草集汉方SPA昊氲洗发露", "id": "佰草集汉方SPA昊氲洗发露", "cust_no": "1052", "critics_consensus": "120.00" }, { "title": "佰草集汉方SPA金致玉妍紧致露(120G)", "id": "佰草集汉方SPA金致玉妍紧致露(120G)", "cust_no": "1123", "critics_consensus": "600.00" }, { "title": "佰草集汉方SPA金致玉妍紧致面膜（120G）", "id": "佰草集汉方SPA金致玉妍紧致面膜（120G）", "cust_no": "1139", "critics_consensus": "0.00" }, { "title": "佰草集汉方SPA金致玉妍御颜膏(120G)", "id": "佰草集汉方SPA金致玉妍御颜膏(120G)", "cust_no": "1124", "critics_consensus": "250.00" }, { "title": "佰草集汉方SPA矜纯沐浴乳(250ML)", "id": "佰草集汉方SPA矜纯沐浴乳(250ML)", "cust_no": "1055", "critics_consensus": "120.00" }];
                return { results: str, more: more };
            }
        },
        formatResult: goodsFormatResult,
        formatSelection: movieFormatSelection,
        dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
    });

    //输入产品名称 价格
    //start


   // var cusurls = "/Goods/QueryGodSerCard.do";
    //var cusurls = "/Customer/QueryCustomer.do?type=name";
    //$("#PROD_NAME_PRICE").select2({
    //    placeholder: "输入项目名、价格进行查询",
    //    minimumInputLength: 2,
    //    width: 'resolve',
    //    ajax: {
    //        url: cusurls,
    //        dataType: 'json',
    //        quietMillis: 100,
    //        data: function (term, page) { // page is the one-based page number tracked by Select2
    //            return {
    //                q: term, //search term
    //                page_limit: 10, // page size
    //                page: page // page number
    //                //apikey: "q7jnbsc56ysdyvvbeanghegk" // please do not use so this example keeps working
    //            };
    //        },
    //        results: function (data, page) {
    //            var datas=[{"title": "佰草集汉方SPA矜纯沐浴乳(250ML)", "id": "1055", "price": "120.00", "critics_consensus": "120.00" }];
    //            var more = (page * 10) < data.total; // whether or not there are more results available
    //            // notice we return the value of more so Select2 knows if more results can be loaded
    //            var str = eval("(" + datas + ")");
    //           var sd=[{ "title": "罗永浩", "id": "罗永浩", "cust_no": "00020100201", "critics_consensus": "13992988709" }]
    //           return { results: sd, more: more };
    //        }
    //    },
    //    formatResult: movieFormatResult,
    //    formatSelection: movieFormatSelection,
    //    dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
    //    escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
    //});
   //end


  //MULTISELECT2

  // For detailed documentatin, see: loudev.com

$('#multi-select2').multiSelect();

$('#multi-select').multiSelect({
  selectableHeader: "<input type='text' class='form-control' style='margin-bottom: 10px;'  autocomplete='off' placeholder='Filter entries...'>",
  selectionHeader: "<input type='text' class='form-control' style='margin-bottom: 10px;' autocomplete='off' placeholder='Filter entries...'>",
  afterInit: function(ms){
    var that = this,
        $selectableSearch = that.$selectableUl.prev(),
        $selectionSearch = that.$selectionUl.prev(),
        selectableSearchString = '#'+that.$container.attr('id')+' .ms-elem-selectable:not(.ms-selected)',
        selectionSearchString = '#'+that.$container.attr('id')+' .ms-elem-selection.ms-selected';

    that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
    .on('keydown', function(e){
      if (e.which === 40){
        that.$selectableUl.focus();
        return false;
      }
    });

    that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
    .on('keydown', function(e){
      if (e.which == 40){
        that.$selectionUl.focus();
        return false;
      }
    });
  },
  afterSelect: function(){
    this.qs1.cache();
    this.qs2.cache();
  },
  afterDeselect: function(){
    this.qs1.cache();
    this.qs2.cache();
  }
  });


});








function movieFormatResult(movie) {
        var markup = "<table class='movie-result'><tr>";
        if (movie.posters !== undefined && movie.posters.thumbnail !== undefined) {
            markup += "<td class='movie-image'><img src='" + movie.posters.thumbnail + "'/></td>";
        }
        markup += "<td class='movie-info'><div class='movie-title'>" + movie.title + "</div>";
        if (movie.critics_consensus !== undefined) {
            markup += "<div class='movie-synopsis'>" + movie.critics_consensus + "</div>";
        }
        else if (movie.synopsis !== undefined) {
            markup += "<div class='movie-synopsis'>" + movie.synopsis + "</div>";
        }
        markup += "</td></tr></table>"
        return markup;
    }

function movieFormatSelection(movie) {
    $("#custmobile").html(movie.critics_consensus);
    $("#custcard").html("10000102");
        return movie.title;
}

function goodsFormatResult(obj) {
    var markup = "<table class='movie-result'><tr>";
    if (obj.posters !== undefined && obj.posters.thumbnail !== undefined) {
        markup += "<td class='movie-image'><img src='" + obj.posters.thumbnail + "'/></td>";
    }
    markup += "<td class='movie-info'><div class='movie-title'>" + obj.title + "</div>";
    if (obj.critics_consensus !== undefined) {
        markup += "<div class='movie-synopsis'>￥：" + obj.critics_consensus + "</div>";
    }
    else if (obj.synopsis !== undefined) {
        markup += "<div class='movie-synopsis'>" + obj.synopsis + "</div>";
    }
    markup += "</td></tr></table>"
    return markup;
}