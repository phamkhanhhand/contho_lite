/*
 * Màn hình dạng master-nhiều detail
 * phamkhanhhand
 * */

class CTBaseForm {


    //ID page
    constructor() {
        this.pageID = PKHA.commonFn.uuidv4();


        if (typeof App == 'undefined') {
            App = {};
        }

        //App[this.pageID] = this;
    }

    /*
     * Gán control từ giao diện vào js
     * phamkhanhhand
     * */
    configControls(mapping) {
        let me = this;

        if (mapping && Array.isArray(mapping)) {

            mapping.filter(function (e) {
                me[Object.keys(e)[0]] = App[Object.values(e)[0]];
            });
        }

        me.afterBindingComplete();

    }

    ///*
    //* Config mapping control và gọi các hàm khởi động của page
    //* phamkhanhhand 05.11.2022
    //* */
    //config(mappingConfig) {
    //    let me = this;

    //    for (var i = 0; i < mappingConfig.length; i++) {
    //        let map = mappingConfig[i];
    //        let key = Object.keys(map)[0];

    //        me[key] = App[map[key]];

    //    }
    //}



    afterBindingComplete() {

    }

    refresh() {
        let me = this;
        me.loadData();
    }

    /*
     * Đặt editmode cho form
     * phamkhanhhand
     * */
    setEditMode(editmode) {
        let me = this;

        me.EditMode = editmode;

        switch (editmode) {
            case PKHA.Enumeration.EditMode.View:
                me.setEnableControl(false);
                break;
            case PKHA.Enumeration.EditMode.Add:
            case PKHA.Enumeration.EditMode.Edit:
                me.setEnableControl(true);
                break;
            default:
        }
    }

    /*
     * Đặt enable/disable control cho toàn màn hình
     * phamkhanhhand
     * */
    setEnableControl(enable) {
        let me = this;

        //Lần từng cái một, nếu là control thì disable hết đi (control của custom nhé)
        let allControl = $('.mb-control');

        //foreach để gọi hàm disable của nó (coi kiểu đối tượng)
        for (var i = 0; i < allControl.length; i++) {
            let tg = allControl[i],
                ctr = App[tg.id];
            if (ctr && ctr.setEnable) {

                ctr.setEnable(enable);
            }
        }

    }



    /*
     * Bind masterdata vào màn hình
     * phamkhanhhand
     * */
    bindMaster(entity, bindingField ="khsetfield") {
        var me = this;

        if (entity) {

            var controls = $(`[${bindingField}]`);

            for (var i = 0; i < controls.length; i++) {
                var ctr = controls[i];

                ctr.value = entity[ctr.getAttribute(bindingField)];

            }
        }

    }

}