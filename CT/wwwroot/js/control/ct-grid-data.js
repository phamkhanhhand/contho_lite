/*
 * Grid
 * */
class CTGridData extends CTBaseControl {
    //Tên entity bind vào (để build cột)
    EntityName = null

    store = new CTStore();

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
     * phamkhanhhand 
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
     * phamkhanhhand
     * */
    getDataFromServer(callbackFunct) {
        var me = this,
            url = me.url;

        if (callbackFunct) {
            PKHA.commonFn.ajax(url, {}, callbackFunct.bind(me), { isSync: true });
        } else {
            PKHA.commonFn.ajax(url, {}, me.loadDataComplete.bind(me), { isSync: true });
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
                me.store = new CTStore(lstObject);

                me.initGrid();
            }

        }

    }


    initGrid() {
        var me = this,
            gridHtml = $("#" + me.htmlID + " .ct-grid-body tbody");

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
     * phamkhanhhand
     * */
    setEnable(enable) {
        let me = this;

        me.editable = enable;
    }

    /*
     * Sau khi gán data vào xong
     * phamkhanhhand
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

 