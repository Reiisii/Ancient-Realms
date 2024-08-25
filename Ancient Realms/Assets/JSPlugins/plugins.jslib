mergeInto(LibraryManager.library, {
    Warn: function (str) {
        console.log("triggered");
        console.warn(Pointer_stringify(str));
    },
    Confirmation: function(){
        var dialogText = "You are sure you want to quit?";
        return dialogText;
    }
});