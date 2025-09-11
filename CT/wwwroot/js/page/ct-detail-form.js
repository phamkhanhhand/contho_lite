/*
 * Màn hình dạng master-nhiều detail
 * phamkhanhhand
 * */

class CTDetailForm extends CTBaseForm {
    /*
     * Tên entity
     * */
    entityName = null

    serviceName = ""

    entityData = {
        Master: {
            EntityName: this.entityName,
            EntityObject: null,
        },
        Details: [

        ]
    }

    /*
     * Sự kiện nút back
     * phamkhanhhand
     * */
    back_Onclick() {
        let me = this;

        if (me.masterObject && me.masterObject.showMasterDetai) {
            me.masterObject.showMasterDetai(true);
        }
    }

    afterBindingComplete() {
        let me = this;

        me.loadForm();
    }

    /*
     * Gán control từ giao diện vào js
     * phamkhanhhand
     * */
    loadForm() {
        let me = this;

        me.loadData();
        me.initMenu();
        me.initControlBase();

    }

    initControlBase() {
        let me = this;

        if (me.btnBack) {
            $('#' + me.btnBack.htmlID).on('click', function (e) {
                me.back_Onclick(e);
            });
        }
    }

    loadData() {
        var me = this;


        //cái này không sync nhé (để đảm bảo load xong dữ liệu rồi mới load data)
        me.getBussinessData();

        //Dữ liệu bình thường
        me.bindData();

        //các control tự load dữ liệu từ server
        //me.bindControls();
        me.setEditMode(PKHA.Enumeration.EditMode.View);
    }

    /*
     * Lấy dữ liệu của màn hình
     * phamkhanhhand
     * */
    getBussinessData() {
        let me = this,
            url = "/api/" + me.serviceName + "/Get?id=203";


        let callBack = function (arg) {
            var data = JSON.parse(arg);

            me.entityData = data;

        };


        PKHA.commonFn.ajax(url, {}, callBack, { isSync: false });

    }

    /*
     * Gán dữ liệu vào control 
     * Sau đó nhờ sự kiện sẽ bind dữ liệu vào control
     * phamkhanhhand
     * */
    initMenu() {
        let me = this;


        //thêm scrope vào cho nó

        $("#" + me.toolBar.htmlID + ".mb-grid-tool-bar li").click(function (event) {
            me.onToolbarItem_Click(event);
        });

    }


    /*
     * Sự kiện nhấn nút click vào menu
     * phamkhanhhand
     * */
    onToolbarItem_Click(arg) {
        var me = this,
            menuID = arg.currentTarget.id,
            menu = me.toolBar.lstMenuItem.find(x => x.id == menuID),
            commandName = menu.Command;


        switch (commandName) {

            case PKHA.Enumeration.CommandName.Save:
                me.save();
                break;

            case PKHA.Enumeration.CommandName.Edit:
                me.setEditMode(PKHA.Enumeration.EditMode.Edit);
                break;

            case PKHA.Enumeration.CommandName.Refresh:
                me.refresh();

                break;

            default:
        }

    }


    /*
     * Bind dữ liệu vào màn hình
     * phamkhanhhand
     * */
    bindData() {
        var me = this;

        PKHA.commonFn.showMask($('.mb-content'));
        me.bindMaster();
        me.bindDetails();

        setTimeout(function () {
            //do what you need here
            PKHA.commonFn.hideMask($('.mb-content'));
        }, 500);


    }

    /*
     * Bind masterdata vào màn hình
     * phamkhanhhand
     * */
    bindMaster() {
        var me = this,
            obj = me.entityData.Master.EntityObject;

        super.bindMaster(obj);
         
        //if (obj) {

        //    var controls = $('[setfield]');

        //    for (var i = 0; i < controls.length; i++) {
        //        var ctr = controls[i];

        //        ctr.value = obj[ctr.getAttribute("setfield")];

        //    }
        //}

    }

    /*
     * Bind masterdata vào màn hình
     * phamkhanhhand
     * */
    bindDetails() {
        var me = this;

    }

    /*
     * Gán dữ liệu vào control 
     * Sau đó nhờ sự kiện sẽ bind dữ liệu vào control
     * phamkhanhhand
     * */
    bindControls() {
        var me = this;

        var controls = $('.mb-combobox'); //todo có 1 trường đánh dấu có load theo base hay không

        //Thử custom sự kiện
        //$('.mb-combobox')[0].onCompleteBinding = function myfunction() {
        //    alert('hhhhhh');
        //}


        for (var i = 0; i < controls.length; i++) {
            var ctr = controls[i];
            //Đối với từng combobox thì load dữ liệu ra
            var url = ctr.getAttribute("url");

            //Nếu có url và có sự kiện bind event
            if (url && ctr.bindevent) {
                PKHA.commonFn.getAndBindControl(url, ctr.bindevent, me.entityData);
            }
        }

    }

    /*
     * Lưu dữ liệu từ form vào db
     * phamkhanhhand
     * */
    save() {
        let me = this;

        //Lấy danh sách ra đây
        let data = me.getEntitySetFromGuid();

        let isValid = me.validateBeforeSave(data);
        //isValid = true;
        if (isValid) {
            //json
            let datazip = me.prepareEntityToPush(data);

            let pro = JSON.stringify(datazip);

            //up lên server
            //PKHA.commonFn.ajax("/api/Contract/Post", { data: pro }, me.saveComplete.bind(me), { method: 'POST' });
            PKHA.commonFn.ajax("/api/" + me.serviceName + "/Post", { data: pro }, me.saveComplete.bind(me), { method: 'POST' });
        }

    }

    /*
     * Validate
     * trả về: true: pass; false: fail
     * phamkhanhhand
     * */
    validateBeforeSave(data) {
        let me = this,
            valid = !(me.validCommon === false);

        return valid;
    }

    /*
     * Xử lý sau khi lưu dữ liệu
     * phamkhanhhand
     * */
    saveComplete(data) {
        let me = this;

        if (me.refresh) {
            me.refresh();
        }
    }

    prepareEntityToPush(entityData) {
        let datazip = entityData;
        datazip.Master.EntityObject = JSON.stringify(entityData.Master.EntityObject);
        datazip.Master = JSON.stringify(entityData.Master);


        //nén cả detail lại
        if (entityData.Details && entityData.Details.length > 0) {
            let lstDetail = entityData.Details;
            let lstDetailAfter = [];

            lstDetail.filter(function (e) {
                if (e.ListObject) {
                    let lstobject = JSON.stringify(e.ListObject);

                    lstDetailAfter.push({

                        EntityName: e.EntityName,
                        ListObject: lstobject,
                    });

                }
            });
            datazip.Details = JSON.stringify(lstDetailAfter);
        }


        return datazip;
    }

    /*
     * Lấy dữ liệu từ form
     * phamkhanhhand
     * */
    getEntitySetFromGuid() {
        let me = this;


        me.entityData = {
            Master: {
                EntityName: me.entityName,
                EntityObject: me.getMasterFromGuid(),
            },
            Details: me.getDetailsFromGuid()
        };

        return me.entityData;
    }

    /*
     * Lấy dữ liệu master form
     * phamkhanhhand
     * */
    getMasterFromGuid() {
        let me = this,
            master = {},
            controls = $('.ct-control [setfield]');

        for (var i = 0; i < controls.length; i++) {
            var ctr = controls[i];
            let setField = $(ctr).attr("setfield");

            //Kiểm tra Nếu require thì phải require, nếu không thì k được

            let pCtrID = $(ctr).attr("ctid");
            let ctrApp = App[pCtrID];

            if (ctrApp.getValue && PKHA.commonFn.isEmpty(ctr.value) && ctrApp && ctrApp.showRequire) {
                ctrApp.showRequire(true);
                me.validCommon = false;
            }

            master[setField] = ctr.value;

        }
        //todelete
        master['OrganizationID'] = 9310;
        master['ContractID'] = 203;



        master['EntityState'] = (me.EditMode == PKHA.Enumeration.EditMode.Add ? PKHA.Enumeration.EntityState.Add : PKHA.Enumeration.EntityState.Edit);
        return master;
    }

    /*
     * Lấy dữ liệu master form
     * Cái này tùy màn hình mà override
     * phamkhanhhand
     * */
    getDetailsFromGuid() {
        let me = this,
            lst = [];
    }

}