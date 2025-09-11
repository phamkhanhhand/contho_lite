
/*
 * Cột (để sử dụng các tính năng của cột)
 * phamkhanhhand
 * */
class CTColumn {

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
 * phamkhanhhand
 * */
class CTDateTimeColumn extends CTColumn {
    render(value) {
        var me = this;

        value = (value ? new Date(value) : null);


        value = value ? (value.getDate() + "/" + (value.getMonth() + 1) + "/" + value.getFullYear()) : null;


        return value;
    }
}
