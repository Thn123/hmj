//倒计时

var num=60;	
var timerrr;
var flag=true;
var daojishi=document.getElementById('timeid');
function dao(){
	if(num>0){
        num -= 1;
        daojishi.innerHTML  = num;
	    if(num<10){
            daojishi.innerHTML ="0"+num;
		}
	}
	if(num==0){
        daojishi.innerHTML ="验证";
		num=60;	
		flag=true;
		clearInterval(timerrr);
	}
	
}	
$("#timeid").on("tap",function(){	
	if(flag==true){
		dao();
		timerrr=setInterval('dao()',1000);
		alert("发送验证码操作！");
		flag=false;
	}
})



function jian(){
	 var num = document.getElementById("carnum"); 
	 if(parseInt(num.value)>1){
       num.value = parseInt(num.value) - 1; 
	 }
}
function add(){
    var num = document.getElementById("carnum"); 
    num.value = parseInt(num.value) + 1; 
}
