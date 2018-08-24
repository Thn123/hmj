$(function(){
	//showMsg("aa","提示","说的都是的发表反对吧肉体和肉体和人特瑞特让他忽然他和认同感和二哥二个人个人个而而二哥额");
	//showConfirm("bb","提示","是个verge如果如果不认同让他个人提供二哥如果而而个人","aa()");
    //showTip("cc", "数据加载中，请稍候...");
    tips("数据加载中，请稍候...", false, 0, true);
});

function showTip(boxID,boxMsg){
	if($("#" + boxID).length>0) $("#" + boxID).remove();
	var $showMsg = $('<div id="'+ boxID +'" class="ac_box"><div class="ac_box_tip">'+ boxMsg +'</div></div>')
	$("body").append($showMsg);
    var maskHeight = $(document).height();
    var maskWidth = $(window).width();
    var relLeft = ($(window).width() - $("#" + boxID).width())/2;
    var relTop = ($(window).height() - $("#" + boxID).height())/2;
    $(".mask").css({height:maskHeight, width:maskWidth}).fadeIn(150);
    $("#" + boxID).css({top:relTop + 'px', left:$(window).scrollLeft() + relLeft + 'px'}).fadeIn();
	setTimeout(function(){
		$(".mask, .ac_box").fadeOut(150).remove();
	},2000);
}


function showMsg(boxID,boxTitle,boxMsg){
	if($("#" + boxID).length>0) $("#" + boxID).remove();
	var $showMsg = $('<div id="'+ boxID +'" class="ac_box"><div class="ac_box_tit">'+ boxTitle +'</div><div class="ac_box_msg">'+ boxMsg +'</div><div class="ac_btn_box"><a class="ab_btn_gb close_btn" href="javascript:void(0);">关闭</a></div></div>')
	$("body").append($showMsg);
    var maskHeight = $(document).height();
    var maskWidth = $(window).width();
    var relLeft = ($(window).width() - $("#" + boxID).width())/2;
    var relTop = ($(window).height() - $("#" + boxID).height())/2;
    $(".mask").css({height:maskHeight, width:maskWidth}).fadeIn(150);
    $("#" + boxID).css({top:relTop + 'px', left:$(window).scrollLeft() + relLeft + 'px'}).fadeIn();
	$(document).on("click",".close_btn, .mask",function(){
		$(".mask, .ac_box").fadeOut(150).remove();
	});
}

function showConfirm(boxID,boxTitle,boxMsg,fn){
	if($("#" + boxID).length>0) $("#" + boxID).remove();
	var $showMsg = $('<div id="'+ boxID +'" class="ac_box"><div class="ac_box_tit">'+ boxTitle +'</div><div class="ac_box_msg">'+ boxMsg +'</div><div class="ac_btn_box"><a class="ab_btn_qx close_btn" href="javascript:void(0);">取消</a><a class="ab_btn_qr" href="javascript:void(0);" onclick="'+ fn +'">确认</a></div></div>')
	$("body").append($showMsg);
    var maskHeight = $(document).height();
    var maskWidth = $(window).width();
    var relLeft = ($(window).width() - $("#" + boxID).width())/2;
    var relTop = ($(window).height() - $("#" + boxID).height())/2;
    $(".mask").css({height:maskHeight, width:maskWidth}).fadeIn(150);
    $("#" + boxID).css({top:relTop + 'px', left:$(window).scrollLeft() + relLeft + 'px'}).fadeIn();
	$(document).on("click",".close_btn, .mask",function(){
		$(".mask, .ac_box").fadeOut(150).remove();
	});
}

