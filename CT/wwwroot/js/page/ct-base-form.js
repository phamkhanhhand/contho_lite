/*
 * Màn hình dạng master-nhiều detail
 * phamkhanhhand
 * */

class CTBaseForm {

    pageID

    /*
    * scope to get component: textbox...
    */
    pageScope

    ////ID page

    /*
    * Truyền arg thôi, chứ nhiều tham số phức tạp, javascript chỉ cho 1 constructor
    */
    constructor(arg) {
        this.pageID = PKHA.commonFn.uuidv4();
        this.pageScope = arg?.pageScope;


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



    /*
    * Auto bind not need map
    * Tự động bind không cần map
    * phamkhanhhand Sep 11, 2025
    */
    configControlsAutoConext() {
        let me = this;
        let mapping =[];
         

        //get all 
        let allControlPage = $("[ct-page='" + me.pageScope + "']");

        allControlPage.filter(function (ix)  {
             
            let x = allControlPage[ix];

            let field = $(x).attr('ct-name');


            let obj = {
               
            };

            obj[field] =  $(x).attr('id');

            mapping.push(obj);
        });

         

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
    loadData() {

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
        let allControl = $('.ct-control');

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
    bindMaster(entity, bindingField ="ctsetfield") {
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