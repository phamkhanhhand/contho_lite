
/*
 * Store để cache và thao tác dữ liệu
 * */
class CTStore {
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