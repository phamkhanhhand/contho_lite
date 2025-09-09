String.prototype.format = function () {
    var formatted = this;
    for (var arg in arguments) {
        formatted = formatted.replace("{" + arg + "}", arguments[arg]);
    }
    return formatted;
};


if (typeof PKHA == "undefined") {
    PKHA = {};
}

PKHA.commonFn = {

    //uuidv4() {
    //    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
    //        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    //    );
    //},

    //#region ajax
    /*
     * Ajax
     * phamha
     * */
    ajax(url, params, callbackFunction, cfg = { method: "", isSync: true }) {

        if (cfg) {
            cfg.method = cfg.method ? cfg.method : "GET";
        } else {
            cfg = { isSync: true };
        }

        $.ajax({
            url: url,
            context: document.body,
            async: cfg.isSync,
            dataType: "json",
            data: params,
            method: cfg.method,
        }).done(function (arguments) {

            if (callbackFunction) {
                callbackFunction(arguments);
            }

        });
    },


    /*
     * Ajax lấy dữ liệu để bind control, sau đó là bind data
     * phamha
     * */
    getAndBindControl(url, callbackFunction, entity = {}, isSync = true) {

        $.ajax({
            url: url,
            context: document.body,
            async: isSync,
        }).done(function (arguments) {

            if (callbackFunction) {
                callbackFunction(arguments, entity);
            }

        });
    },

    //#endregion


    //#region showMask

    /*
     * Show mask container truyền vào
     * phamha
     * */
    showMask(container) {

        if (container) {
            var loadDing = '<div class="mask-loading"><img src="/Base/Icon/loading.gif" alt="Loading..." /></div>';
            container.append(loadDing);
        }
    },

    /*
     * Xóa mask trong container truyền vào
     * phamha
     * */
    hideMask(container) {
        if (container && container[0]) {
            var containerClass = container[0].className;
            $('.' + containerClass + ' .mask-loading').remove();
        }
    },

    /*
     * Xóa mask trong container truyền vào
     * phamha
     * */
    showUC(container, cfg) {
        var key = cfg.key,
            uiConfig = UiConfig.find(x => x.key == key);

        if (uiConfig) {
            var id = PKHA.commonFn.uuidv4();
            var uiID = 'dialog_' + id;
            //TODO nếu có rồi thì chỉ hiện lên thôi (cần thiết không?)
            var frame = `
            <div class="popup-frame" id="`+ uiID + `" title="đây là popup">
                <iframe style="border: 0px;" src="/Common/FrameUC?ucURL=`+ uiConfig.ascxLink + `" width="` + uiConfig.width + `" height="` + uiConfig.height + `" ></iframe>
            </div>
           `;

            if (container) {

                container.children().hide();
                container.append(frame);
            }

            $("#" + uiID).dialog();
        }
    },

    /*
     * Xóa mask trong container truyền vào
     * phamha
     * */
    showUCHTML(container, cfg) {
        var key = cfg.key,
            uiConfig = UiConfig.find(x => x.key == key);

        if (!PKHA["loaded_" + cfg.key]) {
            if (uiConfig) {
                var id = PKHA.commonFn.uuidv4();
                var uiID = 'dialog_' + id;


                //Nếu đã có rồi thì k load script nữa

                if (!PKHA["loaded_" + cfg.key]) {

                    //load file script trước
                    $.getScript(uiConfig.js, function () {

                    });

                    PKHA["loaded_" + cfg.key] = true;
                }

                //let pageID = (calledObj & calledObj.pageID) ? calledObj.pageID : '';

                //load html
                //let frame = PKHA.commonFn.httpGet("/Common/FrameUCHTML?ucURL=" + uiConfig.ascxLink + "&masterID=" + pageID);
                let frame = PKHA.commonFn.httpGet("/Common/FrameUCHTML?ucURL=" + uiConfig.ascxLink);

                frame = `<div class='frame-uc' id="` + uiID + `" >` + frame + `</div>`;

                if (container) {
                    //container.children().hide();
                    container.append(frame);

                    //gán master vào
                    if (cfg && cfg.jsCaller) {
                        let jsCaller = cfg.jsCaller;

                        //Đặt cha vào cho nó để còn callback lại
                        $(function () {
                            let detailObj = window[cfg.detailObj];

                            detailObj.masterObject = jsCaller;
                            detailObj.loadingCfg = cfg;

                        });


                    }

                }

            }
        }
    },

    /*
     * Lấy chuỗi html
     * */
    httpGet(theUrl) {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("GET", theUrl, false); // false for synchronous request
        xmlHttp.send(null);
        return xmlHttp.responseText;
    },


    isEmpty(value) {
        let isEmpty = false;

        if (value == undefined || value == null || value == '') {
            isEmpty = true;
        }
        return isEmpty;
    },

    /*
     * Gen Guid
     * TODO cái này phải xem lại
     * phamha
     * */
    uuidv4() {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        );
    },


    //#endregion

}


