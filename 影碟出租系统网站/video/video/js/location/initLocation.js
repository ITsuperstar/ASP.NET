/*
 * 改程序封装了GlobalProvinces_extend.js、GlobalProvinces_main.js两个文件中的对象和数组
 * 
 * @作者：侯伟
 * @创建日期：2007-07-20
 * 
 * 
 */


function initLocation(option)
{
	
	option = jQuery.extend({
		sheng:"sheng",		//省的网页ID
		shi:"shi",			//市的网页ID
		xian:"xian",		//县的网页ID
		xiang:"xiang",		//乡的网页ID
		sheng_val:"",		//默认省份
		shi_val:"",			//默认地区
		xian_val:"",		//默认县
		xiang_val:""		//默认乡镇
	},option||{});
	
	
	if(option.sheng_val == ""){
		option.sheng_val == "-1";
	}
		
	var gpm = new GlobalProvincesModule;



	gpm.def_province = ["---", -1];

	gpm.initProvince(document.getElementById(option.sheng));
	
	gpm.initCity1(document.getElementById(option.shi), option.sheng_val);

	gpm.initCity2(document.getElementById(option.xian), option.sheng_val, option.shi_val);

	gpm.initCity3(document.getElementById(option.xiang), option.sheng_val, option.shi_val, option.xian_val);


	gpm.selectProvincesItem(document.getElementById(option.sheng), option.sheng_val);

	gpm.selectCity2Item(document.getElementById(option.xian), option.xian_val);

	gpm.selectCity1Item(document.getElementById(option.shi), option.shi_val);

	
	
	if(document.getElementById(option.xiang).options.length > 1){
		gpm.selectCity2Item(document.getElementById(option.xiang), option.xiang_val);
		document.getElementById(option.xiang).style.display ="inline";
		document.getElementById(option.xiang).style.display = "inline";
	}

	



	var onchgProv = function()
	{	
		gpm.initCity1(document.getElementById(option.shi), gpm.getSelValue(document.getElementById(option.sheng)));
		gpm.initCity2(document.getElementById(option.xian), '', '');		/* clear city2 select options*/
		gpm.initCity3(document.getElementById(option.xiang), '', '', '');
		$("#"+option.xiang).hide();
		
	}
	var onchgCity1 = function()
	{
		gpm.initCity2(document.getElementById(option.xian), gpm.getSelValue(document.getElementById(option.sheng)), gpm.getSelValue(document.getElementById(option.shi)));
		gpm.initCity3(document.getElementById(option.xiang), '', '', '');
		$("#"+option.xiang).hide();
		
	}

	var onchgStreet1 = function(){
		
		gpm.initCity3(document.getElementById(option.xiang), gpm.getSelValue(document.getElementById(option.sheng)), gpm.getSelValue(document.getElementById(option.shi)), gpm.getSelValue(document.getElementById(option.xian)));

		if($("#"+option.xiang).children().length > 1) {
				$("#"+option.xiang).show();
		} else {
				$("#"+option.xiang).hide();
		}
	}


	if(option.xiang_val == "") 
		$("#"+option.xiang).hide();
	$("#"+option.sheng).change(onchgProv);
	$("#"+option.shi).change(onchgCity1);
	$("#"+option.xian).change(onchgStreet1);
	
	



}