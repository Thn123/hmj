(function(){
var now = { row:1, col:1 }, last = { row:0, col:0};
const towards = { up:1, right:2, down:3, left:4};
var isAnimating = false;

s=window.innerHeight/500;
ss=250*(1-s);

$('.wrap').css('-webkit-transform','scale('+s+','+s+') translate(0px,-'+ss+'px)');

document.addEventListener('touchmove',function(event){
	event.preventDefault(); },false);

$(document).swipeUp(function(){
	if (isAnimating) return;
	last.row = now.row;
	last.col = now.col;
	if (last.row != 4) { now.row = last.row+1; now.col = 1; pageMove(towards.up);}	
	
})

$(document).swipeDown(function(){
	if (isAnimating) return;
	last.row = now.row;
	last.col = now.col;
	if (last.row!=1) { now.row = last.row-1; now.col = 1; pageMove(towards.down);}	
})


function pageMove(tw){
	var lastPage = ".page-"+last.row+"-"+last.col,
     	nowPage = ".page-"+now.row+"-"+now.col;
	
	switch(tw) {
		case towards.up:
			outClass = 'pt-page-moveToTop';
			inClass = 'pt-page-moveFromBottom';
			break;
		case towards.right:
			outClass = 'pt-page-moveToRight';
			inClass = 'pt-page-moveFromLeft';
			break;
		case towards.down:
			outClass = 'pt-page-moveToBottom';
			inClass = 'pt-page-moveFromTop';
			break;
		case towards.left:
			outClass = 'pt-page-moveToLeft';
			inClass = 'pt-page-moveFromRight';
			break;
	}
	isAnimating = true;
	$(nowPage).removeClass("hide");
	
	$(lastPage).addClass(outClass);
	$(nowPage).addClass(inClass);
	
	setTimeout(function(){
		$(lastPage).removeClass('page-current');
		$(lastPage).removeClass(outClass);
		$(lastPage).addClass("hide");
		$(lastPage).find("img").addClass("hide");
		
		$(nowPage).addClass('page-current');
		$(nowPage).removeClass(inClass);
		$(nowPage).find("img").removeClass("hide");
		
		isAnimating = false;
	},300);
}
})();



$('#yue').swipeRight(function(){
   $(this).addClass('img_3');
   setTimeout(function(){window.location.href="http://m.51job.com/search/joblist.php?jobarea=020000&keyword=%E4%B8%BD%E4%BA%BA%E4%B8%BD%E5%A6%86&from=index";},1000);
});

function huadong(id){
	var imgnum=0;
	var zuimg=document.getElementById('zutu'+id);
	
	$('.photocon ul li').swipeLeft(function(){
		imgnum++;
		startMove(zuimg,{left:-300*imgnum});
		if(imgnum>=2){
			imgnum=-1;
			startMove(zuimg,{left:-300*2});
		 }
	});
	
	
	$('.photocon ul li').swipeRight(function(){
		imgnum--;	
		if(imgnum<=-1){
			startMove(zuimg,{left:-300*2});
			imgnum=2;
			}else{
			startMove(zuimg,{left:-300*imgnum});
			}
	});
}

huadong(1);
huadong(2);
huadong(3);
huadong(4);
huadong(5);
huadong(6);
huadong(7);
huadong(8);
huadong(9);