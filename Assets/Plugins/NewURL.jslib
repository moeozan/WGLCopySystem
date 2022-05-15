//Assets/Plugins/NewURL.jslib
 mergeInto(LibraryManager.library, {
  
     SendURL: function (newClipText) {
	var clipText = UTF8ToString(newClipText);
	if (navigator.clipboard && window.isSecureContext) {
        return navigator.clipboard.writeText(clipText);
    	}
}
});