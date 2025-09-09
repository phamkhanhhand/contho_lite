
/*
 * 
 * 
 */
class FitScreen {


    //
    removeNull(mapperObject) {
        mapperObject.borderTop = mapperObject.borderTop || 0;
        mapperObject.borderBottom = mapperObject.borderBottom || 0;
        mapperObject.borderLeft = mapperObject.borderLeft || 0;
        mapperObject.borderRight = mapperObject.borderRight || 0;

        mapperObject.paddingTop = mapperObject.paddingTop || 0;
        mapperObject.paddingBottom = mapperObject.paddingBottom || 0;
        mapperObject.paddingLeft = mapperObject.paddingLeft || 0;
        mapperObject.paddingRight = mapperObject.paddingRight || 0;

        mapperObject.marginTop = mapperObject.marginTop || 0;
        mapperObject.marginBottom = mapperObject.marginBottom || 0;
        mapperObject.marginLeft = mapperObject.marginLeft || 0;
        mapperObject.marginRight = mapperObject.marginRight || 0;

    }



    /*
     * Hàm đặt số px trên màn hình hiển thị cho layout
     * Chú ý trong hàm này height là bao gồm cả border, margin, padding
     * phamha 25.10.2022
     * */
    fitControl(mapperObject) {
        var me = this;

        if (mapperObject && mapperObject.id) {


            //todo Nếu là fitItself thì tự lấy mapperOjbect
            if (mapperObject.fitItself && App[mapperObject.id].getSizeItself) {
                let fitScreen = mapperObject.fitScreen;
                mapperObject = App[mapperObject.id].getSizeItself();
                mapperObject.fitScreen = fitScreen;
            }

            var tag = $('#' + mapperObject.id);
             
            //todo Nếu là fitScreen
            if (mapperObject.fitScreen) {
                var scrWidth = $(window).width();
                var scrHeight = $(window).height() - 1;//todo chưa hiểu tại sao luôn

                mapperObject.height = mapperObject.height || scrHeight;
                mapperObject.width = mapperObject.width || scrWidth;

            }

            me.removeNull(mapperObject);

            //Đặt cho chính nó
            var innerWidth = (mapperObject.width
                - mapperObject.borderLeft - mapperObject.borderRight
                - mapperObject.paddingLeft - mapperObject.paddingRight
                - mapperObject.marginLeft - mapperObject.marginRight) || 0;

            var innerHeight = (mapperObject.height
                - mapperObject.borderTop - mapperObject.borderBottom
                - mapperObject.paddingTop - mapperObject.paddingBottom
                - mapperObject.marginTop - mapperObject.marginBottom) || 0;

            tag.height(innerHeight);
            tag.width(innerWidth);

            //#region Đặt cho con nó
            if (mapperObject.children) {
                var arrFlexHeightChild = [];
                var sumFixHeight = 0;
                //lọc ra danh sách flex để chia đều. Những cái đã fix sẵn thì cho fit luôn
                mapperObject.children.filter(function (child) {

                    //Tạm thời nếu k có width thì cho bằng width của cha
                    child.width = child.width || innerWidth;

                    if (child.flexHeight) {
                        arrFlexHeightChild.push(child);
                    } else {
                        sumFixHeight += (child.height || 0);
                        me.fitControl(child);
                    }

                });

                //Bắt đầu xử lý cái flexHeight
                if (arrFlexHeightChild.length > 0) {
                    var eachHeight = (innerHeight - sumFixHeight) / arrFlexHeightChild.length;

                    arrFlexHeightChild.filter(function (child) {
                        child.height = eachHeight;

                        me.fitControl(child);
                    });

                }

            }

            //#endregion
        }

    }
}