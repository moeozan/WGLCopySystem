//Assets/Plugins/GetURL.jslib
 mergeInto(LibraryManager.library, {
  
     GetURLFromPage: function () {
 	var s ="";
  	var strUrl = window.location.search;
	var getSearch = strUrl.split("?");
         if(getSearch == ""){ return; } 
         else{ 
	var getPara = getSearch[1].split("&");
  	var v1 = getPara[0].split("=");

   //Get size of the string
   	var bufferSize = lengthBytesUTF8(v1[1]) + 1;
   //Allocate memory space
   	var buffer = _malloc(bufferSize);
   //Copy old data to the new one then return it
   	stringToUTF8(v1[1], buffer, bufferSize);
   	return buffer;

	}
}
});
