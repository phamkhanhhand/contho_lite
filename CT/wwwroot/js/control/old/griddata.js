
/*
 * Store để cache và thao tác dữ liệu
 * */
class Store {
    //dạng mảng của {origin, selection}
    data = []

    /*
     * Đã update data lên các cái mảng chưa
     * Cho đỡ phải đi lại nhiều lần. cái này bên ngoài không gọi, chỉ check nội bộ thôi
     * */
    isUpdateModifyInfo = true

    /*
     * Các dữ liệu thay đổi (thêm/sửa/xóa)
     * */
    _dataChange = []

    constructor(data) {
        var me = this;

        me.bindData(data);
    }

    structuredClone(obj) {
        let cl = JSON.stringify(obj);

        return JSON.parse(cl);

    }

    bindData(data) {
        let me = this;


        me.data = [];
        //validate 
        data = data || [];

        data.filter(function (item) {

            item.mbguidid = NSP.commonFn.uuidv4();
            //var cloneItem = structuredClone(item);
            var cloneItem = me.structuredClone(item);

            var itemStore = {
                origin: item,
                selection: cloneItem,
                EntityState: NSP.Enumeration.EntityState.None,
                mbguidid: item.mbguidid,
            }

            me.data.push(itemStore);

        });
    }

    /*
     * Thêm
     * */
    insert(item) {
        var me = this;

        item.mbguidid = NSP.commonFn.uuidv4();
        item.EntityState = NSP.Enumeration.EntityState.Add;

        var dataItem = {
            origin: null,
            selection: item,
            EntityState: NSP.Enumeration.EntityState.Add,
            //ai bắt đâu nhề
            mbguidid: item.mbguidid,
        }

        me.data.push(dataItem);

        me.isUpdateModifyInfo = false;
    }

    /*
     * xóa
     * */
    delete(item) {
        var me = this;

        var id = item.mbguidid;
        var currentItem = me.data.find(x => x.mbguidid == id);

        if (currentItem.EntityState == NSP.Enumeration.EntityState.Add) {
            //Nếu là thêm thì

            //bỏ luôn ở data
            me.data = me.data.filter(function (e) {
                return e.mbguidid != id;
            });

        } else {
            //cập nhật thôi
            currentItem.EntityState = NSP.Enumeration.EntityState.Delete;
            //Bổ sung luôn cho nó tiện
            currentItem.selection['EntityState'] = NSP.Enumeration.EntityState.Delete;
        }

        me.isUpdateModifyInfo = false;
    }

    /*
     * Cập nhật
     * (Hàm này có dùng đâu nhờ)
     * */
    update(item) {
        var me = this;

        var id = item.mbguidid;
        var currentItem = me.data.find(x => x.mbguidid == id);

        currentItem.selection = item;

        //Nếu là thêm thì cập nhật chỗ thêm, nếu là xóa thì cập nhật chỗ xóa
        if (currentItem.EntityState != NSP.Enumeration.EntityState.Add) {
            //cập nhật các trường vào
            currentItem.EntityState = NSP.Enumeration.EntityState.Edit;
            //Bổ sung luôn cho nó tiện
            currentItem.selection['EntityState'] = NSP.Enumeration.EntityState.Edit;
        }

        me.isUpdateModifyInfo = false;
    }

    /*
     * Cập nhật
     * */
    updateField(item, field) {
        var me = this;

        var id = item.mbguidid;
        var currentItem = me.data.find(x => x.mbguidid == id);

        currentItem.selection[field] = item[field];

        //Nếu là thêm thì cập nhật chỗ thêm, nếu là xóa thì cập nhật chỗ xóa
        if (currentItem.EntityState != NSP.Enumeration.EntityState.Add) {
            //cập nhật các trường vào
            currentItem.EntityState = NSP.Enumeration.EntityState.Edit;
            //Bổ sung luôn cho nó tiện
            currentItem.selection['EntityState'] = NSP.Enumeration.EntityState.Edit;
        }

        me.isUpdateModifyInfo = false;
    }

    /*
     * Lấy tất cả thay đổi (thêm/sửa/xóa)
     * */
    getAllChange() {
        var me = this,
            dataChange = me._dataChange;

        //Nếu chưa update thì phải cập nhật lại
        if (!me.isUpdateModifyInfo) {

            me._dataChange = me.data.filter(function (e) {
                return e.EntityState && e.EntityState != NSP.Enumeration.EntityState.None;
            });

            me.isUpdateModifyInfo = true;

            dataChange = me._dataChange;
        }

        return dataChange;
    }

    /*
     * Lấy những cái insert
     * */
    getInserted() {
        var me = this,
            allChange = me.getAllChange(),
            inserted = [];

        inserted = allChange.filter(x => x.EntityState == NSP.Enumeration.EntityState.Add);

        return inserted;
    }

    /*
     * Lấy những cái insert
     * */
    getUpdated() {
        var me = this,
            allChange = me.getAllChange(),
            updated = [];

        updated = allChange.filter(x => x.EntityState == NSP.Enumeration.EntityState.Edit);

        return updated;
    }

    /*
     * Lấy những cái insert
     * */
    getDeleted() {
        var me = this,
            allChange = me.getAllChange(),
            deleted = [];

        deleted = allChange.filter(x => x.EntityState == NSP.Enumeration.EntityState.Edit);

        return deleted;
    }
    //todo getpaing
}




/*
 * Grid
 * */
class GridData extends BaseControl {
    //Tên entity bind vào (để build cột)
    EntityName = null

    store = new Store();

    lstColumn = null
    url = null
    height = null
    menuTool = null
    customMenuID = "grdList_menu" + this.htmlID
    customMenuHeight = 31

    /*
     * Lấy ra các thành phần config để set size
     * */
    getSizeObject() {
        var me = this,
            children = [

                {
                    id: "grdList_header" + me.htmlID,
                    height: 26
                },
                {
                    id: "grdList_body" + me.htmlID,
                    flexHeight: true,
                    //top: (23),
                    //bottom: (28),//padding//border, còn số 5 là để bù trừ linh tinh
                },
                {
                    id: "grdList_paging" + me.htmlID,
                    height: 40,
                    padding: 10,//pading4 + border1
                }
            ];
         
        if (this.menuTool) {
            children.push({
                id: me.customMenuID,
                height: me.customMenuHeight,
            });
        }

        return {
            id: me.htmlID,
            children: children,
        };

    }
     
    /*
     * Danh sách cột
     * phamha
     * phamha
     * */
    getListColumnVisibl() {
        var me = this;

        return me.lstColumn;

    }


    loadData(lst) {
        let me = this;

        me.store.bindData(lst);

        me.initGrid();
    }

    /*
     * Load data from serrver
     * phamha
     * */
    getDataFromServer(callbackFunct) {
        var me = this,
            url = me.url;

        if (callbackFunct) {
            NSP.commonFn.ajax(url, {}, callbackFunct.bind(me), { isSync: true });
        } else {
            NSP.commonFn.ajax(url, {}, me.loadDataComplete.bind(me), { isSync: true });
        }

    }

    //load complete
    //load complete before binddata
    //bindding data

    loadDataComplete(data) {
        var me = this;

        if (data) {
            var paging = JSON.parse(data);

            var lstObject = paging.ListObject;

            if (lstObject) {
                //lstObject = JSON.parse(lstObject);

                //this.store = new Store(lstObject);
                me.store = new Store(lstObject);

                me.initGrid();
            }

        }

    }


    initGrid() {
        var me = this,
            gridHtml = $("#" + me.htmlID + " .mb-grid-body tbody");

        me.bindToGrid(gridHtml, me.store);

    }


    //Các hàm ẩn hiện cột

    bindToGrid(grd, store) {
        var me = this,
            lstColumn = me.getListColumnVisibl();

        me.store = store;

        grd.html('');

        if (lstColumn) {
            //Lấy danh sách dữ liệu
            var data = store.data.map(x => x.selection);

            if (data && data.length > 0) {
                data.filter(function (item) {

                    var row = $("<tr data-entity-id='" + item.mbguidid + "'></tr>");

                    for (var j = 0; j < lstColumn.length; j++) {

                        var colItem = lstColumn[j];
                        var fieldValue = item[colItem.DataIndex];


                        var innerValue = colItem.render ? colItem.render(item, fieldValue, colItem.ColumnType) : (fieldValue || '');

                        var tdHTML = $("<td dataindex='" + colItem.DataIndex + "'>" + innerValue + "</td>");

                        //Build từng column
                        var col = tdHTML;

                        col.appendTo(row);

                    }

                    //Gán luôn data vào thẻ để khỏi phải tìm
                    //row.rawDataHide = item; 
                    row.data('newDataHide', item);

                    row.appendTo(grd);
                });
            }

            me.createEvent();

        }//Nếu 0 có entity
        else {

        }

        me.bindComplete();
    }

    /*
     * Cho sửa hay không
     * phamha
     * */
    setEnable(enable) {
        let me = this;

        me.editable = enable;
    }

    /*
     * Sau khi gán data vào xong
     * phamha
     * */
    bindComplete() {
        var me = this,
            gridBody = $("#grdList_body" + me.htmlID + ""),
            gridHeader = $("#grdList_header" + me.htmlID + "");

        if (gridBody.prop('scrollHeight') > gridBody.height()) {

            $(gridHeader).css({
                overflowY: 'scroll'
            });

        }
        else {

            gridHeader.css({
                overflowY: 'none'
            });

        }

    }


    //Theem evnet cho grid

    createEvent() {
        var me = this;

        $("td").click(function (event) {
            if (!me.editable) {
                return;
            }
            var tdObj = $(this);
            //Kiểm tra xem nó thuộc loại nào

            if (false) {
                if ($(this).children("select").length > 0)
                    return false;
                //Thử bind combo lên xem nhá

                var tagParent = this;//td
                var par = tagParent.parentElement;//tr

                tdObj.html("");

                var parentTRID = 'tr[data-entity-id="' + $(par).data('entity-id') + '"]' + " > td[dataindex='name']";

                var combobEnume = new ComboboxEnum();
                combobEnume.createTag(parentTRID, "OrganSub");

            }
            else {
                //nếu đã có rồi thì thôi
                if ($(this).children("input").length > 0)
                    return false;



                //Giá trị org
                var preText = tdObj.html();
                //đối tượng input
                var inputObj = $("<input type='text' />");

                ///Xóa giá trị ban đầu
                tdObj.html("");


                inputObj.width(tdObj.width())

                    //Style
                    //.height(tdObj.height())
                    .height("100%")
                    .width("100%")
                    .css({ border: "0px", fontSize: "17px" })

                    //đặt giá trị cũ vào
                    .val(preText)

                    //Gán làm con của TD
                    .appendTo(tdObj)

                    //Focus và select cái text vào (tức chọn text trong đấy luôn)
                    .trigger("focus")
                    .trigger("select");


                //Sự kiện gõ
                inputObj.keyup(function (event) {

                    if (13 == event.which) { // press ENTER-key
                        var text = $(this).val();
                        tdObj.html(text);
                    }
                    else if (27 == event.which) {  // press ESC-key
                        tdObj.html(preText);
                    }
                });

                //td
                var tagParent = this;

                //Sự kiện leave control ra ngoài
                inputObj.focusout(function (e) {
                    //So sánh giá trị hiện tại với giá trị vừa rồi. Nếu thay đổi thì tức là Cập nhật. Đánh dấu object là cập nhật

                    var par = tagParent.parentElement;//tr
                    var thisTag = $(this);

                    var tagDataNew = $(par).data('newDataHide');

                    var guiID = $(par).data('entity-id');

                    //dataindex của ô hiện tại
                    let dataindex = $(tagParent).attr('dataindex');

                    //Giá trị cũ ban đầu
                    var oldVal = tagDataNew[dataindex];
                    var newVal = $(this).val();


                    if (oldVal != newVal) {
                        //cập nhật store ở đây
                        var updatedItem = {
                            mbguidid: guiID,
                            //name: newVal
                        }
                        updatedItem[dataindex] = newVal;

                        me.store.updateField(updatedItem, dataindex);

                        //tagData.Updated = true;
                        //tagDataNew.EntityState = 1;
                        //tagDataNew.name = newVal;

                    }
                });


                inputObj.click(function () {
                    return false;
                });
            }
        });
    }
}


/*
 * Cột (để sử dụng các tính năng của cột)
 * phamha
 * */
class Column {

    constructor(obj) {

        Object.assign(this, obj);
    }

    render(item, value, datatype) {
        var me = this;

        switch (datatype) {

            case NSP.Enumeration.ColumnType.DateTime:
                value = (value ? new Date(value) : null);
                value = value ? (value.getDate() + "/" + (value.getMonth() + 1) + "/" + value.getFullYear()) : null;
                break;

            case NSP.Enumeration.ColumnType.Link:
                value = '<a href="' + me.Url.format(item.ID) + '">' + value + '</a>';
                break;

            default:
        }

        return value;
    }
}

/*
 * Cột (để sử dụng các tính năng của cột)
 * phamha
 * */
class DateTimeColumn extends Column {
    render(value) {
        var me = this;

        value = (value ? new Date(value) : null);


        value = value ? (value.getDate() + "/" + (value.getMonth() + 1) + "/" + value.getFullYear()) : null;


        return value;
    }
}
